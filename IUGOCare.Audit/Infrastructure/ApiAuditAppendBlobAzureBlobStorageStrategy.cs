using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using IUGOCare.Audit.Models;
using IUGOCare.Audit.Repositories;
using log4net;
using Microsoft.Azure.Storage;
using Polly;

namespace IUGOCare.Audit.Infrastructure
{
    public class ApiAuditAppendBlobAzureBlobStorageStrategy : IApiAuditAzureBlobStorageStrategy
    {
        private readonly BlockingCollection<ApiAudit> _queue = new BlockingCollection<ApiAudit>();
        private readonly IAzureBlobNameGenerator _blobNameGenerator;
        private readonly IAzureBlobStorageUtilities _utilities;
        private readonly string _clientIdentifier;
        private readonly IApiAuditFailoverRepository _failoverRepository;
        private readonly int _retryIntervalInMilliseconds;
        private static readonly ILog log = LogManager.GetLogger(typeof(ApiAuditAppendBlobAzureBlobStorageStrategy));
        private readonly ManualResetEvent _isEmpty = new ManualResetEvent(false);

        public ApiAuditAppendBlobAzureBlobStorageStrategy(IAzureBlobNameGenerator blobNameGenerator, IAzureBlobStorageUtilities utilities,
            IApiAuditFailoverRepository failoverRepository, string clientIdentifier, int retryIntervalInMilliseconds = 0)
        {
            _blobNameGenerator = blobNameGenerator;
            _utilities = utilities;
            _clientIdentifier = clientIdentifier;
            _failoverRepository = failoverRepository;
            _retryIntervalInMilliseconds = retryIntervalInMilliseconds;
            Task.Factory.StartNew(RunBlobWriter); // Spawn a background thread to read from the queue and write to the Blob
        }

        public bool Add(ApiAudit apiAuditModel)
        {
            _queue.Add(apiAuditModel);
            return true;
        }

        public bool Flush()
        {
            _queue.CompleteAdding();
            while (!_isEmpty.WaitOne(1000))
            {
                log.Info("Waiting for ApiAudit queue to empty");
            }
            log.Info("ApiAudit queue is empty");

            return true;
        }

        private void RunBlobWriter()
        {
            foreach (ApiAudit apiAuditModel in _queue.GetConsumingEnumerable())
            {
                var retryCount = 3;
                var fallbackPolicy = Policy
                    .Handle<Exception>()
                    .Fallback(
                        () =>
                        {
                            _failoverRepository.Append(apiAuditModel);
                            log.Warn("Persisted ApiAudit log to database.");
                        },
                        (exception) =>
                        {
                            log.Error("Unable to persist ApiAudit log to Azure Blob Storage.", exception);
                        }
                    );

                var retryPolicy = Policy
                    .Handle<Exception>()
                    .WaitAndRetry(
                        retryCount,
                        retryAttempt => {
                            int interval = (_retryIntervalInMilliseconds > 0) ? _retryIntervalInMilliseconds : retryAttempt * 1000;
                            return TimeSpan.FromMilliseconds(interval);
                        },
                        (exception, timeSpan, retries, context) =>
                        {
                            if (retryCount != retries)
                            {
                                if (exception is StorageException ex)
                                {
                                    if (ex.RequestInformation?.HttpStatusCode == (int)HttpStatusCode.Conflict)
                                    {
                                        // something may be wrong with the blob, 
                                        // increment the fileNumber postfix so it can create a new blob
                                        _blobNameGenerator.IncrementFileNumber();
                                    }
                                }
                                return;
                            }
                        });

                var policyWrap = fallbackPolicy.Wrap(retryPolicy);

                policyWrap.Execute(() => AppendToBlob(apiAuditModel.ToDelimitedString("|")));
            }
            _isEmpty.Set();
        }
        private void AppendToBlob(string msg)
        {
            var blobName = _blobNameGenerator.Generate(_clientIdentifier, DateTime.UtcNow);
            var blob = _utilities.GetCurrentApiAuditBlobReference(blobName);
            blob.AppendText(msg);
        }
    }
}
