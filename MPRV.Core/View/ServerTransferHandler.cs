using System;
using System.Web;

namespace MPRV.View
{
	public class ServerTransferHandler : IHttpHandler
	{
		#region Constructors

		public ServerTransferHandler(string virtualPath)
		{
			_virtualPath = virtualPath;
		}

		#endregion

		#region IHttpHandler implementation
		public void ProcessRequest(HttpContext context)
		{
			context.Server.Transfer(_virtualPath);
		}

		public bool IsReusable
		{
			get{ return false;}
		}
		#endregion

		#region Private Fields

		private string _virtualPath;

		#endregion
	}
}

