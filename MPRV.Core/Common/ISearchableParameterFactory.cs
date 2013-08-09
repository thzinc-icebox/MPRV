using System;

namespace MPRV.Common
{
	public interface ISearchableParameterFactory<TTarget>
	{
		#region Methods
		ISearchableParameter GetSearchableParameter();
		#endregion
	}
}

