using System;
using System.Web;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MPRV.View
{
	[AttributeUsage(AttributeTargets.Method)]
	public class RequestMappingAttribute : Attribute
	{
		public RequestMappingAttribute(string[] methods, string restfulPattern)
		{
			Methods = methods;
			RestfulPattern = restfulPattern;
			_regexPattern = BuildRegexFromRestfulPattern(restfulPattern);
		}
		#region Public Properties
		public IEnumerable<string> Methods{ get; protected set; }

		public string RestfulPattern { get; protected set; }
		#endregion
		#region Public Methods
		public bool TryMapRequest<TDelegate>(HttpContext context, MethodInfo methodInfo, out Delegate handlerInstantiator)
		{
			bool result;

			var absolutePath = context.Request.Url.AbsolutePath;
			var match = _regexPattern.Match(absolutePath);
			handlerInstantiator = null;
			if (match.Success)
			{
//				handlerInstantiator = Delegate.CreateDelegate(typeof(TDelegate), methodInfo, false);
//
//				if (handlerInstantiator == null)
//				{
//					result = false;
//				}
//				else
//				{
					var urlParameters = _regexPattern.GetGroupNames().ToDictionary(key => key, key => match.Groups[key].Value);

					context.Items.Add(Process.HttpContextExtensions.URL_PARAMETERS, urlParameters);

					result = true;
//				}
			}
			else
			{
				handlerInstantiator = null;
				result = false;
			}

			return result;
		}
		#endregion
		#region Private Properties
		private Regex _regexPattern { get; set; }
		#endregion
		#region Private Fields
		private static Dictionary<string, string> _restfulTypePatterns = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
			{"string", @"[^/]+"},
			{"number", @"\d+"}
		};
		private static Regex _restfulPropertyPattern = new Regex(@":(\((?<type>[^\)]+)\))?(?<property>\w+)", RegexOptions.Compiled);
		#endregion
		#region Private Methods
		private static Regex BuildRegexFromRestfulPattern(string restfulPattern)
		{
			// restful pattern:	/path/:thing
			// regex pattern:	^/path/(?<thing>[^/]+)/?$
			// restful pattern:	/path/:(number)thingId
			// regex pattern:	^/path/(?<thingId>\d+)/?$

			string regexPattern = _restfulPropertyPattern.Replace(restfulPattern, match => {
				var property = match.Groups["property"].Value;
				var type = "string";

				var typeGroup = match.Groups["type"];
				if (typeGroup != null)
				{
					type = typeGroup.Value;
				}

				string typePattern;
				if (!_restfulTypePatterns.TryGetValue(type, out typePattern))
				{ 
					typePattern + _restfulTypePatterns["string"];
				}

				return string.Concat("(?<", property, ">", typePattern, ")");
			})
				.TrimStart(new char[] { '/' })
				.TrimEnd(new char[] { '/' });


			return new Regex(string.Concat("^/", regexPattern, "/?$"), RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
		}
		#endregion
	}
}

