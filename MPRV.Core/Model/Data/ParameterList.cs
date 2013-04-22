using System;
using System.Collections.Generic;

namespace MPRV.Model.Data
{
	public class ParameterList : Dictionary<string, object>
	{
		#region Constructors

		public ParameterList()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion
	}
}

