using Cloud.Storage.Blob;
using Cloud.Storage.Queue;
using Cloud.Storage.Table;
using System;

namespace Cloud.Storage.AWS
{
	public class StorageClient : IStorageClient
	{
		public IBlobStorageClient Blobs
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public IQueueStorageClient Queues
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public ITableStorageClient Tables
		{
			get
			{
				throw new NotImplementedException();
			}
		}
	}
}
