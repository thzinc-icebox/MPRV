using System;

namespace MPRV.Model
{
	public interface ILazyReadableModel
	{
		ReadableModelPopulator Populator { get; set; }
	}

	public class LazyReadableModel<T> : Lazy<T>, ILazyReadableModel
		where T : IReadableModel, new()
	{
		public LazyReadableModel()
		{
			_instantiator = () => {
				T value = new T();
				value.Populate(Populator);

				return value;
			};
		}

		public LazyReadableModel(Func<T> instantiator)
		{
			_instantiator = instantiator;
		}

		public LazyReadableModel(T instance)
		{
			_value = instance;

			IsValueCreated = true;
		}

		public ReadableModelPopulator Populator { get; set; }

		public new bool IsValueCreated { get; protected set; }

		public new T Value
		{
			get
			{
				if (!IsValueCreated)
				{
					_value = _instantiator();

					IsValueCreated = true;
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
		protected Func<T> _instantiator;
	}
}

