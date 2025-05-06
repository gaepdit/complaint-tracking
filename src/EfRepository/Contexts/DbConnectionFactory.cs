using Microsoft.Data.SqlClient;
using System.Data;

namespace Cts.EfRepository.Contexts;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}

public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection Create() => new SqlConnection(connectionString);
}
