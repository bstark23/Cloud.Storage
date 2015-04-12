using Cloud.Storage.Azure.Blob;
using Cloud.Storage.Blob;
using Cloud.Storage.Queue;
using Cloud.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure
{
	public class StorageServiceClient : IStorageClient
	{
		public StorageServiceClient()
			: this(ConfigurationManager.AppSettings["StorageAccountName"], ConfigurationManager.AppSettings["StorageAccountKey"])
		{
		}
		public StorageServiceClient(string storageAccountName, string storageAccountKey)
		{
			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 100;

			bool useHttps = string.Equals(storageAccountName, "devstoreaccount1") ? false : true;

			var storageCredentials = new StorageCredentials(storageAccountName, storageAccountKey);
			AzureStorageAccount = new CloudStorageAccount(storageCredentials, useHttps);

			Blobs = new BlobServiceClient(AzureStorageAccount.CreateCloudBlobClient());
		}

		public IBlobStorageClient Blobs { get; private set; }
		public IQueueStorageClient Queues { get; private set; }
		public ITableStorageClient Tables { get; private set; }


		public static StorageServiceClient GetDefaultClient() { return new StorageServiceClient(); }
		public static StorageServiceClient GetClient(string storageAccountName, string storageAccountKey)
		{
			return new StorageServiceClient(storageAccountName, storageAccountKey);
		}

		private CloudStorageAccount AzureStorageAccount { get; set; }
	}
}
