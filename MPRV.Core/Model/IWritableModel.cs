using System;

namespace MPRV.Model
{
	public interface IWritableModel<TWritableModel> : IChangeableModel<TWritableModel>
		where TWritableModel : IReadableModel, new()
	{
		bool Commit();
	}
}

