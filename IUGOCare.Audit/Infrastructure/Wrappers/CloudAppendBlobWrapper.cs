using Microsoft.Azure.Storage.Blob;

namespace IUGOCare.Audit.Infrastructure.Wrappers
{
    public class CloudAppendBlobWrapper : ICloudAppendBlob
    {
        private readonly CloudAppendBlob _blob;

        public CloudAppendBlobWrapper(CloudAppendBlob blob)
        {
            _blob = blob;
        }

        public void AppendText(string content)
        {
            _blob.AppendText(content);
        }

        public void CreateOrReplace()
        {
            _blob.CreateOrReplace();
        }

        public bool Exists()
        {
            return _blob.Exists();
        }
    }
}
