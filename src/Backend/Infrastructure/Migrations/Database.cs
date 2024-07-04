using Dapper;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Migrations;

public static class Database
{
    public static void CreateDb(string dbConnection, string dbName)
    {
        using var myConnection = new SqlConnection(dbConnection);

		myConnection.Open();

		var param = new DynamicParameters();
		param.Add("name", dbName);

		string sql = $@"
            IF NOT EXISTS (SELECT [name] FROM sys.databases WHERE [name] = @name)
            BEGIN
                CREATE DATABASE [{dbName}];
            END";

		myConnection.Execute(sql, param);

		var result = myConnection.Query("SELECT * FROM sys.databases WHERE name = @name", param);

		myConnection.Close();

		Console.WriteLine(result.Any() ? $"Database '{dbName}' created successfully." : $"Database '{dbName}' was not created.");
	}
}