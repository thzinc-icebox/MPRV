using System;

namespace MPRV.Model
{
	public interface IChangeableModel : IReadableModel
	{
		bool IsChanged {get;}
	}
}

