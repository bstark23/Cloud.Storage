using Cloud.Storage.Blobs;
using Cloud.Storage.Queues;
using Cloud.Storage.Tables;

namespace Cloud.Storage
{
	public interface IStorageClient
	{

		IBlobStorageClient Blobs { get; }
		IQueueStorageClient Queues { get; }
		ITableStorageClient Tables { get; }
	}
}
