using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud.Storage.Table
{
	public class RowReference
		: IRow
	{
		public RowReference(string primaryKey, string secondaryKey)
		{
			PrimaryKey = primaryKey;
			SecondaryKey = secondaryKey;
		}

		public string PrimaryKey { get; private set; }
		public string SecondaryKey { get; private set; }
	}
}
