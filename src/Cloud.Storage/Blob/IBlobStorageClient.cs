namespace Cloud.Storage.Blob
{
	public interface IBlobStorageClient
	{
		IContainer CreateContainerIfNotExists(string containerName);
    }
}
