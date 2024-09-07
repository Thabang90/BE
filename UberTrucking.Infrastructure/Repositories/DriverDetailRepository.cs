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
    public class DriverDetailRepository : IDriverDetailRepository
    {
        private readonly IDapperSqlHelper dapperSqlHelper;

        public DriverDetailRepository(IDapperSqlHelper dapperSqlHelper)
        {
            this.dapperSqlHelper = dapperSqlHelper;
        }

        #region Queries
        private readonly string getDriverDetailsQuery =
                @"SELECT u.name AS DriverName, u.surname AS DriverSurname, 
                         d.vehicle_registration AS VehicleRegistration, d.vehicle_make AS VehicleMake, 
                         d.vehicle_model AS VehicleModel, d.is_available AS IsAvailable
                  FROM driver_details d WITH(NOLOCK)
                  JOIN users u WITH(NOLOCK) ON u.id = d.driver_id
                  WHERE d.driver_id = @driverId AND d.is_available = 1 
                  ";
        #endregion

        public async Task<DriverDetail> GetDriverDetailsAsync(int driverId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@driverId", driverId);

                var result = await this.dapperSqlHelper.QueryFirstOrDefaultAsync<DriverDetail>(this.getDriverDetailsQuery, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
