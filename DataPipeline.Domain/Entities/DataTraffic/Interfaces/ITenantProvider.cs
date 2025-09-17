using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Domain.Entities.DataTraffic.Interfaces;
public interface ITenantProvider
{
    Guid TenantId { get; }
}
