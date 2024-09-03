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

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Listen(IPAddress.Any, 5001, listenOptions =>
//    {
//        listenOptions.UseHttps();
//    });
//});

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
builder.Services.AddCors(option =>
{
    option.AddPolicy("AllowAll", builder =>
    {
        builder.SetIsOriginAllowed(_ => true)
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

var app = builder.Build();

//app.UseCors(builder => builder
//    .AllowAnyHeader()
//    .AllowAnyMethod()
//    .AllowCredentials()
//    .WithOrigins("http://localhost:7267/"));

// Add SignalR endpoint
app.UseCors("AllowAll");
app.MapHub<ChatHub>("/chatHub");

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
