using FawfulFinance.EdgarBackendScraperAPI.Data.Dao;
using FawfulFinance.EdgarBackendScraperAPI.Data.Dao.Interfaces;
using FawfulFinance.EdgarBackendScraperAPI.Services;
using FawfulFinance.EdgarBackendScraperAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEdgarDao, EdgarDao>();
builder.Services.AddScoped<ICompanyFilingsService, CompanyFilingsService>();

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

