using Cloud.Storage.Tables;
using Microsoft.WindowsAzure.Storage.Table;

namespace Cloud.Storage.Azure.Tables
{
	public class Row : TableEntity, IRow
	{
		public Row()
			: base()
		{
		}

		public Row(string partitionKey, string rowKey)
			: base(partitionKey, rowKey)
		{
		}

		public Row(IRow row)
			: this(row.PrimaryKey, row.SecondaryKey)
		{
			Data = row;
		}

		public string PrimaryKey { get { return PartitionKey; } }

		public string SecondaryKey { get { return RowKey; } }

		public IRow Data
		{
			get
			{
				if (mData != null)
				{
					return mData;
				}
				else
				{
					return this;
				}
			}
			internal set
			{
				mData = value;
			}
		}

		private IRow mData { get; set; }
	}
}
