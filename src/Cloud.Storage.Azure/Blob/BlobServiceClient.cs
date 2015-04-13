using Cloud.Storage.Azure.Retries;
using Cloud.Storage.Blob;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Blob
{
    public class BlobServiceClient : IBlobStorageClient
    {
        internal BlobServiceClient(CloudBlobClient azureBlobClient)
        {
            AzureBlobClient = azureBlobClient;
            AzureBlobClient.RetryPolicy = new StorageRetryPolicy(new Incremental(3, TimeSpan.FromMilliseconds(25D), TimeSpan.FromMilliseconds(25D)));
        }

        public async Task<Blob> GetBlob(string uri)
        {
            return await GetBlob(new Uri(uri));
        }

        public async Task<Blob> GetBlob(Uri uri)
        {
            var blobReference = await AzureBlobClient.GetBlobReferenceFromServerAsync(uri) as CloudBlockBlob;
            return new Blob(blobReference);
        }

        public IContainer CreateContainerIfNotExists(string name)
        {
            var container = AzureBlobClient.GetContainerReference(name);
            container.CreateIfNotExists();

            return new Container(container);
        }

        public Container GetContainer(string name)
        {
            var container = AzureBlobClient.GetContainerReference(name);

            return new Container(container);
        }

        private CloudBlobClient AzureBlobClient { get; set; }
    }
}
