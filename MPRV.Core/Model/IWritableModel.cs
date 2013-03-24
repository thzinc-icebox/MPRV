using System;

namespace MPRV.Model
{
	public interface IWritableModel : IChangeableModel
	{
		bool Commit();
	}
}

