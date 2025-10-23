using Dotnet8MySqlCrud.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// MySQL EF Core
var conn = builder.Configuration.GetConnectionString("Default");
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
