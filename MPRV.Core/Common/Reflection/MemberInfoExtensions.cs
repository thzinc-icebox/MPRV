using System;
using System.Reflection;


namespace MPRV.Common.Reflection
{
	public static class MemberInfoExtensions
	{
		public static Type GetMemberType(this MemberInfo memberInfo)
		{
			Type type;
			
			if (memberInfo is PropertyInfo)
			{
				type = (memberInfo as PropertyInfo).PropertyType;
			}
			else if (memberInfo is FieldInfo)
			{
				type = (memberInfo as FieldInfo).FieldType;
			}
			else {
				// TODO: Localize
				throw new ArgumentOutOfRangeException("memberInfo", "Member must be a field or property");
			}
			
			return type;
		}
	}
}

