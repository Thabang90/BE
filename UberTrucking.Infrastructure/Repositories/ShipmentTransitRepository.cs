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
        private readonly string createShipmentQuery = 
                @"INSERT INTO shipment_transits VALUES (@pickup_address, @pickup_latitude, @pickup_longitude, @delivery_address, @delivery_latitude, @delivery_longitude,@address_data, @user_id, @height, @width, @length, @description)";

        private readonly string createTransactionQuery =
                @"INSERT INTO shipment_transactions VALUES (@shipment_id, @price, @distance, @payment_method)";

        private readonly string updateShipmentDriverId =
                @"Update shipment_transits
                  SET driver_id = @driver_id
                  WHERE id = @id";

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

                var result = await this.dapperSqlHelper.ExecuteAsync(createShipmentQuery, parameters); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task CreateShipmentTransactionAsync(ShipmentTransaction shipmentTransaction)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@shipment_id", shipmentTransaction.ShipmentId);
                parameters.Add("@price", shipmentTransaction.Price);
                parameters.Add("@distance", shipmentTransaction.Distance);
                parameters.Add("@payment_method", shipmentTransaction.PaymentMethod);

                var result = await this.dapperSqlHelper.ExecuteAsync(this.createTransactionQuery, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task UpdateShipmentDriverAsync(int shipmentId, int driver_id)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", shipmentId);
                parameters.Add("@driver_id", driver_id);

                var result = await this.dapperSqlHelper.ExecuteAsync(this.updateShipmentDriverId, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }


        }
    }
}
