using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataPipeline.Sharedkernel.Validation;
public static class FileValidator
{
    public async static Task<bool> ValidateChecksumAsync(Stream fileStream, string? expectedChecksum=null)
    {
        if (string.IsNullOrWhiteSpace(expectedChecksum))
            return false;

        if (fileStream.CanSeek)
            fileStream.Seek(0, SeekOrigin.Begin);

        using var sha256 = SHA256.Create();
        var hash = await sha256.ComputeHashAsync(fileStream);

        var actualChecksum = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();

        return actualChecksum == expectedChecksum?.ToLowerInvariant();
    }

}
