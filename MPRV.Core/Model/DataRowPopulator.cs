using System;
using System.Data;
using MPRV.Common.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;

namespace MPRV.Model
{
	public class DataRowPopulator
	{
		#region Attributes

		public class ColumnAttribute : Attribute
		{
			#region Constructors

			public ColumnAttribute(string columnName)
			{
				ColumnName = columnName;
			}

			#endregion

			#region Public Properties

			public string ColumnName { get; protected set; }

			#endregion

			#region Public Methods

			public bool TryGetValue(DataRow row, out object value)
			{
				bool result = false;
				value = null;

				if (row != null && row.Table.Columns.Contains(ColumnName))
				{
					value = row[ColumnName];
					result = true;
				}

				return result;
			}

			#endregion
		}

		public class ParentRelationshipAttribute : Attribute
		{
			#region Constructors

			public ParentRelationshipAttribute(string relationshipName)
			{
				RelationshipName = relationshipName;
			}

			#endregion

			#region Public Properties

			public string RelationshipName { get; protected set; }

			#endregion

			#region Public Methods

			public bool TryGetRow(DataRow row, out DataRow parentRow)
			{
				bool result = false;
				parentRow = null;

				if (row != null && row.Table.ParentRelations.Contains(RelationshipName))
				{
					parentRow = row.GetParentRow(RelationshipName);
					result = parentRow != null;
				}

				return result;
			}

			#endregion
		}

		#endregion

		#region Constructors

		public DataRowPopulator(DataRow row)
		{
			_row = row;
		}

		#endregion

		#region Public Methods

		public bool Populator(IReadableModel model)
		{
 			bool result = false;

			model.SetMembers<ColumnAttribute>(grouping => {
				object value = null;
				result = grouping.FirstOrDefault(ca => ca.TryGetValue(_row, out value)) != null || result;

				return value;
			});

			// TODO: implement ParentRelationship assignment
			model.SetMembers<ParentRelationshipAttribute>(grouping => {
				object value = null;
				DataRow row = null;

				if (grouping.Any(pra => pra.TryGetRow(_row, out row)))
				{
					var memberType = grouping.Key.GetMemberType();
					if (memberType.GetGenericTypeDefinition().IsAssignableFrom(typeof(LazyReadableModel<>)))
					{
						var genericType = memberType.GetGenericArguments()[0];
						if (typeof(IReadableModel).IsAssignableFrom(genericType))
						{
							ILazyReadableModel lazyReadableModel = (ILazyReadableModel)Activator.CreateInstance(typeof(LazyReadableModel<>).MakeGenericType(new Type[] { genericType }));
							lazyReadableModel.Populator = new DataRowPopulator(row).Populator;
							value = lazyReadableModel;

							result = true;
						}
					}
					else if (typeof(IReadableModel).IsAssignableFrom(memberType))
					{
						throw new NotImplementedException();
					}
				}

				return value;
			});

			return result;
		}

		#endregion

		#region Protected Fields
		
		protected DataRow _row;
		
		#endregion
	}
}

