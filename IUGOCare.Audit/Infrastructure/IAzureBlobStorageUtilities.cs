using IUGOCare.Audit.Infrastructure.Wrappers;

namespace IUGOCare.Audit.Infrastructure
{
    public interface IAzureBlobStorageUtilities
    {
        ICloudAppendBlob GetCurrentApiAuditBlobReference(string blobName);
    }
}
