﻿using Cloud.Storage.Blob;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Blob
{
	public class Blob : IBlob
    {
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


        private CloudBlockBlob AzureBlob { get; set; }
    }
}
