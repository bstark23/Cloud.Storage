﻿using Cloud.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Cloud.Storage.Azure.Queue
{
	public class Queue : IQueue
	{
		public Queue(CloudQueue azureQueue)
		{
			azureQueue = AzureQueue;
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

		public CloudQueue AzureQueue { get; private set; }
	}
}
