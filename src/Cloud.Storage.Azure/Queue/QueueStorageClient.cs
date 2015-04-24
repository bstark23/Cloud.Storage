using Cloud.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace Cloud.Storage.Azure.Queue
{
	public class QueueStorageClient : IQueueStorageClient
	{

		public static CloudQueueClient QueueClient { get { return mQueueClient.Value; } }
		private static Lazy<CloudQueueClient> mQueueClient = new Lazy<CloudQueueClient>(() => StorageClient.StorageAccount.CreateCloudQueueClient());

	}
}
