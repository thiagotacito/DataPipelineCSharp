using DataPipeline.Application.Common.Interfaces;

namespace DataPipeline.Infrastructure.Settings;
public class CsvSettings : IFileSettings
{
    public string  Delimiter { get; set; }
    public bool HasHeader { get; set; }
    public string DateFormat { get; set; }

}
