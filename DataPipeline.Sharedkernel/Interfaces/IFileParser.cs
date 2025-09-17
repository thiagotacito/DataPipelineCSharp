using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.SharedKernel.Interfaces;
public interface IFileParser<T>
{
    IAsyncEnumerable<string[]> ParseAsync(Stream stream, CancellationToken cancellationToken);
}
