using System;
using System.IO;
using System.Threading.Tasks;

namespace Cloud.Storage.Blob
{
	public interface IBlob
	{
		Task<bool> CheckIfExists();
		Task<byte[]> GetData(string leaseId = null);
		Task UploadData(byte[] data, string leaseId = null);
		Task UploadText(string text, string leaseId = null);
		Task UploadFromStream(Stream stream, string leaseId = null);
		Task DeleteIfExists(string leaseId = null);
		string AquireLease(TimeSpan? lockLength);
		TimeSpan BreakLease(string leaseId);
		Task AppendText(string text, string leaseId = null);
		Task AppendData(byte[] data, string leaseId = null);


	}
}
