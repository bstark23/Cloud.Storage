using Cloud.Storage.Azure.Blob;
using Cloud.Storage.Blob;
using Cloud.Storage.Queue;
using Cloud.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System.Configuration;
using System.Net;

namespace Cloud.Storage.Azure
{
	public class StorageClient : IStorageClient
	{
		public StorageClient()
			: this(ConfigurationManager.AppSettings["StorageAccountName"], ConfigurationManager.AppSettings["StorageAccountKey"])
		{
		}
		public StorageClient(string storageAccountName, string storageAccountKey)
		{
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 100;

			bool useHttps = string.Equals(storageAccountName, "devstoreaccount1") ? false : true;

			var storageCredentials = new StorageCredentials(storageAccountName, storageAccountKey);
			AzureStorageAccount = new CloudStorageAccount(storageCredentials, useHttps);

			Blobs = new BlobStorageClient(AzureStorageAccount.CreateCloudBlobClient());
		}

		public IBlobStorageClient Blobs { get; private set; }
		public IQueueStorageClient Queues { get; private set; }
		public ITableStorageClient Tables { get; private set; }


		public static StorageClient GetDefaultClient() { return new StorageClient(); }
		public static StorageClient GetClient(string storageAccountName, string storageAccountKey)
		{
			return new StorageClient(storageAccountName, storageAccountKey);
		}

		private CloudStorageAccount AzureStorageAccount { get; set; }
	}
}
