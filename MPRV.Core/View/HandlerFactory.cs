using System;
using System.Reflection;
using System.Web;
using MPRV.Common.Reflection;
using System.Linq;

namespace MPRV.View
{
	public class HandlerFactory : IHttpHandlerFactory
	{
		#region Public Methods

		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
		{
//			var methodGroupings = this.GetType().GetMembers<RequestMappingAttribute>();
//
//			HandlerInstantiator handlerInstantiator = null;
//			foreach (var methodGrouping in methodGroupings)
//			{
//				var methodInfo = methodGrouping.Key;
////				if (methodGrouping.Any(rma => rma.TryMapRequest<HandlerInstantiator>(context, methodInfo, out handlerInstantiator)))
////				{
////					break;
////				}
//			}
//
//			if (handlerInstantiator == null)
//			{
//				throw new NotImplementedException("HACK: No method found for this URI");
//			}
//
//			return handlerInstantiator(context);
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

