namespace Cloud.Storage.Blob
{
	public interface IContainer
	{
		IBlob GetBlob(string uri);
	}
}
