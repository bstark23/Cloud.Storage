using Cloud.Storage.Azure.Retries;
using Cloud.Storage.Blob;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Blob
{
	public class BlobStorageClient : IBlobStorageClient
	{
		internal BlobStorageClient()
		{
			StorageClient.BlobClient.DefaultRequestOptions.RetryPolicy = new StorageRetryPolicy(new Incremental(3, TimeSpan.FromMilliseconds(25D), TimeSpan.FromMilliseconds(25D))); ;
		}

		public async Task<Blob> GetBlob(string uri)
		{
			return await GetBlob(new Uri(uri));
		}

		public async Task<Blob> GetBlob(Uri uri)
		{
			var blobReference = await StorageClient.BlobClient.GetBlobReferenceFromServerAsync(uri) as CloudBlockBlob;
			return new Blob(blobReference);
		}

		public IContainer CreateContainerIfNotExists(string name)
		{
			var container = StorageClient.BlobClient.GetContainerReference(name);
			container.CreateIfNotExists();

			return new Container(container);
		}

		public Container GetContainer(string name)
		{
			var container = StorageClient.BlobClient.GetContainerReference(name);

			return new Container(container);
		}
	}
}
