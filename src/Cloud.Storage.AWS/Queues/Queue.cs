using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cloud.Storage.Queues;

namespace Cloud.Storage.AWS.Queues
{
	public class Queue : IQueue
	{
		public Task AddMessage(IMessage message)
		{
			throw new NotImplementedException();
		}

		public Task Clear()
		{
			throw new NotImplementedException();
		}

		public IMessage CreateMessage(byte[] messageContents)
		{
			throw new NotImplementedException();
		}

		public IMessage CreateMessage(string messageContents)
		{
			throw new NotImplementedException();
		}

		public int GetApproximateMessageCount()
		{
			throw new NotImplementedException();
		}

		public Task<List<IMessage>> GetMessages(int numMessages, TimeSpan? visibilityTimeout = default(TimeSpan?))
		{
			throw new NotImplementedException();
		}

		public Task<IMessage> GetNextMessage(TimeSpan? visibilityTimeout = default(TimeSpan?))
		{
			throw new NotImplementedException();
		}

		public Task<List<IMessage>> PeekMessages(int numMessages)
		{
			throw new NotImplementedException();
		}

		public Task<IMessage> PeekNextMessage()
		{
			throw new NotImplementedException();
		}
	}
}
