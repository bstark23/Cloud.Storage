using Cloud.Storage.Azure.Blobs;
using Cloud.Storage.Azure.Queues;
using Cloud.Storage.Azure.Tables;
using Cloud.Storage.Blobs;
using Cloud.Storage.Queues;
using Cloud.Storage.Tables;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Configuration;
using System.Net;

namespace Cloud.Storage.Azure
{
	public class StorageClient : IStorageClient
	{
		static StorageClient()
		{
			StorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageAccountConnectionString"]);

			ServicePointManager.Expect100Continue = false;
			ServicePointManager.DefaultConnectionLimit = 100;
		}
		
		public IBlobStorageClient Blobs { get { return mBlobs.Value; } }
		public IQueueStorageClient Queues { get { return mQueues.Value; } }
		public ITableStorageClient Tables { get { return mTables.Value; } }
		
		public static IStorageClient CloudStorage { get { return mCloudStorage.Value; } }
		public static CloudStorageAccount StorageAccount { get; set; }

		private static Lazy<IBlobStorageClient> mBlobs = new Lazy<IBlobStorageClient>(() => new BlobStorageClient());
		private static Lazy<IQueueStorageClient> mQueues = new Lazy<IQueueStorageClient>(() => new QueueStorageClient());
		private static Lazy<ITableStorageClient> mTables = new Lazy<ITableStorageClient>(() => new TableStorageClient());
		private static Lazy<IStorageClient> mCloudStorage = new Lazy<IStorageClient>(() => new StorageClient());


	}
}
