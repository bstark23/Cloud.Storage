using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Linq;

namespace Cloud.Storage.Azure.Blobs
{
	/// <summary>
	/// Small payload blob list item for describing a blob without retrieving it
	/// </summary>
	public class BlobListItem
    {
        internal BlobListItem(IListBlobItem blobListItem)
        {
            AzureBlobListItem = blobListItem;
        }

        public string Name { get { return PrimaryBlobUri.Segments.Last(); } }
        public Uri PrimaryBlobUri { get { return AzureBlobListItem.StorageUri.PrimaryUri; } }
        public Uri SecondaryBlobUri { get { return AzureBlobListItem.StorageUri.SecondaryUri; } }

        private IListBlobItem AzureBlobListItem { get; set; }
    }
}
