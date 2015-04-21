using Cloud.Storage.Azure.Blob;
using Cloud.Storage.Azure.Queue;
using Cloud.Storage.Azure.Table;
using Cloud.Storage.Blob;
using Cloud.Storage.Queue;
using Cloud.Storage.Table;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Configuration;
using System.Net;

namespace Cloud.Storage.Azure
{
	public class StorageClient : IStorageClient
	{
		static StorageClient()
		{
			StorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureStorageConnectionString"]);

			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 100;
		}
		
		public IBlobStorageClient Blobs { get { return mBlobs.Value; } }
		public IQueueStorageClient Queues { get { return mQueues.Value; } }
		public ITableStorageClient Tables { get { return mTables.Value; } }
		
		internal static CloudStorageAccount StorageAccount { get; set; }
		internal static CloudBlobClient BlobClient { get { return mBlobClient.Value; } }
		internal static CloudTableClient TableClient { get { return mTableClient.Value; } }
		internal static CloudQueueClient QueueClient { get { return mQueueClient.Value; } }

		private static Lazy<IBlobStorageClient> mBlobs = new Lazy<IBlobStorageClient>(() => new BlobStorageClient());
		private static Lazy<IQueueStorageClient> mQueues = new Lazy<IQueueStorageClient>(() => new QueueStorageClient());
		private static Lazy<ITableStorageClient> mTables = new Lazy<ITableStorageClient>(() => new TableStorageClient());


		private static Lazy<CloudBlobClient> mBlobClient = new Lazy<CloudBlobClient>(() => StorageAccount.CreateCloudBlobClient());
		private static Lazy<CloudTableClient> mTableClient = new Lazy<CloudTableClient>(() => StorageAccount.CreateCloudTableClient());
		private static Lazy<CloudQueueClient> mQueueClient = new Lazy<CloudQueueClient>(() => StorageAccount.CreateCloudQueueClient());
	}
}
