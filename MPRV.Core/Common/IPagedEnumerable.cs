using System;
using System.Collections.Generic;

namespace MPRV.Common
{
	public interface IPagedEnumerable<TElement> : IEnumerable<TElement>
	{
		#region Properties
		long TotalItems{ get; }

		long TotalPages { get; }

		long Page { get; }

		long Count { get; }
		#endregion
	}
}

