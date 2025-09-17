using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Application.Common.Interfaces;
public interface IFileSettings
{
    string Delimiter { get; }
    string DateFormat { get; }
}
