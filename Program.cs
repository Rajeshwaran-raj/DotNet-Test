using Dotnet8MySqlCrud.Data;
using Microsoft.EntityFrameworkCore;
using dotenv.net;

DotEnv.Load(); // Load .env file into environment variables

var builder = WebApplication.CreateBuilder(args);

// SQL Server EF Core
var conn = Environment.GetEnvironmentVariable("MSSQL_CONN");

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(conn);
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
