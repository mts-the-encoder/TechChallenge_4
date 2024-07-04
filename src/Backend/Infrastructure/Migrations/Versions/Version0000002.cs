using FluentMigrator;

namespace Infrastructure.Migrations.Versions;

[Migration((int)VersionsNumber.MovieTable,"Movies table")]
public class Version0000002 : Migration
{
    public override void Up()
    {
        CreateMovies();
    }

    public override void Down()
    {
    }

    private void CreateMovies()
    {
		var table = Create.Table("Movies");
		VersionBase.CreateTableWithDefaultColumns(table);

		table
			.WithColumn("Name").AsString(255).NotNullable()
			.WithColumn("Director").AsString(100).NotNullable()
			.WithColumn("ReleasedYear").AsString(10).NotNullable()
			.WithColumn("Duration").AsString(20).NotNullable()
			.WithColumn("Country").AsInt32().NotNullable()
			.WithColumn("Gender").AsInt32().NotNullable()
			.WithColumn("Rate").AsDouble().NotNullable()
			.WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Movies_Users_Id", "Users", "Id");
	}
}