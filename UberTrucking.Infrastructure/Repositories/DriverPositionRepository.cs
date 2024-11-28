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
    public class DriverPositionRepository : IDriverPositionRepository
    {
        private readonly IDapperSqlHelper dapperSqlHelper;

        public DriverPositionRepository(IDapperSqlHelper dapperSqlHelper)
        {
            this.dapperSqlHelper = dapperSqlHelper;
        }

        #region Queries
        private readonly string createDriverPositionQuery = "insert into driver_positions values (@driver_id, @shipment_id, @latitude, @longitude, @waypoint)";

        private readonly string updateDriverPositionQuery =
            @"UPDATE driver_positions
              SET latitude = @latitude, longitude = @longitude, waypoint = @waypoint
              WHERE driver_id = @driver_id";

        private readonly string getDriverPositionQuery =
            @"SELECT latitude AS Latitude, longitude AS Longitude, waypoint AS Waypoint, driver_id AS DriverId
              FROM  driver_positions
              WHERE driver_id = @driver_id";
        #endregion

        public async Task AddDriverPositionAsync(DriverPosition driverPosition)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@driver_id", driverPosition.DriverId);
                parameters.Add("@shipment_id", driverPosition.ShipmentId);
                parameters.Add("@latitude", driverPosition.Latitude);
                parameters.Add("@longitude", driverPosition.Longitude);
                parameters.Add("@waypoint", driverPosition.Waypoint);

                var result = await this.dapperSqlHelper.ExecuteAsync(createDriverPositionQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task UpdateDriverPositionAsync(DriverPosition driverPosition)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@driver_id", driverPosition.DriverId);
                parameters.Add("@latitude", driverPosition.Latitude);
                parameters.Add("@longitude", driverPosition.Longitude);
                parameters.Add("@waypoint", driverPosition.Waypoint);

                var result = await this.dapperSqlHelper.ExecuteAsync(updateDriverPositionQuery, parameters);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<DriverPosition> GetDriverPositionAsync(int driverId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@driver_id", driverId);

                var result = await this.dapperSqlHelper.QueryFirstOrDefaultAsync<DriverPosition>(getDriverPositionQuery, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}