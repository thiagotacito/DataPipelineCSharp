using DataPipeline.Domain.Entities.DataTraffic.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Infrastructure.Tenant;

public class HttpTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid TenantId
    {
        get
        {
            //TODO: Implement a more robust way to get tenant ID, e.g., from JWT token or user claims
            //TODO: Implement Middleware to validate tenant ID in headers
            var headers = _httpContextAccessor.HttpContext?.Request?.Headers;
            if (headers != null && headers.TryGetValue("X-Tenant-ID", out var tenantIdHeader))
            {
                if (Guid.TryParse(tenantIdHeader, out var tenantId))
                {
                    return tenantId;
                }
            }
            throw new Exception("Tenant ID not found in headers or invalid format.");
        }
    }
}

