namespace Cloud.Storage.Blobs
{
	public interface IContainer
	{
		IBlob GetBlob(string uri);
	}
}
