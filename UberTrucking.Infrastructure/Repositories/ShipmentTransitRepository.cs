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
    public class ShipmentTransitRepository : IShipmentTransitRepository
    {
        private readonly IDapperSqlHelper dapperSqlHelper;

        #region Queries
        private readonly string createShipmentQuery = "insert into shipment_transits values (@pickup_address, @pickup_latitude, @pickup_longitude, @delivery_address, @delivery_latitude, @delivery_longitude,@address_data, @user_id, @height, @width, @length, @description)";
        #endregion

        public ShipmentTransitRepository(IDapperSqlHelper dapperSqlHelper)
        {
            this.dapperSqlHelper = dapperSqlHelper;
        }

        public async Task CreateShimentTransitAsync(ShipmentTransit shipmentTransit)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@pickup_address", shipmentTransit.PickupAddress);
                parameters.Add("@pickup_latitude", shipmentTransit.PickupLatitude);
                parameters.Add("@pickup_longitude", shipmentTransit.PickupLongitude);
                parameters.Add("@delivery_address", shipmentTransit.DeliveryAddress);
                parameters.Add("@delivery_latitude", shipmentTransit.DeliveryLatitude);
                parameters.Add("@delivery_longitude", shipmentTransit.DeliveryLongitude);
                parameters.Add("@address_data", shipmentTransit.AddressData);
                parameters.Add("@user_id", shipmentTransit.UserId);
                parameters.Add("@height", shipmentTransit.Height);
                parameters.Add("@width", shipmentTransit.Width);
                parameters.Add("@length", shipmentTransit.Length);
                parameters.Add("@description", shipmentTransit.Description);
                //@, @, @, @

                var result = await this.dapperSqlHelper.ExecuteAsync(createShipmentQuery, parameters); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
    }
}
