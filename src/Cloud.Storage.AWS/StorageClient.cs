using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloud.Storage.Blob;
using Cloud.Storage.Queue;
using Cloud.Storage.Table;

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
