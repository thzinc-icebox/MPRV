using System;

namespace MPRV.Model
{
	public interface IChangeableModel<TChangeableModel> : IReadableModel, IEquatable<TChangeableModel>
		where TChangeableModel : IReadableModel, new()
	{
		bool IsChanged {get;}
	}
}

