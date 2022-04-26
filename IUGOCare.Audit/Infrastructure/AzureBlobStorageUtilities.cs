using System;
using IUGOCare.Audit.Constants;
using IUGOCare.Audit.Infrastructure.Wrappers;

namespace IUGOCare.Audit.Infrastructure
{
    public class AzureBlobStorageUtilities : IAzureBlobStorageUtilities
    {
        private readonly ICloudBlobContainer _container;

        public AzureBlobStorageUtilities(ICloudBlobContainer container)
        {
            _container = container;
        }

        public ICloudAppendBlob GetCurrentApiAuditBlobReference(string blobName)
        {
            ICloudAppendBlob blob = _container.GetAppendBlobReference(blobName);
            if (!blob.Exists())
            {
                blob.CreateOrReplace();
                blob.AppendText($"{DelimitedFileHeaders.ApiAuditModelPipeDelimited}{Environment.NewLine}");
            }
            return blob;
        }
    }
}
