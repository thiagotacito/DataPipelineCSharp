using DataPipeline.Domain.Entities.DataTraffic.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Domain.Entities.DataTraffic
{
    public class Vehicle
    {
        public Guid TenantId { get; private set; }
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime Timestamp { get; private set; }
        public string Place { get; private set; }   
        public string City { get; private set; }
        public string State { get; private set; }
        public decimal PaidAmount { get; private set; }
        public VehicleTypeEnum VehicleType { get; private set; }
        public bool IsOfficialVehicle { get; private set; }

        public Vehicle(Guid tenantId, DateTime timestamp, string place, string city, string state,
                        decimal paidAmount, VehicleTypeEnum vehicleType, bool isOfficialVehicle)
        {
            TenantId = tenantId;
            Timestamp = timestamp;
            Place = place;
            City = city;
            State = state;
            PaidAmount = paidAmount;
            VehicleType = vehicleType;
            IsOfficialVehicle = isOfficialVehicle;
        }

    }
}
