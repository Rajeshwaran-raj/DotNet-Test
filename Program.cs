using Dotnet8MySqlCrud.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv; // 👈 Add this

var builder = WebApplication.CreateBuilder(args);

// 👇 Load .env file before configuration is built
Env.Load();

// 👇 Add environment variables to configuration
builder.Configuration.AddEnvironmentVariables();

// MySQL EF Core
var conn = builder.Configuration.GetConnectionString("Default");
// OR: var conn = Environment.GetEnvironmentVariable("ConnectionStrings__Default");

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    var serverVersion = ServerVersion.AutoDetect(conn);
    opt.UseMySql(conn, serverVersion);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("allow-all", p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("allow-all");
app.MapControllers();

app.Run();
