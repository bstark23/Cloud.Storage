using Cloud.Storage.Blob;
using Cloud.Storage.Queue;
using Cloud.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage
{
	public interface IStorageClient
	{

		IBlobStorageClient Blobs { get; }
		IQueueStorageClient Queues { get; }
		ITableStorageClient Tables { get; }
	}
}
