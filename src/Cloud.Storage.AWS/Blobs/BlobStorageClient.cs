using Cloud.Storage.Blobs;
using System;

namespace Cloud.Storage.AWS.Blobs
{
	public class BlobStorageClient : IBlobStorageClient
	{
		public IContainer CreateContainerIfNotExists(string containerName)
		{
			throw new NotImplementedException();
		}
	}
}
