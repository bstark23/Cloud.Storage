using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cloud.Storage.Queue;

namespace Cloud.Storage.AWS.Queue
{
	public class Queue : IQueue
	{
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
