using System;
using System.Web;

namespace MPRV.Process
{
	/// <summary>
	/// Represents a handler that produces a result that may be used when processing an <see cref="IHandlerResponse"/>
	/// </summary>
	public abstract class ProducerHandler<TResult> : IHttpHandler
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="Intake.Core.Process.Handler.CreateDatum"/> class
		/// </summary>
		/// <param name="response">Response to be called when the handler completes</param>
		protected ProducerHandler(IHandlerResponse<TResult> response)
		{
			Response = response;
		}
		#endregion
		#region Public Properties
		/// <summary>
		/// Response to be called when the handler completes
		/// </summary>
		/// <value>The response.</value>
		public IHandlerResponse<TResult> Response { get; protected set; }

		/// <summary>
		/// Determines whether this instance may be used for handling multiple requests
		/// </summary>
		public bool IsReusable
		{
			get{ return false;}
		}
		#endregion
		#region Public Methods
		/// <summary>
		/// Processes the request
		/// </summary>
		/// <param name="context">Context</param>
		public void ProcessRequest(HttpContext context)
		{
			var result = GetResult(context);
			Response.ProcessResponse(result);
		}
		#endregion
		#region Protected Methods
		/// <summary>
		/// Produces the result of this handler
		/// </summary>
		/// <returns>The result</returns>
		/// <param name="context">Context</param>
		protected abstract TResult GetResult(HttpContext context);
		#endregion
	}
}

