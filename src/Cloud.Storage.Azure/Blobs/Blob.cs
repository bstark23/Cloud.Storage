using Cloud.Storage.Blobs;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Cloud.Storage.Azure.Blobs
{
	public class Blob : IBlob
    {
		private static int BlockBatchSize = 4194304;

		internal Blob(CloudBlockBlob azureBlob)
        {
            AzureBlob = azureBlob;
        }

        public Uri Uri
        {
            get
            {
                var isSecondaryDeployment = CloudConfigurationManager.GetSetting("IsSecondaryDeployment");

                return isSecondaryDeployment == null || string.Equals(isSecondaryDeployment, "false", StringComparison.InvariantCultureIgnoreCase) ? AzureBlob.Uri : AzureBlob.StorageUri.SecondaryUri;
            }
        }

        public async Task<bool> CheckIfExists()
        {
            return await AzureBlob.ExistsAsync();
        }

        public async Task<byte[]> GetData(string leaseId = null)
        {
            byte[] results = null;
            var bytesRead = await AzureBlob.DownloadToByteArrayAsync(results, 0, GetAccessCondition(leaseId), null, null);

            return results;
        }

        public async Task UploadData(byte[] data, string leaseId = null)
        {
            await AzureBlob.UploadFromByteArrayAsync(data, 0, data.Length, GetAccessCondition(leaseId), null, null);
        }

        public async Task UploadText(string text, string leaseId = null)
        {
            await AzureBlob.UploadTextAsync(text, Encoding.UTF8, GetAccessCondition(leaseId), null, null);
        }

        public async Task UploadFromStream(Stream stream, string leaseId = null)
        {
            await AzureBlob.UploadFromStreamAsync(stream, GetAccessCondition(leaseId), null, null);
        }

        public async Task DeleteIfExists(string leaseId = null)
        {
            await AzureBlob.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, GetAccessCondition(leaseId), null, null);
        }

        public string AquireLease(TimeSpan? lockLength)
        {
            if (lockLength.HasValue)
            {
                if (lockLength < TimeSpan.FromSeconds(15D) || lockLength > TimeSpan.FromSeconds(60D))
                    throw new NotSupportedException("Azure Blob Storage allows lease times from 15 seconds to 60 seconds or infinite leases, only");
            }

            return AzureBlob.AcquireLease(lockLength, Guid.NewGuid().ToString());
        }

        public TimeSpan BreakLease(string leaseId)
        {
            return AzureBlob.BreakLease(null, GetAccessCondition(leaseId), null, null);
        }

        protected AccessCondition GetAccessCondition(string leaseId)
        {
            if (string.IsNullOrWhiteSpace(leaseId))
                return null;
            else
                return AccessCondition.GenerateLeaseCondition(leaseId);
        }

		public async Task AppendText(string text, string leaseId = null)
		{
			var binaryData = Encoding.UTF8.GetBytes(text);
			await AppendData(binaryData, leaseId);
        }

		public async Task AppendData(byte[] data, string leaseId = null)
		{
			var blockList = new List<Tuple<string, byte[]>>();
			int bytesRead = 0;

			while (bytesRead < data.Length)
			{
				var currBatch = data.Skip(bytesRead).Take(BlockBatchSize).ToArray();

				blockList.Add(new Tuple<string, byte[]>(Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())), currBatch));

				bytesRead += currBatch.Length;
			}

			foreach (var block in blockList)
			{
				using (var memStream = new MemoryStream(block.Item2))
				{
					await AzureBlob.PutBlockAsync(block.Item1, memStream, null);
				}
			}

			var blockListLeaseId = leaseId ?? AzureBlob.AcquireLease(TimeSpan.FromSeconds(15D), Guid.NewGuid().ToString());
			var accessCondition = new AccessCondition() { LeaseId = blockListLeaseId };
			
			var currentBlockList = AzureBlob.DownloadBlockList(BlockListingFilter.Committed, accessCondition).Select(block => block.Name).ToList();

			foreach (var block in blockList)
			{
				currentBlockList.Add(block.Item1);
			}
			
			AzureBlob.PutBlockList(currentBlockList, accessCondition);

			if (leaseId == null)
			{
				AzureBlob.ReleaseLease(accessCondition);
			}
		}

		public async Task<byte[]> DownloadData(string leaseId = null)
		{
			//TODO: Implement leaseId support correctly
			using (var stream = new MemoryStream())
			{
				await AzureBlob.DownloadToStreamAsync(stream);

				return stream.ToArray();
			}
		}

		public async Task<string> DownloadText(string leaseId = null)
		{
			return Encoding.UTF8.GetString((await DownloadData()));
		}

		private CloudBlockBlob AzureBlob { get; set; }
    }
}
