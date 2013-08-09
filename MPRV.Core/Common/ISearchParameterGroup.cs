using System;
using System.Collections.Generic;

namespace MPRV.Common
{
	public interface ISearchParameterGroup : ISearchableParameter, IList<ISearchableParameter>
	{
		SearchParameterGroupComparison Comparison {get;}
		void AddRange(IEnumerable<ISearchableParameter> searchableParameters);
	}

	public enum SearchParameterGroupComparison 
	{
		All = 2,
		Any = 4
	}
}

