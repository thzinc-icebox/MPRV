using System;
using MPRV.Common;
using MPRV.Common.Reflection;
using System.Linq;

namespace MPRV.Model.Data
{
	// TODO: Create extension method for generating ParameterLists
	public static class IWritableModelExtensions
	{
		public static ParameterList BuildParameterList<TWritableModel>(this IWritableModel<TWritableModel> writableModel)
			where TWritableModel : IReadableModel, new()
		{
			ParameterList parameters = new ParameterList();
			var writableModelType = writableModel.GetType();
			var members = writableModelType.GetMembers<ParameterList.ParameterAttribute>(TypeExtensions.PROPERTY_AND_FIELD_BINDING_FLAGS, true);

			foreach (var member in members)
			{
				foreach (var parameterAttribute in member)
				{
					parameters.Add(parameterAttribute.Name, writableModel.GetMember(member.Key));
				}
			}

			var reflectiveMembers = writableModelType.GetMembers<ParameterList.ReflectiveParameterAttribute>(TypeExtensions.PROPERTY_AND_FIELD_BINDING_FLAGS, true);

			foreach (var reflectiveMember in reflectiveMembers)
			{
				foreach (var reflectiveParameterAttribute in reflectiveMember)
				{
					var reflectedType = reflectiveMember.Key.GetMemberType();

					var reflectedMember = reflectedType
						.GetMembers<ParameterList.ParameterAttribute>(TypeExtensions.PROPERTY_AND_FIELD_BINDING_FLAGS, true)
						.FirstOrDefault(g => g.Any(pa => pa.Name.Equals(reflectiveParameterAttribute.ReflectedParameterName)));

					if (reflectedMember != null)
					{
						parameters.Add(reflectiveParameterAttribute.Name, writableModel.GetMember(reflectiveMember.Key).GetMember(reflectedMember.Key));
					}
				}
			}

			return parameters;
		}
	}
}

