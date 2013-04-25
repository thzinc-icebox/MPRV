using System;

namespace MPRV.Common
{
	public static class ObjectExtensions
	{
		public static U GetValueOrDefault<T, U>(this T obj, Func<T, U> selector, Func<U> defaultSelector = null)
		{
			if (defaultSelector == null)
			{
				defaultSelector = () => default(U);
			}

			return (obj == null) ? defaultSelector() : selector(obj);
		}
	}
}

