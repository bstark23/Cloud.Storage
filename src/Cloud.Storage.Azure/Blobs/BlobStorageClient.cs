using Cloud.Storage.Azure.Retries;
using Cloud.Storage.Blobs;
using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Blobs
{
	public class BlobStorageClient : IBlobStorageClient
	{
		internal BlobStorageClient()
		{
			BlobClient.DefaultRequestOptions.RetryPolicy = new StorageRetryPolicy(new Incremental(3, TimeSpan.FromMilliseconds(25D), TimeSpan.FromMilliseconds(25D))); ;
		}

		public async Task<Blob> GetBlob(string uri)
		{
			return await GetBlob(new Uri(uri));
		}

		public async Task<Blob> GetBlob(Uri uri)
		{
			var blobReference = await BlobClient.GetBlobReferenceFromServerAsync(uri) as CloudBlockBlob;
			return new Blob(blobReference);
		}

		public async Task<IContainer> GetContainer(string name, bool createIfNotExists)
		{
			var container = BlobClient.GetContainerReference(name);

			if (createIfNotExists)
			{
				await container.CreateIfNotExistsAsync();
			}

			return new Container(container);
		}

		public static CloudBlobClient BlobClient { get { return mBlobClient.Value; } }
		private static Lazy<CloudBlobClient> mBlobClient = new Lazy<CloudBlobClient>(() => StorageClient.StorageAccount.CreateCloudBlobClient());

	}
}
