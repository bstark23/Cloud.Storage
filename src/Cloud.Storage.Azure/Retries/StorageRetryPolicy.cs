using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Retries
{
    public class StorageRetryPolicy : IRetryPolicy
    {
        public StorageRetryPolicy(RetryStrategy retryStrategy)
        {
            RetryStrategy = retryStrategy;
        }

        public IRetryPolicy CreateInstance()
        {
            return new StorageRetryPolicy(RetryStrategy);
        }

        public bool ShouldRetry(int currentRetryCount, int statusCode, Exception lastException, out TimeSpan retryInterval, Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            return RetryStrategy.GetShouldRetry()(currentRetryCount, lastException, out retryInterval);
        }

        private RetryStrategy RetryStrategy { get; set; }
    }
}
