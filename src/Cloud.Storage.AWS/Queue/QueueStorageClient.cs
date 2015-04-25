using System;
using System.Threading.Tasks;
using Cloud.Storage.Queue;

namespace Cloud.Storage.AWS.Queue
{
	public class QueueStorageClient : IQueueStorageClient
	{
		public Task<IQueue> GetQueue(string queueName, bool createIfNotExists = false)
		{
			throw new NotImplementedException();
		}
	}
}
