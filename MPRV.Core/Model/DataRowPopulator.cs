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
		
		public class ChildRelationshipAttribute : Attribute
		{
			#region Constructors
			
			public ChildRelationshipAttribute(string relationshipName)
			{
				RelationshipName = relationshipName;
			}
			
			#endregion
			
			#region Public Properties
			
			public string RelationshipName { get; protected set; }
			
			#endregion
			
			#region Public Methods
			
			public bool TryGetRows(DataRow row, out IEnumerable<DataRow> childRows)
			{
				bool result = false;
				childRows = null;
				
				if (row != null && row.Table.ChildRelations.Contains(RelationshipName))
				{
					childRows = row.GetChildRows(RelationshipName);
					result = childRows != null;
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

			model.SetMembers<ChildRelationshipAttribute>(grouping => {
				object value = null;
				IEnumerable<DataRow> rows = null;
				
				if (grouping.Any(cra => cra.TryGetRows(_row, out rows)))
				{
					var memberType = grouping.Key.GetMemberType();
					if (memberType.GetGenericTypeDefinition().IsAssignableFrom(typeof(LazyList<>)))
					{
						var genericType = memberType.GetGenericArguments()[0];
						if (typeof(IReadableModel).IsAssignableFrom(genericType))
						{
							var source = rows.Select(row => {
								IReadableModel readableModel = (IReadableModel)Activator.CreateInstance(genericType);
								var populator = new DataRowPopulator(row);
								readableModel.Populate(populator.Populator);

								return readableModel;
							});

							value = Activator.CreateInstance(typeof(LazyList<>).MakeGenericType(new Type[] { genericType }), new object[] { source });
							
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

