using System;
using System.Collections.Generic;

namespace MPRV.Model.Data
{
	public class ParameterList : Dictionary<string, object>
	{
		#region Attributes

		public class ParameterAttribute : Attribute
		{
			#region Constructors

			public ParameterAttribute(string name)
			{
				Name = name;
			}

			#endregion

			#region Public Properties

			public string Name { get; protected set; }

			#endregion
		}

		public class ReflectiveParameterAttribute : Attribute
		{
			#region Constructors
			
			public ReflectiveParameterAttribute(string name)
				: this(name, name)
			{
			}

			public ReflectiveParameterAttribute(string name, string reflectedParameterName)
			{
				Name = name;
				ReflectedParameterName = reflectedParameterName;
			}

			#endregion
			
			#region Public Properties
			
			public string Name { get; protected set; }

			public string ReflectedParameterName {get;protected set;}
			
			#endregion
		}

		#endregion

		#region Constructors

		public ParameterList()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		#endregion
	}
}

