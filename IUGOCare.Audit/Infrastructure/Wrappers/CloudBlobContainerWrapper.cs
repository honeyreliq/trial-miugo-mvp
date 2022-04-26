using Microsoft.Azure.Storage.Blob;

namespace IUGOCare.Audit.Infrastructure.Wrappers
{
    public class CloudBlobContainerWrapper : ICloudBlobContainer
    {
        private readonly CloudBlobContainer _container;

        public CloudBlobContainerWrapper(CloudBlobContainer container)
        {
            _container = container;
        }

        public ICloudAppendBlob GetAppendBlobReference(string blobName)
        {
            return new CloudAppendBlobWrapper(_container.GetAppendBlobReference(blobName));
        }
    }
}
