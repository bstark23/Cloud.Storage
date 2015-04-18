using Cloud.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cloud.Storage.AWS.Blob
{
	public class Blob : IBlob
	{
		public Task AppendData(byte[] data, string leaseId = null)
		{
			throw new NotImplementedException();
		}

		public Task AppendText(string text, string leaseId = null)
		{
			throw new NotImplementedException();
		}

		public string AquireLease(TimeSpan? lockLength)
		{
			throw new NotImplementedException();
		}

		public TimeSpan BreakLease(string leaseId)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> CheckIfExists()
		{
			throw new NotImplementedException();
		}

		public async Task DeleteIfExists(string leaseId = null)
		{
			throw new NotImplementedException();
		}

		public async Task<byte[]> GetData(string leaseId = null)
		{
			throw new NotImplementedException();
		}

		public async Task UploadData(byte[] data, string leaseId = null)
		{
			throw new NotImplementedException();
		}

		public Task UploadFromStream(Stream stream, string leaseId = null)
		{
			throw new NotImplementedException();
		}

		public async Task UploadText(string text, string leaseId = null)
		{
			throw new NotImplementedException();
		}
	}
}
