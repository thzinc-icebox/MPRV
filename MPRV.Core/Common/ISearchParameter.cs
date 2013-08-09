using System;

namespace MPRV.Common
{
	public interface ISearchParameter : ISearchableParameter
	{
		string Name { get; }

		string Comparison { get; }

		object Value { get; }
	}

	public enum SearchParameterComparison
	{
		// 1 is reserved for logical negations
		Equals = 2,
		NotEquals = Equals | 1,
	}
}

