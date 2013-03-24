using System;

namespace MPRV.Model
{
	public abstract class WritableModel: ReadableModel, IWritableModel
	{
		#region Constructors

		public WritableModel ()
		{
		}

		#endregion

		#region Public Methods

		public abstract bool Commit ();

		#endregion

		#region Public Properties

		public bool IsChanged {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion


	}
}

