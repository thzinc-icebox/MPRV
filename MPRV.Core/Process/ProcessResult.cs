using System;
using System.Collections.Generic;

namespace MPRV.Process
{
	public class ProcessResult<TResult>
	{
		#region Constructors
		public ProcessResult()
			: this(default(TResult))
		{
		}

		public ProcessResult(TResult result)
		{
			Messages = new List<string>();
			Result = result;
		}
		#endregion
		#region Public Properties
		public ICollection<string> Messages { get; protected set; }

		public TResult Result { get; set; }
		#endregion
	}
}

