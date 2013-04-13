using System;
using System.Data;
using MPRV.Common.Reflection;
using System.Linq;
using System.Collections.Generic;

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

			public bool TryGetRows(DataRow row, out IEnumerable<DataRow> rows)
			{
				bool result = false;
				rows = null;

				if (row != null && row.Table.ParentRelations.Contains(RelationshipName))
				{
					rows = row.GetParentRows(RelationshipName);
					result = rows.Any();
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

			return result;
		}

		#endregion

		#region Protected Fields
		
		protected DataRow _row;
		
		#endregion
	}
}

