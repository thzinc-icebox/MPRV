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

		public bool Populate (ReadableModelPopulator populator)
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region Public Properties

		public bool IsPopulated {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion
	}
}

