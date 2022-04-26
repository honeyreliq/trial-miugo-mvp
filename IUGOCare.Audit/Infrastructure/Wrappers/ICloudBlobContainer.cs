namespace IUGOCare.Audit.Infrastructure.Wrappers
{
    public interface ICloudBlobContainer
    {
        ICloudAppendBlob GetAppendBlobReference(string filename);
    }
}
