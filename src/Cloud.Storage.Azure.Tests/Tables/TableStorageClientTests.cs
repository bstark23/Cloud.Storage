using Cloud.Storage.Azure.Tables;
using Cloud.Storage.Azure.Tables.Partitioning;
using Microsoft.WindowsAzure.Storage.Table;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloud.Storage.Azure.Tests.Tables
{
	[TestFixture]
	public class TableStorageClientTests
	{
		public static string TestTableName = "Testing";
		public static int TestRowCount = 100;

		[Test]
		public void TestTableCreation()
		{
			var table = GetTestTable();
			Assert.IsTrue(table.Exists());
		}

		[Test]
		public async Task TestTableInsertion()
		{
			var rows = GenerateTestRows();
			await TestTable.InsertOrUpdateRows(rows, true);
		}

		[Test]
		public async Task TestPartitionTableCreation()
		{
			await PartitionTable.UpdatePartitionTable(TestTableName);
		}

		[Test]
		public async Task AddMoreTestRows()
		{
			await AddMoreRowsToExistingPartitions();
		}

		[Test]
		public async Task TestReadNewRowsSinceLastQuery()
		{
			foreach (var partition in await GetPartitionsForTestTable())
			{
				await AddMoreRowsToPartition(partition);
				var latestRows = await TestTable.GetNewRowsForPartition<TableEntity>(partition.RowKey);
				Assert.IsTrue(latestRows.Count > 0);
				latestRows = await TestTable.GetNewRowsForPartition<TableEntity>(partition.RowKey);
				Assert.IsTrue(latestRows.Count == 0);
				await AddMoreRowsToPartition(partition);
				latestRows = await TestTable.GetNewRowsForPartition<TableEntity>(partition.RowKey);
				Assert.IsTrue(latestRows.Count > 0);
			}
		}

		public static Table GetTestTable()
		{
			var table = TableStorageClient.GetTable(TestTableName, true).Result;
			return table as Table;
		}
		public static System.Collections.Generic.List<TableEntity> GenerateTestRows()
		{
			var rows = new List<TableEntity>();
			for (Int64 i = 0; i < TestRowCount; ++i)
			{
				rows.Add(
					new TableEntity()
					{
						PartitionKey = Guid.NewGuid().ToString(),
						RowKey = i.ToStringComparableValue(),
						ETag = "*",
						Timestamp = DateTime.UtcNow
					});
			}

			return rows;
		}

		public static async Task AddMoreRowsToExistingPartitions()
		{
			var partitionList = await TableStorageClient.PartitionTable.GetPartitionList(TestTableName);
			var rows = new List<TableEntity>();

			foreach (var partition in partitionList)
			{
				await AddMoreRowsToPartition(partition);
			}
		}

		public static async Task AddMoreRowsToPartition(PartitionTableRow partition)
		{
			var rows = new List<TableEntity>();
			var partitionRows = await TestTable.
					GetRowsByPartitionKey<TableEntity>( partition.RowKey);

			var maxRowValue = (Int64)(partitionRows.Any() ? partitionRows.Max(row => Int64.Parse(row.RowKey)) : 0);
			for (var i = 0; i < TestRowCount; ++i)
			{
				++maxRowValue;
				rows.Add(new TableEntity() { PartitionKey = partition.RowKey, RowKey = maxRowValue.ToStringComparableValue(), ETag = "*", Timestamp = DateTime.UtcNow });
			}

			await TestTable.InsertOrUpdateRows(rows);
		}

		public static async Task<List<PartitionTableRow>> GetPartitionsForTestTable()
		{
			var partitionList = await PartitionTable.GetPartitionList(TestTableName);
			return partitionList;
		}

		private static TableStorageClient TableStorageClient { get { return StorageClientTests.StorageClient.Tables as TableStorageClient; } }
		private static PartitionTable PartitionTable { get { return TableStorageClient.PartitionTable; } }
		private static Table TestTable { get { return TableStorageClient.GetTable(TestTableName).Result as Table; } }
	}
}
