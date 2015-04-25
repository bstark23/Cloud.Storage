using Cloud.Storage.Queues;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Queues
{
	public class Message : IMessage
	{
		public Message(CloudQueueMessage azureMessage)
		{
			AzureMessage = azureMessage;
		}

		public CloudQueueMessage AzureMessage { get; private set; }
	}
}
