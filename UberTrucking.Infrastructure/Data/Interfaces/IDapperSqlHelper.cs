using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace UberTrucking.Infrastructure.Data.Interfaces
{
    public interface IDapperSqlHelper
    {
        Task<int> ExecuteAsync(string spName, DynamicParameters parameters = null);

        Task<IEnumerable<T>> QueryAsync<T>(string spName, DynamicParameters parameters = null);

        Task<T> QueryFirstOrDefaultAsync<T>(string spName, DynamicParameters parameters = null);
    }
}
