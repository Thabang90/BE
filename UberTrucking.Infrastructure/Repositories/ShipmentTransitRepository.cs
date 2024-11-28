using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
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
                @"
                INSERT INTO shipment_transits (
                    pickup_address, pickup_latitude, pickup_longitude, 
                    delivery_address, delivery_latitude, delivery_longitude, 
                    address_data, user_id, height, width, length, description
                )
                OUTPUT 
                    INSERTED.id AS Id,
                    INSERTED.pickup_address AS PickupAddress,
                    INSERTED.pickup_latitude AS PickupLatitude,
                    INSERTED.pickup_longitude AS PickupLongitude,
                    INSERTED.delivery_address AS DeliveryAddress,
                    INSERTED.delivery_latitude AS DeliveryLatitude,
                    INSERTED.delivery_longitude AS DeliveryLongitude,
                    INSERTED.address_data AS AddressData,
                    INSERTED.user_id AS UserId,
                    INSERTED.height AS Height,
                    INSERTED.width AS Width,
                    INSERTED.length AS Length,
                    INSERTED.description AS Description
                VALUES 
                    (@pickup_address, @pickup_latitude, @pickup_longitude, 
                    @delivery_address, @delivery_latitude, @delivery_longitude, 
                    @address_data, @user_id, @height, @width, @length, @description);
                ";

        private readonly string createTransactionQuery =
                @"INSERT INTO shipment_transactions (shipment_id, price, distance, payment_method)
                  VALUES 
                (@shipment_id, @price, @distance, @payment_method)";

        private readonly string updateShipmentDriverId =
                @"Update shipment_transits
                  SET driver_id = @driver_id
                  WHERE id = @id";

        private readonly string getAvailableShipmentsQuery=
            @"SELECT 
                id AS Id, pickup_address AS PickupAddress ,pickup_latitude AS PickupLatitude, pickup_longitude AS PickupLongitude, delivery_address AS DeliveryAddress, delivery_latitude AS DeliveryLatitude, delivery_longitude AS DeliveryLongitude,address_data AS AddressData, user_id AS UserId, height AS Height, width AS Width, length AS Length, description AS Description
              FROM shipment_transits
              WHERE driver_id IS NULL";

        private readonly string verifyShipmentDriverQuery =
            @"SELECT 
                s.id AS Id, pickup_address AS PickupAddress ,pickup_latitude AS PickupLatitude, 
				pickup_longitude AS PickupLongitude, delivery_address AS DeliveryAddress, 
				delivery_latitude AS DeliveryLatitude, delivery_longitude AS DeliveryLongitude,
				address_data AS AddressData, user_id AS UserId, height AS Height, width AS Width, 
				length AS Length, description AS Description, 
				driver_id AS DriverId, st.price as Price, st.distance AS Distance, st.payment_method AS PaymentMethod, user_acceptance_cost AS UserAccepted
              FROM shipment_transits s
			  LEFT JOIN shipment_transactions st ON st.shipment_id = s.id
              WHERE s.id = @id";

        private readonly string updateShipmentTransactionUserAcceptanceCost =
            @"UPDATE shipment_transactions
              SET user_acceptance_cost = 1
              OUTPUT
                INSERTED.id AS Id,
              	INSERTED.shipment_id AS ShipmentId,
              	INSERTED.price AS Price,
              	INSERTED.distance AS Distance,
              	INSERTED.payment_method AS PaymentMethod,
              	INSERTED.user_acceptance_cost AS UserAcceptanceCost
              WHERE shipment_id = @id";

        private readonly string userShipmentTransitsQuery =
            @"SELECT st.id AS Id, st.pickup_address AS PickupAddress ,st.pickup_latitude AS PickupLatitude, 
                     st.pickup_longitude AS PickupLongitude, st.delivery_address AS DeliveryAddress, 
                     st.delivery_latitude AS DeliveryLatitude, st.delivery_longitude AS DeliveryLongitude,
                     st.address_data AS AddressData, st.user_id AS UserId, st.height AS Height, st.width AS Width, 
                     st.length AS Length, st.description AS Description, sts.price AS Price, 
					 u.name AS DriverName, u.surname AS DriverSurname
              FROM shipment_transits st
			  LEFT JOIN shipment_transactions sts ON sts.shipment_id = st.id
			  LEFT JOIN users u ON u.id = st.driver_id 
              WHERE user_id = @user_id";
        #endregion

        public ShipmentTransitRepository(IDapperSqlHelper dapperSqlHelper)
        {
            this.dapperSqlHelper = dapperSqlHelper;
        }

        public async Task<ShipmentTransit> CreateShimentTransitAsync(ShipmentTransit shipmentTransit)
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

                var result = await this.dapperSqlHelper.QueryFirstOrDefaultAsync<ShipmentTransit>(createShipmentQuery, parameters);
                return result;
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

        public async Task<IEnumerable<ShipmentTransit>> GetAvailableShipmentsAsync()
        {
            var result = await this.dapperSqlHelper.QueryAsync<ShipmentTransit>(this.getAvailableShipmentsQuery);
            return result;
        }

        public async Task<ShipmentTransit> ShipmentHasDriverAsync(int shipmentId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", shipmentId);

                var result = await this.dapperSqlHelper.QueryFirstOrDefaultAsync<ShipmentTransit>(this.verifyShipmentDriverQuery, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<ShipmentTransaction> UpdateShipmentTransactionUserAcceptanceCostAsync(int shipmentId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", shipmentId);

                var result = await this.dapperSqlHelper.QueryFirstOrDefaultAsync<ShipmentTransaction>(this.updateShipmentTransactionUserAcceptanceCost, parameters);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<IEnumerable<ShipmentTransit>> GetShipmentsByUserIdAsync(int userId)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@user_id", userId);

                var result = await this.dapperSqlHelper.QueryAsync<ShipmentTransit>(this.userShipmentTransitsQuery, parameters);
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
