using DataPipeline.Domain.Entities.DataTraffic;
using DataPipeline.Domain.Entities.DataTraffic.Interfaces;

namespace DataPipeline.Infrastructure.Data.Repositories;

public class VehicleDataRepository : IVehicleDataRepository
{
    private readonly DataPipelineDbContext _context; //TODO: Implement UnitOfWork pattern

    public VehicleDataRepository(DataPipelineDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<Vehicle> vehicles, CancellationToken cancellationToken)
    {
        _context.Vehicles.AddRangeAsync(vehicles);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddVehicleAsync(Vehicle vehicle, CancellationToken cancellationToken)
    {
        _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
