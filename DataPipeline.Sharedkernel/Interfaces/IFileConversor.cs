using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Sharedkernel.Interfaces;

public interface IFileConversor
{
    IAsyncEnumerable<string[]> ConvertFileAsync(Stream input, CancellationToken cancellationToken);
}
