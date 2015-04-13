using Cloud.Storage.Blob;
using Cloud.Storage.Queue;
using Cloud.Storage.Table;

namespace Cloud.Storage
{
	public interface IStorageClient
	{

		IBlobStorageClient Blobs { get; }
		IQueueStorageClient Queues { get; }
		ITableStorageClient Tables { get; }
	}
}
