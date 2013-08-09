using System;
using System.Web;

namespace MPRV.Process
{
	public static class HttpContextExtensions
	{
		public static T GetItem<T>(this HttpContext context, string key)
		{
			return (T)context.Items[key];
		}
	}
}

