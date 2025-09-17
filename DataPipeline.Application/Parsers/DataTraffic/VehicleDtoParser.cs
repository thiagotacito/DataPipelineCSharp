using DataPipeline.Domain.Entities.DataTraffic.DTOs;
using DataPipeline.Domain.Entities.DataTraffic.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Application.Parsers.DataTraffic;
public static class VehicleDtoParser
{
    public static VehicleDto? Parse(string[] columns, string settingsDateFormat)
    {
        if (columns.Length != 7) return null;

        if (!DateTime.TryParse(columns[0], out var timestamp) ||
            !decimal.TryParse(columns[4], out var paidAmount) ||
            !bool.TryParse(columns[6], out var isOfficialVehicle))
            return null;

        return new VehicleDto
        {
            Timestamp = DateTime.ParseExact(columns[0], settingsDateFormat, CultureInfo.InvariantCulture),
            Place = columns[1],
            City = columns[2],
            State = columns[3],
            PaidAmount = paidAmount,
            Type = Enum.Parse<VehicleTypeEnum>(columns[5]),
            IsOfficialVehicle = isOfficialVehicle
        };
    }
}
