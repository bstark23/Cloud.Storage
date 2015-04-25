using Cloud.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Queue
{
	public class Queue : IQueue
	{
		public async Task<List<IMessage>> GetMessages(int numMessages, TimeSpan? visibilityTimeout = default(TimeSpan?))
		{
			throw new NotImplementedException();
		}

		public async Task<IMessage> GetNextMessage(TimeSpan? visibilityTimeout = default(TimeSpan?))
		{
			throw new NotImplementedException();
		}

		public async Task<List<IMessage>> PeekMessages(int numMessages)
		{
			throw new NotImplementedException();
		}

		public async Task<IMessage> PeekNextMessage()
		{
			throw new NotImplementedException();
		}
	}
}
