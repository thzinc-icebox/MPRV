using System;
using System.Web;
using System.Collections.Generic;

namespace MPRV.Process
{
	public static class HttpContextExtensions
	{
		public static T GetItem<T>(this HttpContext context, string key)
		{
			return (T)context.Items[key];
		}

		public const string URL_PARAMETERS = "MPRV.Process.URL_PARAMETERS";

		public static string GetUrlParameter(this HttpContext context, string name)
		{
			var urlParameters = context.GetItem<IDictionary<string, string>>(URL_PARAMETERS);

			string parameter;
			return urlParameters.TryGetValue(name, out parameter) ? context.Server.UrlDecode(parameter) : string.Empty;
		}
	}
}

