using Cloud.Storage.Azure.Queues;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tests.Queues
{
	[TestFixture]
	public class QueueStorageClientTests
	{
		public static string TestingQueueName = "testing";

		[Test]
		public async Task TestQueueCreation()
		{
			var queue = await GetTestingQueue();
			Assert.IsNotNull(queue);
		}

		[Test]
		public async Task TestQueueCreateMessage()
		{
			var queue = await GetTestingQueue();
			var message = queue.CreateMessage("Testing");
			Assert.IsNotNull(message);
		}

		[Test]
		public async Task TestQueueAddMessageAndClear()
		{
			var queue = await GetTestingQueue();
			var message = queue.CreateMessage("Testing");
			await queue.AddMessage(message);
			Assert.IsTrue(queue.GetApproximateMessageCount() > 0);
			await queue.Clear();
			Assert.IsTrue(queue.GetApproximateMessageCount() == 0);
		}

		[Test]
		public async Task TestAddMessagePeekMessage()
		{
			var queue = await GetTestingQueue();
			var message = queue.CreateMessage("Testing");
			await queue.AddMessage(message);
			Assert.IsTrue(queue.GetApproximateMessageCount() > 0);

			var azureMessage = await queue.PeekNextMessage() as Message;
			Assert.IsTrue(string.Equals(azureMessage.AzureMessage.AsString, "Testing", System.StringComparison.InvariantCultureIgnoreCase));

			await queue.Clear();
			Assert.IsTrue(queue.GetApproximateMessageCount() == 0);
		}

		private static async Task<Queue> GetTestingQueue()
		{
			return await StorageClient.CloudStorage.Queues.GetQueue(TestingQueueName, true) as Queue;
		}
	}
}
