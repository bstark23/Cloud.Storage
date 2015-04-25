using System;
using System.Threading.Tasks;
using Cloud.Storage.Queues;

namespace Cloud.Storage.AWS.Queues
{
	public class QueueStorageClient : IQueueStorageClient
	{
		public Task<IQueue> GetQueue(string queueName, bool createIfNotExists = false)
		{
			throw new NotImplementedException();
		}
	}
}
