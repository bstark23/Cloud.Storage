namespace Cloud.Storage.Blobs
{
	public interface IBlobStorageClient
	{
		IContainer CreateContainerIfNotExists(string containerName);
    }
}
