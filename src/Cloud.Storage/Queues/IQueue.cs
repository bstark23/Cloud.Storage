using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloud.Storage.Queues
{
	public interface IQueue
	{
		IMessage CreateMessage(string messageContents);
		IMessage CreateMessage(byte[] messageContents);
		Task AddMessage(IMessage message);
		int GetApproximateMessageCount();
		Task Clear();
		Task<IMessage> PeekNextMessage();
		Task<List<IMessage>> PeekMessages(int numMessages);
		Task<IMessage> GetNextMessage(TimeSpan? visibilityTimeout = null);
		Task<List<IMessage>> GetMessages(int numMessages, TimeSpan? visibilityTimeout = null);
    }
}
