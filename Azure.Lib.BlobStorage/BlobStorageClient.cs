using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Lib.BlobStorage
{
    public class BlobStorageClient
    {
        private BlobStorageSettings _settings;

        public BlobStorageClient(BlobStorageSettings settings)
        {
            _settings = settings;
        }

        private async Task<CloudBlobContainer> GetContainerAsync()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(_settings.StorageAccount, _settings.StorageKey), false);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_settings.ContainerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }

        public async Task UploadFile(string fileName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.UploadFromFileAsync(fileName);
        }

        public async Task UploadStream(Stream stream, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.UploadFromStreamAsync(stream);
        }

        public async Task UploadText(string text, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.UploadTextAsync(text);
        }

        public async Task UploadObject(object obj, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            var text = JsonConvert.SerializeObject(obj);
            await blockBlob.UploadTextAsync(text);
        }

        public async Task DownloadFile(string blobName, string fileName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.DownloadToFileAsync(fileName, FileMode.Create);
        }

        public async Task<Stream> DownloadStream(string blobName)
        {
            Stream downloadedStream = new MemoryStream();
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.DownloadToStreamAsync(downloadedStream);
            return downloadedStream;
        }

        public async Task<string> DownloadText(string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            return await blockBlob.DownloadTextAsync();
        }

        public async Task<T> DownloadObject<T>(string blobName) 
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            var text = await blockBlob.DownloadTextAsync();
            T obj = JsonConvert.DeserializeObject<T>(text);
            return obj;
        }

        public async Task Delete(string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            await blockBlob.DeleteIfExistsAsync();
        }

        public async Task<bool> Exists(string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            return await blockBlob.ExistsAsync();
        }
    }
}
