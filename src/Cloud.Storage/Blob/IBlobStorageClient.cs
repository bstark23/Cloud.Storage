using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Blob
{
	public interface IBlobStorageClient
	{
		IContainer CreateContainerIfNotExists(string containerName);
    }
}
