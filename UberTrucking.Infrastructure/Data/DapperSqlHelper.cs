using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Data.Interfaces;

namespace UberTrucking.Infrastructure.Data
{
    public class DapperSqlHelper : IDapperSqlHelper
    {
        private readonly string connectionString;

        public DapperSqlHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<int> ExecuteAsync(string spName, DynamicParameters parameters = null)
        {
            var result = 0;

            using (var connection = new SqlConnection(this.connectionString))
            {
                await connection.OpenAsync();

                using (var scope = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        await connection.ExecuteAsync(spName, parameters, scope, commandType: CommandType.Text);
                        await scope.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await scope.RollbackAsync();
                        throw;
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string spName, DynamicParameters parameters = null)
        {
            var result = new List<T>();

            using (var connection = new SqlConnection(this.connectionString))
            {
                result = (await connection.QueryAsync<T>(spName, parameters, commandType: CommandType.Text)).AsList();
            }

            return result;
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string spName, DynamicParameters parameters = null)
        {
            var result = default(T);

            using (var connection = new SqlConnection(this.connectionString))
            {
                result = await connection.QueryFirstOrDefaultAsync<T>(spName, parameters, commandType: CommandType.Text);
            }

            return result;
        }
    }
}
