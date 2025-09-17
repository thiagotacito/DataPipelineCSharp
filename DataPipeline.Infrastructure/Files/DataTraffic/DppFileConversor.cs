using DataPipeline.Infrastructure.Settings;
using DataPipeline.Sharedkernel.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Infrastructure.Files.DataTraffic;
/// <summary>
/// Dpp is a binary file format used for storing structured data, that means Data Pipeline Protocol
/// </summary>
public class DppFileConversor : IFileConversor
{

    private readonly CsvSettings _settings;

    public DppFileConversor(IOptions<CsvSettings> options)
    {
        _settings = options.Value;
    }

    public async IAsyncEnumerable<string[]> ConvertFileAsync(Stream input, CancellationToken cancellationToken)
    {
        var buffer = new List<byte>();
        int b;

        // Stream does not have ReadByteAsync, use ReadAsync instead
        var singleByte = new byte[1];
        while (await input.ReadAsync(singleByte, 0, 1, cancellationToken) != 0)
        {
            b = singleByte[0];
            
            if (b == '\n')
            {
                var line = Encoding.UTF8.GetString(buffer.ToArray());
                yield return ProcessLine(line);
                buffer.Clear();
            }
            else
            {
                buffer.Add((byte)b);
            }
        }

        // Last Line (in case it doesn't end with \n)
        if (buffer.Count > 0)
        {
            var line = Encoding.UTF8.GetString(buffer.ToArray());
            yield return ProcessLine(line);
        }
    }

    private string[] ProcessLine(string line)
    {
        return line.Split(_settings.Delimiter);
    }
}
