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

// ✅ Run EF Core migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // <-- This applies any pending migrations
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("allow-all");
app.MapControllers();

app.Run();
