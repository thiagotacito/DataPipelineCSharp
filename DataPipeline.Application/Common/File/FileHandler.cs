using DataPipeline.Sharedkernel.Validation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataPipeline.Application.Common.File;
public static class FileHandler
{
    public async static Task<FileStream> MergeChunkFile(string mergedPath, string fileFolder, string fileId)
    {
        using (var mergedStream = new FileStream(mergedPath, FileMode.Create))
        {
            int i = 0;
            while (true)
            {
                var partPath = Path.Combine(fileFolder, $"{fileId}_{i}.part");
                if (!System.IO.File.Exists(partPath))
                    break;

                using (var partStream = new FileStream(partPath, FileMode.Open, FileAccess.Read))
                {
                    await partStream.CopyToAsync(mergedStream);
                }

                System.IO.File.Delete(partPath);
                i++;
            }
        }

        using var finalStream = new FileStream(mergedPath, FileMode.Open, FileAccess.Read);
        return finalStream;
    }

    public async static Task<bool> Checksum(FileStream finalStream, string mergedPath, string? expectedChecksum=null)
    {
        var isValid = await FileValidator.ValidateChecksumAsync(finalStream, expectedChecksum);

        finalStream.Seek(0, SeekOrigin.Begin);
        System.IO.File.Delete(mergedPath);

        return isValid;
    }
}
