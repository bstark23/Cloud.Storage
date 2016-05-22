using Cloud.Storage.Queues;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Queues
{
	public class QueueStorageClient : IQueueStorageClient
	{
		public async Task<IQueue> GetQueue(string queueName, bool createIfNotExists = false)
		{
			var azureQueue = await GetAzureQueue(queueName, createIfNotExists);
			return new Queue(azureQueue);
		}

		public IMessage CreateMessage(string messageContents)
		{
			var message = new CloudQueueMessage(messageContents);
			return new Message(message);
		}

		public IMessage CreateMessage(byte[] messageContents)
		{
			var message = new CloudQueueMessage(messageContents);
			return new Message(message);
		}

		public async Task<CloudQueue> GetAzureQueue(string queueName, bool createIfNotExists = false)
		{
			var queue = QueueClient.GetQueueReference(queueName);

			if (createIfNotExists)
			{
				await queue.CreateIfNotExistsAsync();
			}

			return queue;
		}

		public static CloudQueueClient QueueClient { get { return mQueueClient.Value; } }
		private static Lazy<CloudQueueClient> mQueueClient = new Lazy<CloudQueueClient>(() => StorageClient.StorageAccount.CreateCloudQueueClient());
	}
}
