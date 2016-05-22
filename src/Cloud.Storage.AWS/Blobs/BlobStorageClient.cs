using Cloud.Storage.Blobs;
using System;
using System.Threading.Tasks;

namespace Cloud.Storage.AWS.Blobs
{
	public class BlobStorageClient : IBlobStorageClient
	{
		public Task<IContainer> GetContainer(string containerName, bool createIfNotExists)
		{
			throw new NotImplementedException();
		}
	}
}
