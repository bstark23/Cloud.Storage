using Cloud.Storage.Blob;
using System;

namespace Cloud.Storage.AWS.Blob
{
	public class BlobStorageClient : IBlobStorageClient
	{
		public IContainer CreateContainerIfNotExists(string containerName)
		{
			throw new NotImplementedException();
		}
	}
}
