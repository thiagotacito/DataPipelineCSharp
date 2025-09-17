using DataPipeline.Application.Common.File;
using DataPipeline.Application.Services.DataTraffic;
using DataPipeline.Domain.Entities.DataTraffic.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DataPipeline.WebApi.Controllers.DataTraffic;

[ApiController]
[Route("api/datatraffic/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly VehicleDataService _service;
    private readonly string _tempFolder = Path.Combine(Path.GetTempPath(), "vehicle_chunks");

    public VehicleController(VehicleDataService vehicleDataService)
    {
        _service = vehicleDataService;
    }

    [HttpPost("single-vehicle")]
    public async Task<IActionResult> PostSingleVehicle([FromBody] VehicleDto dto, CancellationToken cancellationToken)
    {
        await _service.ReceiveSingle(dto, cancellationToken);
        return Ok();
    }

    [HttpPost("upload-dpp-file")]
    public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("File is empty or not provided.");
        }

        await _service.ReceiveFileAsync(file.OpenReadStream(), CancellationToken.None);
        return Ok();
    }

    [HttpPost("upload-dpp-file-chunk")]
    public async Task<IActionResult> UploadFileChunk(
        [FromForm] IFormFile chunk, 
        [FromForm] string fileId, 
        [FromForm] int chunkIndex, 
        [FromForm] int totalChunks, 
        [FromForm] bool isLastChunk, 
        [FromForm] string? expectedChecksum = null)
    {
        if (chunk == null || chunk.Length == 0 || string.IsNullOrEmpty(fileId))
        {
            return BadRequest("Chunk is empty or not provided, or fileId is missing.");
        }
        var fileFolder = Path.Combine(_tempFolder, fileId);
        Directory.CreateDirectory(fileFolder);
        var chunkPath = Path.Combine(fileFolder, $"{fileId}_{chunkIndex}.part");

        using (var stream = new FileStream(chunkPath, FileMode.Create))
        {
            await chunk.CopyToAsync(stream);
        }

        if (isLastChunk)
        {
            var mergedPath = Path.Combine(fileFolder, $"{fileId}_merged.tmp");
            var finalStream = await FileHandler.MergeChunkFile(mergedPath, fileFolder, fileId);

            var isValid = await FileHandler.Checksum(finalStream, mergedPath, expectedChecksum);
            if (!isValid)
            {
                return BadRequest("File checksum does not match.");
            }
            
            await _service.ReceiveFileAsync(finalStream, CancellationToken.None);           
            return Ok("File processed successfuly");
        }

        return Ok($"Chunk {chunkIndex} uploaded successfully.");
    }
}
