using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ComplaintTracking.Data
{
    public static class DataSQLHelper
    {
        // From https://github.com/aspnet/EntityFrameworkCore/issues/1862#issuecomment-331081468
        // This function is only necessary until ad hoc mapping of arbitrary types
        // is added to Entity Framework Core
        public static async Task<List<T>> ExecSQL<T>(string query, ApplicationDbContext context, int? Timeout = null)
        {
            using var command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            if (Timeout.HasValue) command.CommandTimeout = Timeout.Value;
            context.Database.OpenConnection();
            using var result = await command.ExecuteReaderAsync();
            List<T> list = new List<T>();
            T obj = default;
            while (await result.ReadAsync())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties().Where(p => p.CanWrite))
                {
                    if (!object.Equals(result[prop.Name], DBNull.Value))
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