using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ComplaintTracking.Data
{
    public static class DataSqlHelper
    {
        // From https://github.com/aspnet/EntityFrameworkCore/issues/1862#issuecomment-331081468
        // This function is only necessary until ad hoc mapping of arbitrary types
        // is added to Entity Framework Core
        public static async Task<List<T>> ExecSQL<T>(string query, ApplicationDbContext context, int? Timeout = null)
        {
            await using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            if (Timeout.HasValue) command.CommandTimeout = Timeout.Value;
            await context.Database.OpenConnectionAsync();
            await using var result = await command.ExecuteReaderAsync();
            var list = new List<T>();
            while (await result.ReadAsync())
            {
                var obj = Activator.CreateInstance<T>();
                foreach (var prop in obj.GetType().GetProperties().Where(p => p.CanWrite))
                {
                    if (!Equals(result[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, result[prop.Name], null);
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}
