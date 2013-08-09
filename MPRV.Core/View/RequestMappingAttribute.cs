using System;

namespace MPRV.View
{
	[AttributeUsage(AttributeTargets.Method)]
	public class RequestMappingAttribute : Attribute
	{
		public RequestMappingAttribute(string[] methods, string restfulPattern)
		{
		}
	}
}

