using System;
using System.Web;

namespace MPRV.View
{
	public class RedirectHandler : IHttpHandler
	{
		#region Constructors
		public RedirectHandler(string url)
		{
			_url = url;
		}
		#endregion
		#region IHttpHandler implementation
		public void ProcessRequest(HttpContext context)
		{
			context.Response.Redirect(_url);
		}

		public bool IsReusable
		{
			get{ return false;}
		}
		#endregion

		#region Private Fields

		private string _url;

		#endregion
	}
}

