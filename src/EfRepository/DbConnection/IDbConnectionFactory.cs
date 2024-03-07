using System.Data;

namespace Cts.EfRepository.DbConnection;

public interface IDbConnectionFactory
{
    IDbConnection Create();
}
