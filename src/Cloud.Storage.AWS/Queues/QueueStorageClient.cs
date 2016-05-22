using System;
using System.Threading.Tasks;
using Cloud.Storage.Queues;

namespace Cloud.Storage.AWS.Queues
{
	public class QueueStorageClient : IQueueStorageClient
	{
		public IMessage CreateMessage(byte[] messageContents)
		{
			throw new NotImplementedException();
		}

		public IMessage CreateMessage(string messageContents)
		{
			throw new NotImplementedException();
		}

		public Task<IQueue> GetQueue(string queueName, bool createIfNotExists = false)
		{
			throw new NotImplementedException();
		}
	}
}
