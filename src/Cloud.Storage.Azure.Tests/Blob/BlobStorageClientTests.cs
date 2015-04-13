using NUnit.Framework;
using System.IO;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tests.Blob
{
	[TestFixture]
    public class BlobStorageClientTests
    {
        [Test]
        public void EnsureBuildContainerExists()
        {
            var container = GetClient().Blobs.CreateContainerIfNotExists("builds");

            Assert.IsNotNull(container);
        }

        [Test]
        public async Task EnsureUploadBlobTextAndDeleteBlobWorks()
        {
            var container = GetClient().Blobs.CreateContainerIfNotExists("builds");

            var blob = container.GetBlob("test.txt");

            await blob.UploadText("Testing");

            var exists = await blob.CheckIfExists();

            Assert.IsTrue(exists);

            await blob.DeleteIfExists();

            exists = await blob.CheckIfExists();

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task EnsureUploadBlobBinaryWorks()
        {
            var container = GetClient().Blobs.CreateContainerIfNotExists("builds");

            var blob = container.GetBlob("test.bin");

            await blob.UploadData(new byte[] { 5, 4, 3, 2, 1 });

            var exists = await blob.CheckIfExists();

            Assert.IsTrue(exists);

            await blob.DeleteIfExists();

            exists = await blob.CheckIfExists();

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task EnsureUploadBlobFromStreamWorks()
        {
            var container = GetClient().Blobs.CreateContainerIfNotExists("builds");

            var blob = container.GetBlob("test.bin");

            using (var stream = new MemoryStream(new byte[] { 5, 4, 3, 2, 1 }))
            {
                await blob.UploadFromStream(stream);
            }

            var exists = await blob.CheckIfExists();

            Assert.IsTrue(exists);

            await blob.DeleteIfExists();

            exists = await blob.CheckIfExists();

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task TestBlobLeasing()
        {
            var container = GetClient().Blobs.CreateContainerIfNotExists("builds");

            var blob = container.GetBlob("blobLeaseTest.txt");

            await blob.UploadText("Testing");

            var exists = await blob.CheckIfExists();

            Assert.IsTrue(exists);

            var leaseId = blob.AquireLease(null);

            await blob.UploadText("More Testing", leaseId);

            blob.BreakLease(leaseId);

            await blob.DeleteIfExists();

            exists = await blob.CheckIfExists();

            Assert.IsFalse(exists);
        }

        protected StorageClient GetClient()
        {
            return StorageClient.GetDefaultClient();
        }
    }
}
