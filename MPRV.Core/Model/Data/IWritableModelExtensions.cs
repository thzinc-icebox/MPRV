using System;
using MPRV.Common.Reflection;

namespace MPRV.Model.Data
{
	// TODO: Create extension method for generating ParameterLists
	public static class IWritableModelExtensions
	{
		public static ParameterList BuildParameterList<TWritableModel>(this IWritableModel<TWritableModel> writableModel)
			where TWritableModel : IReadableModel, new()
		{
			ParameterList parameters = new ParameterList();

			var members = writableModel.GetType().GetMembers<ParameterList.ParameterAttribute>(TypeExtensions.PROPERTY_AND_FIELD_BINDING_FLAGS, true);

			foreach (var member in members) {
				foreach (var parameterAttribute in member) {
					parameters.Add(parameterAttribute.Name, writableModel.GetMember(member.Key));
				}
			}

			return parameters;
		}
	}
}

