using System.Threading.Tasks;

namespace Cloud.Storage.Blobs
{
	public interface IBlobStorageClient
	{
		Task<IContainer> GetContainer(string containerName, bool createIfNotExists = false);
	}
}
