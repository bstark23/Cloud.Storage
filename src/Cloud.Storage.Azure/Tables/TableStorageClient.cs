﻿using Cloud.Storage.Azure.Tables.Partitioning;
using Cloud.Storage.Tables;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tables
{
	public class TableStorageClient : ITableStorageClient
	{
		private static string PartitionTableName = "Partitions";

		static TableStorageClient()
		{
			var usePartitionTableSetting = string.Empty;
			try
			{
				usePartitionTableSetting = CloudConfigurationManager.GetSetting("UsePartitionTable");
			}
			catch
			{
			}
			if (!string.IsNullOrWhiteSpace(usePartitionTableSetting))
			{
				UsePartitionTable = bool.Parse(usePartitionTableSetting);
			}
			else
			{
				UsePartitionTable = true;
			}
        }

		public async Task<ITable> GetTable(string tableName, bool createIfNotExists = false)
		{
			var azureTable = await GetAzureTable(tableName, createIfNotExists);
			return new Table(azureTable);
		}

		public static async Task<CloudTable> GetAzureTable(string tableName, bool createIfNotExists = false)
		{
			var table = TableClient.GetTableReference(tableName);

			if (createIfNotExists)
			{
				await table.CreateIfNotExistsAsync();
			}

			return table;
		}

		public static bool UsePartitionTable { get; private set; }

		public static CloudTableClient TableClient { get { return mTableClient.Value; } }
		public static PartitionTable PartitionTable { get { return mPartitionTable.Value; } }

		private static Lazy<CloudTableClient> mTableClient = new Lazy<CloudTableClient>(() => StorageClient.StorageAccount.CreateCloudTableClient());
		private static Lazy<PartitionTable> mPartitionTable = new Lazy<PartitionTable>(() => new PartitionTable(GetAzureTable(PartitionTableName, true).Result));

	}
}
