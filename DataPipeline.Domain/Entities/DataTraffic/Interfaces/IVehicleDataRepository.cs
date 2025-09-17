using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Domain.Entities.DataTraffic.Interfaces;

    public interface IVehicleDataRepository
    {
        Task AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<Vehicle> vehicles, CancellationToken cancellationToken);
    }
