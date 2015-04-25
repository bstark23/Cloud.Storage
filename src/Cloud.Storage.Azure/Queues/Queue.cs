using Cloud.Storage.Queues;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Cloud.Storage.Azure.Tables;

namespace Cloud.Storage.Azure.Queues
{
	public class Queue : IQueue
	{
		public Queue(CloudQueue azureQueue)
		{
			AzureQueue = azureQueue;
		}

		public int GetApproximateMessageCount()
		{
			return AzureQueue.ApproximateMessageCount ?? AzureQueue.PeekMessages(32).Count();
		}

		public async Task Clear()
		{
			await AzureQueue.ClearAsync();
		}

		public async Task<List<IMessage>> GetMessages(int numMessages, TimeSpan? visibilityTimeout = default(TimeSpan?))
		{
			var messages = await AzureQueue.GetMessagesAsync(numMessages, visibilityTimeout, null, null);
			return messages.Select(message => new Message(message) as IMessage).ToList();
		}

		public async Task<IMessage> GetNextMessage(TimeSpan? visibilityTimeout = default(TimeSpan?))
		{
			var message = await AzureQueue.GetMessageAsync(visibilityTimeout, null, null);
			return new Message(message);
		}

		public async Task<List<IMessage>> PeekMessages(int numMessages)
		{
			var messages = await AzureQueue.PeekMessagesAsync(numMessages);
			return messages.Select(message => new Message(message) as IMessage).ToList();
		}

		public async Task<IMessage> PeekNextMessage()
		{
			var message = await AzureQueue.PeekMessageAsync();
			return new Message(message);
		}

		public async Task AddMessage(IMessage message)
		{
			var azureMessage = message as Message;
			await AzureQueue.AddMessageAsync(azureMessage.AzureMessage);
		}
		public IMessage CreateMessage(string messageContents)
		{
			return StorageClient.CloudStorage.Queues.CreateMessage(messageContents);
		}

		public IMessage CreateMessage(byte[] messageContents)
		{
			return StorageClient.CloudStorage.Queues.CreateMessage(messageContents);
		}

		public CloudQueue AzureQueue { get; private set; }
	}
}
