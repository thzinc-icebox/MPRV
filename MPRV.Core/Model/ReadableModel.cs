using System;
using System.Data;

namespace MPRV.Model
{
	public abstract class ReadableModel : IReadableModel
	{
		#region Constructors

		public ReadableModel ()
		{
		}

		#endregion

		#region Public Methods

		public virtual bool Populate (ReadableModelPopulator populator) {
			return IsPopulated = populator(this);
		}

		#endregion

		#region Public Properties

		public bool IsPopulated { get; protected set; }

		#endregion
	}
}

