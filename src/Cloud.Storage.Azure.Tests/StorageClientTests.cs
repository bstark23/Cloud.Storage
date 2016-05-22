using Cloud.Storage.Azure;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tests
{
	[TestFixture]
	public class StorageClientTests
	{
		[Test]
		public void EnsureBlobStorageInitializes()
		{
			var blobs = StorageClient.Blobs;
			Assert.IsNotNull(blobs);
		}

		[Test]
		public void EnsureTableStorageInitializes()
		{
			var tables = StorageClient.Tables;
			Assert.IsNotNull(tables);
		}

		[Test]
		public void EnsureQueueStorageInitializes()
		{
			var queues = StorageClient.Queues;
			Assert.IsNotNull(queues);
		}

		public static StorageClient StorageClient { get { return new StorageClient(); } }		
	}
}
