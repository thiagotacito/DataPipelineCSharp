using DataPipeline.Application.Common.Interfaces;
using DataPipeline.Application.Services.DataTraffic;
using DataPipeline.Domain.Entities.DataTraffic.DTOs;
using DataPipeline.Domain.Entities.DataTraffic.Interfaces;
using DataPipeline.Infrastructure.Data;
using DataPipeline.Infrastructure.Data.Repositories;
using DataPipeline.Infrastructure.Files.DataTraffic;
using DataPipeline.Infrastructure.Settings;
using DataPipeline.Infrastructure.Tenant;
using DataPipeline.Sharedkernel.Interfaces;
using DataPipeline.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<CsvSettings>(builder.Configuration.GetSection("CsvSettings"));
builder.Services.AddSingleton<IFileSettings>(sp =>
    sp.GetRequiredService<IOptions<CsvSettings>>().Value);

builder.Services.AddDbContext<DataPipelineDbContext>(
    opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DataPipelineDbContext>();
builder.Services.AddScoped<IVehicleDataRepository, VehicleDataRepository>();
builder.Services.AddScoped<IHttpContextAccessor>();
builder.Services.AddScoped<ITenantProvider, HttpTenantProvider>();
builder.Services.AddScoped<VehicleDataService>();
builder.Services.AddScoped<IFileParser<VehicleDto>, CsvVehicleParser>();
builder.Services.AddScoped<IFileConversor, DppFileConversor>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
