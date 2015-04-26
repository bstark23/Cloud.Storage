using Cloud.Storage.Azure.Blobs;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tests.Blobs
{
	[TestFixture]
	public class BlobStorageClientTests
	{
		public static string TestingContainerName = "testing";

		[Test]
		public async Task TestContainerCreation()
		{
			var container = await GetTestingContainer();
			Assert.IsNotNull(container);
		}

		[Test]
		public async Task TestGetBlob()
		{
			var container = await GetTestingContainer();
			var blob = container.GetBlob("Testing.txt");
			await blob.UploadText("Testing");
			var textContents = await blob.DownloadText();
			Assert.IsTrue(string.Equals(textContents, "Testing", System.StringComparison.InvariantCultureIgnoreCase));

		}
		public static async Task<Container> GetTestingContainer()
		{
			return await StorageClient.CloudStorage.Blobs.GetContainer(TestingContainerName, true) as Container;
		}
	}
}
