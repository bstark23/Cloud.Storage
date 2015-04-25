using System.Threading.Tasks;

namespace Cloud.Storage.Queue
{
	public interface IQueueStorageClient
	{
		Task<IQueue> GetQueue(string queueName, bool createIfNotExists = false);
	}
}
