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
    }
}
