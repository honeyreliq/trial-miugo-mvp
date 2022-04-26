namespace IUGOCare.Audit.Infrastructure.Wrappers
{
    public interface ICloudAppendBlob
    {
        void CreateOrReplace();
        void AppendText(string content);
        bool Exists();
    }
}
