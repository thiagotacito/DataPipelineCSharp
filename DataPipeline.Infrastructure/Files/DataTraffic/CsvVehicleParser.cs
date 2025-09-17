using DataPipeline.Domain.Entities.DataTraffic.DTOs;
using DataPipeline.Domain.Entities.DataTraffic.Enums;
using DataPipeline.Infrastructure.Settings;
using DataPipeline.SharedKernel.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Infrastructure.Files.DataTraffic;
public class CsvVehicleParser : IFileParser<VehicleDto>
{
    private readonly CsvSettings _settings;

    public CsvVehicleParser(IOptions<CsvSettings> options)
    {
        _settings = options.Value;
    }

    public async IAsyncEnumerable<string[]> ParseAsync(Stream stream, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);

        if (_settings.HasHeader && !reader.EndOfStream)
        {
            await reader.ReadLineAsync(cancellationToken);
        }

        while(!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(line)) continue;

            yield return line.Split(_settings.Delimiter);
        }
    }
}
