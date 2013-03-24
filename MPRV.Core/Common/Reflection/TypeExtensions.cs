using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace MPRV.Common.Reflection
{
	/// <summary>
	/// Reflection-specific extension methods for <see cref="Type"/>
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// <see cref="BindingFlags"/> bitmask to identify public and non-public properties and fields
		/// </summary>
		internal const BindingFlags PROPERTY_AND_FIELD_BINDING_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		
		/// <summary>
		/// Gets a lookup of members for this type that are decorated with <typeparamref name="T"/>
		/// </summary>
		/// <returns>
		/// Lookup of members for this type that are decorated with <typeparamref name="T"/>
		/// </returns>
		/// <param name='type'>
		/// Type being extended
		/// </param>
		/// <param name='bindingFlags'>
		/// A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.
		/// </param>
		/// <param name='inherit'>
		/// <c>true</c> to search this member's inheritance chain to find the attributes; otherwise, <c>false</c>. This parameter is ignored for properties and events.
		/// </param>
		/// <typeparam name='T'>
		/// Type of custom attribute to use in filtering the members that will be iterated over.
		/// </typeparam>
		public static ILookup<MemberInfo, T> GetMembers<T>(this Type type, BindingFlags bindingFlags = PROPERTY_AND_FIELD_BINDING_FLAGS, bool inherit = false)
			where T : Attribute
		{
			ILookup<MemberInfo, T> members;
			
			// TODO: Evaluate static caching scheme
			
			members = type.GetMembers(bindingFlags).SelectMany(m => m.GetCustomAttributes(typeof(T), inherit).Cast<T>(), (m, a) => Tuple.Create(m, a)).ToLookup(t => t.Item1, t => t.Item2);			
			
			return members;
		}
	}
}

