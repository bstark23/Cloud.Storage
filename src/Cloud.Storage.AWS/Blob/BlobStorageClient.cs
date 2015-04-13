using Cloud.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
