using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UberTrucking.Infrastructure.Entities
{
    public class DriverDetail
    {
        public int DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverSurname { get; set; }
        public string VehicleRegistration { get; set; }    
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public bool IsAvailable { get; set; }
    }
}
