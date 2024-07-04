using FluentMigrator;

namespace Infrastructure.Migrations.Versions;

[Migration((int)VersionsNumber.UserTable, "User table")]
public class Version0000001 : Migration
{
    public override void Up()
    {
        var table = Create.Table("Users");
        VersionBase.CreateTableWithDefaultColumns(table);

        table
            .WithColumn("Name").AsString(100).NotNullable()
            .WithColumn("Email").AsString(100).NotNullable()
            .WithColumn("YearBorn").AsString(4).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }

    public override void Down()
    {
    }
}