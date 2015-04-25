using System.Threading.Tasks;

namespace Cloud.Storage.Queues
{
	public interface IQueueStorageClient
	{
		Task<IQueue> GetQueue(string queueName, bool createIfNotExists = false);
	}
}
