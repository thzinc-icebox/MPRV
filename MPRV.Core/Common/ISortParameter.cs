using System;

namespace MPRV.Common
{
	public interface ISortParameter
	{
		#region Properties
		string Name{ get; }

		SortDirection Direction { get; }
		#endregion
	}

	public enum SortDirection
	{
		Ascending,
		Descending
	}
}

