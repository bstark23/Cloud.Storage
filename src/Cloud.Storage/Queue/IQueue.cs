using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloud.Storage.Queue
{
	public interface IQueue
	{
		Task<IMessage> PeekNextMessage();
		Task<List<IMessage>> PeekMessages(int numMessages);
		Task<IMessage> GetNextMessage(TimeSpan? visibilityTimeout = null);
		Task<List<IMessage>> GetMessages(int numMessages, TimeSpan? visibilityTimeout = null);
	}
}
