using EtlHangfireDemo.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// ✅ Agrega Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ETL API", Version = "v1" });
});


// Conexión a SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Hangfire
builder.Services.AddHangfire(config =>
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//lama el csv
builder.Services.AddSingleton(new CsvPacienteService("Data/pacientes.csv"));
builder.Services.AddTransient<PacienteJob>();

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
      .WriteTo.File("Logs/log.txt"));
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHangfireDashboard(); // Dashboard en /hangfire
app.UseAuthorization();
app.MapControllers();

//Job
using (var scope = app.Services.CreateScope())
{
    var job = scope.ServiceProvider.GetRequiredService<PacienteJob>();

    // Ejecutar una vez al iniciar
    job.Ejecutar();

    // Programar ejecución diaria
    RecurringJob.AddOrUpdate("etl-pacientes", () => job.Ejecutar(), "*/5 * * * *");
}
app.Run();


record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
