using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UberTrucking.Infrastructure.Data.Interfaces;
using UberTrucking.Infrastructure.Entities;
using UberTrucking.Infrastructure.Repositories.Interfaces;

namespace UberTrucking.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperSqlHelper dapperSqlHelper;

        #region Queries
        private readonly string createUserQuery = "insert into users values (@name,@surname,@email,@phone_number,@role_id,@password)";
        private readonly string getUserByEmailQuery = "select * from users with(nolock) where email = @email";
        #endregion

        public UserRepository(IDapperSqlHelper dapperSqlHelper)
        {
            this.dapperSqlHelper = dapperSqlHelper;
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@name", user.Name);
                parameters.Add("@surname", user.Surname);
                parameters.Add("@email", user.Email);
                parameters.Add("@phone_number", user.PhoneNumber);
                parameters.Add("@password", user.Password);
                parameters.Add("@role_id", user.RoleId);

                var result = await this.dapperSqlHelper.ExecuteAsync(createUserQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<User> RetrieveUserByEmailAsync(string email)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@email", email);

                var result = await this.dapperSqlHelper.QueryFirstOrDefaultAsync<User>(this.getUserByEmailQuery, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
