using System;
using System.Web;

namespace MPRV.View
{
	public class HandlerFactory : IHttpHandlerFactory
	{
		#region Public Methods

		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
			throw new NotImplementedException();
		}

		public void ReleaseHandler(IHttpHandler handler)
		{
			if (handler is IDisposable)
			{
				((IDisposable)handler).Dispose();
			}
		}

		#endregion
		#region Delegates
		public delegate IHttpHandler HandlerInstantiator(HttpContext context);
		#endregion
	}

}

