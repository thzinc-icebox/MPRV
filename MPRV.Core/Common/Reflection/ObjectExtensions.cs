using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace MPRV.Common.Reflection
{
	/// <summary>
	/// Reflection-specific extension methods for <see cref="Object"/>
	/// </summary>
	public static class ObjectExtensions
	{
		public static object GetMember(this object obj, MemberInfo member)
		{
			object value = null;

			PropertyInfo property;
			FieldInfo field;

			if ((property = member as PropertyInfo) != null)
			{
				value = property.GetValue(obj, null);
			}
			else if ((field = member as FieldInfo) != null)
			{
				value = field.GetValue(obj);
			}

			return value;
		}

		/// <summary>
		/// Sets members of this object decorated with <typeparamref name="T"/> based on the output of <paramref name="instantiator"/>
		/// </summary>
		/// <param name='obj'>
		/// Object being extended
		/// </param>
		/// <param name='instantiator'>
		/// Function to instantiate the value of a member of <paramref name="obj"/>
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
		public static void SetMembers<T>(this object obj, Func<IGrouping<MemberInfo, T>, object> instantiator, BindingFlags bindingFlags = TypeExtensions.PROPERTY_AND_FIELD_BINDING_FLAGS, bool inherit = true)
			where T : Attribute
		{
			if (obj != null)
			{
				foreach (IGrouping<MemberInfo, T> grouping in obj.GetType().GetMembers<T>(bindingFlags, inherit))
				{
					obj.SetMember(grouping.Key, instantiator(grouping));
				}
			}
		}
		
		/// <summary>
		/// Sets a specific member of the object with a given value
		/// </summary>
		/// <param name='obj'>
		/// Object being extended
		/// </param>
		/// <param name='member'>
		/// Member of the object that is being set
		/// </param>
		/// <param name='value'>
		/// Value to set the member to. If necessary, this will be converted from the value's type to the member's type. If <c>null</c>, no attempt to set the member will be made.
		/// </param>
		public static void SetMember(this object obj, MemberInfo member, object value)
		{
			// If the value is null to begin with, do not attempt to set a member to null
			if (obj != null && value != null)
			{
				PropertyInfo property;
				FieldInfo field;

				// If the member is a property, do some property-specific handling
				if ((property = member as PropertyInfo) != null)
				{
					if (value.TryChangeType(property.PropertyType, out value))
					{
						property.SetValue(obj, value, null);
					}
					else
					{
						// Properties can have DefaultValue decorations, which this is able to deal with; better than merely setting to null
						PropertyDescriptor pd = TypeDescriptor.GetProperties(obj)[property.Name];
						if (pd.CanResetValue(obj))
						{
							pd.ResetValue(obj);
						}
					}
				}
				else if ((field = member as FieldInfo) != null)
				{
					// If the member is a field, do some field-specific handling
					value.TryChangeType(field.FieldType, out value);

					field.SetValue(obj, value);
				}
			}
		}
		
		/// <summary>
		/// Attempts to change the type of a value
		/// </summary>
		/// <returns>
		/// <c>true</c> if the type was able to be changed to the <paramref name="destinationType"/>, otherwise <c>false</c>
		/// </returns>
		/// <param name='obj'>
		/// Object being extended
		/// </param>
		/// <param name='destinationType'>
		/// The desired type of the value
		/// </param>
		/// <param name='value'>
		/// The value with the changed type
		/// </param>
		public static bool TryChangeType(this object obj, Type destinationType, out object value)
		{
			bool result = false;
			value = null;
			
			if (obj != null)
			{
				Type sourceType = obj.GetType();
				
				// If the source is already an instance of the destination, do nothing
				if (destinationType.IsInstanceOfType(obj))
				{
					value = obj;
					result = true;
				}
				else
				{
					// Otherwise, get the TypeConverter for the destination and attempt to convert to the destination type
					TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
					if (converter.CanConvertFrom(sourceType))
					{
						try
						{
							value = converter.ConvertTo(obj, destinationType);
							result = true;
						}
						catch
						{
							value = null;
							result = false;
						}
					}
				}
			}
			
			return result;
		}
	}
}

