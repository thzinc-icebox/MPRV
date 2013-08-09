using System;

namespace MPRV.Process
{
	public class DelegateHandlerResponse<TResponseData> : IHandlerResponse<TResponseData>
	{
		#region Constructors

		public DelegateHandlerResponse(ResponseProcessor processor)
		{
			_processor = processor;
		}

		#endregion

		#region Delegates

		public delegate void ResponseProcessor(TResponseData responseData);

		#endregion


		#region IHandlerResponse implementation

		public void ProcessResponse(TResponseData responseData)
		{
			_processor(responseData);
		}

		#endregion

		#region Private Fields

		private ResponseProcessor _processor;

		#endregion
	}
}

