using System;
using System.Web;
using System.Collections.Generic;

namespace MPRV.Process
{
	public interface IHandlerResponse<TResponseData>
	{
		void ProcessResponse(TResponseData responseData);
	}
}

