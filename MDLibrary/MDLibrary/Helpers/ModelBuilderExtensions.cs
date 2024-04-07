using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MDLibrary.Helpers
{
	public static class ModelBuilderExtensions
	{
		public static void MigrateTablenamesToPostgres(this ModelBuilder builder)
		{
#pragma warning disable CS8604
			foreach (var entity in builder.Model.GetEntityTypes())
			{
				// Replace table names

				entity.SetTableName(entity.GetTableName().ToSnakeCase());

				// Replace column names
				foreach (var property in entity.GetProperties())
				{
					var columnName = property.GetColumnName(StoreObjectIdentifier.Table(property.DeclaringType.GetTableName(), null));
					property.SetColumnName(columnName.ToSnakeCase());
				}

				foreach (var key in entity.GetKeys())
				{
					key.SetName(key.GetName().ToSnakeCase());
				}

				foreach (var key in entity.GetForeignKeys())
				{
					key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
				}

				foreach (var index in entity.GetIndexes())
				{
					index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
				}
			}
#pragma warning restore CS8604
		}
	}
}
