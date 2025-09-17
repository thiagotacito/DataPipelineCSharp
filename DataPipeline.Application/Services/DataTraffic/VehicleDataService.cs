using DataPipeline.Application.Common.Interfaces;
using DataPipeline.Application.Parsers.DataTraffic;
using DataPipeline.Domain.Entities.DataTraffic;
using DataPipeline.Domain.Entities.DataTraffic.DTOs;
using DataPipeline.Domain.Entities.DataTraffic.Interfaces;
using DataPipeline.Sharedkernel.Validation;
using DataPipeline.SharedKernel.Interfaces;

namespace DataPipeline.Application.Services.DataTraffic;

public class VehicleDataService
{
    private readonly IVehicleDataRepository _vehicleDataRepository;
    private readonly ITenantProvider _tenantProvider;
    private readonly IFileParser<VehicleDto> _fileParser;
    private readonly IFileSettings _fileSettings;

    public VehicleDataService(IVehicleDataRepository vehicleDataRepository, ITenantProvider tenantProvider, IFileParser<VehicleDto> fileParser, IFileSettings fileSettings)
    {
        _vehicleDataRepository = vehicleDataRepository;
        _tenantProvider = tenantProvider;
        _fileParser = fileParser;
        _fileSettings = fileSettings;
    }

    public async Task ReceiveSingle(VehicleDto dto, CancellationToken cancellationToken)
    {

        var vehicle = new Vehicle(
            _tenantProvider.TenantId,
            dto.Timestamp,
            dto.Place,
            dto.City,
            dto.State,
            dto.PaidAmount,
            dto.Type,
            dto.IsOfficialVehicle
        );

        await _vehicleDataRepository.AddVehicleAsync(vehicle, cancellationToken);
    }

    public async Task ReceiveFileAsync(Stream fileStream, CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;
        await foreach (var line in _fileParser.ParseAsync(fileStream, cancellationToken))
        {
            var dto = VehicleDtoParser.Parse(line, _fileSettings.DateFormat);
            var vehicle = new Vehicle(
                tenantId,
                dto.Timestamp,
                dto.Place,
                dto.City,
                dto.State,
                dto.PaidAmount,
                dto.Type,
                dto.IsOfficialVehicle
            );

            await _vehicleDataRepository.AddVehicleAsync(vehicle, cancellationToken);
        }
    }
}
