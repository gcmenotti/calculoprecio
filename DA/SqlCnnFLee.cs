using System.Data.SqlClient;

namespace DA;

public class SqlCnnFLee
{
    public string Server { get; set; }
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public SqlCnnFLee(string server, string database, string username, string password)
    {
        Server = server;
        Database = database;
        Username = username;
        Password = password;
    }
    public SqlConnection GetConnection()
    {
        string connectionString = $"Server={Server};Database={Database};User Id={Username};Password={Password};TrustServerCertificate=true;";
        return new SqlConnection(connectionString);
    }
}