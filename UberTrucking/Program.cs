using UberTrucking.Infrastructure.Repositories.Interfaces;
using UberTrucking.Infrastructure.Repositories;
using UberTrucking.Services.Services;
using UberTrucking.Services.Services.Interfaces;
using UberTrucking.Infrastructure.Data.Interfaces;
using UberTrucking.Infrastructure.Data;
using UberTrucking.Hubs;
using Microsoft.Extensions.Options;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//for testing realtime with signal R
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5000);
    serverOptions.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

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

//builder.Services.AddCors(option =>
//{
//    option.AddPolicy("AllowAll", builder =>
//    {
//        builder.SetIsOriginAllowed(_ => true)
//               .AllowAnyHeader()
//               .AllowAnyMethod()
//               .AllowCredentials();
//    });
//});

builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowLocalhost", builder =>
    {
        builder.WithOrigins("http://localhost:4200", "http://localhost:4200/", "http://localhost:8081", "exp://192.168.1.110:8081", "http://localhost:8080", "http://uber-truck-76ed9258cd67.herokuapp.com", "http://uber-truck-76ed9258cd67.herokuapp.com/", "http://drivermanagement-001-site1.otempurl.com", "http://drivermanagement-001-site1.otempurl.com/") // Angular app URL during development
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

builder.Services.AddSignalR();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShipmentTransitRepository, ShipmentTransitRepository>();
builder.Services.AddScoped<IShipmentTransitService, ShipmentTransitService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDriverPositionRepository, DriverPositionRepository>();
builder.Services.AddScoped<IDriverDetailRepository, DriverDetailRepository>();
builder.Services.AddScoped<IDriverDetailService, DriverDetailService>();

var app = builder.Build();

// Add SignalR endpoint
app.UseCors("AllowLocalhost");
app.MapHub<ChatHub>("/chatHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
