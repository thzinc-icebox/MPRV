using System;

namespace MPRV.Model
{
	public interface ILazyReadableModel {
		ReadableModelPopulator Populator { get; set; }
	}

	public class LazyReadableModel<T> : Lazy<T>, ILazyReadableModel
		where T : IReadableModel, new()
	{
		public LazyReadableModel()
		{
		}

		public ReadableModelPopulator Populator { get; set; }

		public new bool IsValueCreated { get; protected set; }

		public new T Value
		{
			get
			{
				if (!IsValueCreated)
				{
					_value = new T();
					_value.Populate(Populator);
				}

				return _value;
			}
		}

		public override string ToString()
		{
			string result;
			if (IsValueCreated)
			{
				result = _value.ToString();
			}
			else
			{
				result = "Value is not created";
			}

			return result;
		}

		protected T _value;
	}
}

