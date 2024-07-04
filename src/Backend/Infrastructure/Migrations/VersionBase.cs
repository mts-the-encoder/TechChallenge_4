using FluentMigrator.Builders.Create.Table;

namespace Infrastructure.Migrations;

public static class VersionBase
{
    public static ICreateTableColumnOptionOrWithColumnSyntax CreateTableWithDefaultColumns(ICreateTableWithColumnOrSchemaOrDescriptionSyntax table)
    {
        return table.WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("CreatedOn").AsDateTime().NotNullable();
    }
}