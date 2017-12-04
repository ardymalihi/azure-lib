using System.IO;
using System.Threading.Tasks;

namespace Azure.Lib.BlobStorage
{
    public interface IBlobStorageClient
    {
        Task UploadFile(string fileName, string blobName);

        Task UploadStream(Stream stream, string blobName);

        Task UploadText(string text, string blobName);

        Task UploadObject(object obj, string blobName);

        Task DownloadFile(string blobName, string fileName);

        Task<Stream> DownloadStream(string blobName);

        Task<string> DownloadText(string blobName);

        Task<T> DownloadObject<T>(string blobName);

        Task Delete(string blobName);

        Task<bool> Exists(string blobName);
        
    }
}
