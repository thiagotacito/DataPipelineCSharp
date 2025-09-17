using DataPipeline.Domain.Entities.DataTraffic.Enums;

namespace DataPipeline.Domain.Entities.DataTraffic.DTOs;

public class VehicleDto
{
    public DateTime Timestamp { get; set; }
    public string Place { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public decimal PaidAmount { get; set; }
    public VehicleTypeEnum Type { get; set; }
    public bool IsOfficialVehicle { get; set; }
}
