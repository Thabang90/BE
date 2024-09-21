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
                         d.driver_id as DriverId,d.vehicle_registration AS VehicleRegistration, d.vehicle_make AS VehicleMake, 
                         d.vehicle_model AS VehicleModel, d.is_available AS IsAvailable
                  FROM driver_details d WITH(NOLOCK)
                  JOIN users u WITH(NOLOCK) ON u.id = d.driver_id
                  WHERE d.driver_id = @driver_id";

        private readonly string createDriverDetailQuery =
                @"INSERT INTO driver_details VALUES (@driver_id, @vehicle_registration, @vehicle_make, @vehicle_model, @is_available)";

        private readonly string updateStatusQuery =
                @"UPDATE driver_details
                  SET is_available = @is_available
                  WHERE driver_id = @driver_id";

        private readonly string getAvailableDrivers =
                @"SELECT u.name AS DriverName, u.surname AS DriverSurname, 
                         d.driver_id as DriverId, d.vehicle_registration AS VehicleRegistration, d.vehicle_make AS VehicleMake, 
                         d.vehicle_model AS VehicleModel, d.is_available AS IsAvailable
                  FROM driver_details d WITH(NOLOCK)
                  JOIN users u WITH(NOLOCK) ON u.id = d.driver_id
                  WHERE d.is_available = 1";

        #endregion

        public async Task<DriverDetail> GetDriverDetailsAsync(int driverId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@driver_id", driverId);

                var result = await this.dapperSqlHelper.QueryFirstOrDefaultAsync<DriverDetail>(this.getDriverDetailsQuery, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task CreateDriverDetailAsync(DriverDetail driverDetail)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@driver_id", driverDetail.DriverId);
                parameters.Add("@vehicle_registration", driverDetail.VehicleRegistration);
                parameters.Add("@vehicle_make", driverDetail.VehicleMake);
                parameters.Add("@vehicle_model", driverDetail.VehicleModel);
                parameters.Add("@is_available", driverDetail.IsAvailable);

                var result = await this.dapperSqlHelper.ExecuteAsync(this.createDriverDetailQuery, parameters);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task UpdateDriverStatusAsync(int driverId,bool isAvailable)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@is_available", isAvailable);
                parameters.Add("driver_id", driverId);

                var result = await this.dapperSqlHelper.ExecuteAsync(this.updateStatusQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<List<DriverDetail>> GetAvailableDriversAsync()
        {
            try
            {
                var result = await this.dapperSqlHelper.QueryAsync<DriverDetail>(getAvailableDrivers);
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
