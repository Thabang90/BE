using UberTrucking.Infrastructure.Repositories.Interfaces;
using UberTrucking.Infrastructure.Repositories;
using UberTrucking.Services.Services;
using UberTrucking.Services.Services.Interfaces;
using UberTrucking.Infrastructure.Data.Interfaces;
using UberTrucking.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDapperSqlHelper>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration["SqlServerConn"];
    return new DapperSqlHelper(connectionString);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShipmentTransitRepository, ShipmentTransitRepository>();
builder.Services.AddScoped<IShipmentTransitService, ShipmentTransitService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
