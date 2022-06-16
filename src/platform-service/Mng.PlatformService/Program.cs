using System.Net;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Mng.PlatformService;
using Mng.PlatformService.Data;
using Mng.PlatformService.Grpc;
using Mng.PlatformService.Options;
using Mng.PlatformService.Services.DataSync;
using Mng.PlatformService.Services.Events;

var builder = WebApplication.CreateBuilder(args);

// TODO: Refactor, check if httpPortDefinition is required
builder.WebHost.UseKestrel(options =>
{
    var ports = builder.Configuration.GetSection("Ports");
    var httpPort = ports.GetValue("Http", 80);
    var grpcPort = ports.GetValue("Grpc", 5001);
    options.Listen(IPAddress.Any, httpPort, listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
    options.Listen(IPAddress.Any, grpcPort, listenOptions => listenOptions.Protocols = HttpProtocols.Http2);
});

// Add services to the container.

builder.Services.AddDbContext<PlatformContext>(o =>
{
    if (builder.Environment.IsDevelopment())
    {
        o.UseInMemoryDatabase(nameof(PlatformContext));
    }
    else
    {
        var connectionStringOptions = builder.Configuration.GetSection(SqlConnectionStringOptions.SectionName).Get<SqlConnectionStringOptions>();
        var connectionString = $"Server={connectionStringOptions.Server};Database=PlatformService;User ID={connectionStringOptions.UserId};Password={connectionStringOptions.Password}";
        o.UseSqlServer(connectionString);
    }
});

builder.Services.AddGrpc();

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<HttpCommandSyncServiceOptions>()
    .Bind(builder.Configuration.GetSection(HttpCommandSyncServiceOptions.SectionName));

builder.Services
    .AddOptions<RabbitMqMessageBusServiceOptions>()
    .Bind(builder.Configuration.GetSection(RabbitMqMessageBusServiceOptions.SectionName));

builder.Services.AddHttpClient<HttpCommandSyncService>();
builder.Services.AddTransient<ICommandSyncService>(sp => sp.GetRequiredService<HttpCommandSyncService>());

builder.Services.AddSingleton<IMessageBusService, RabbitMqMessageBusService>();

builder.Services.AddMapser();

var app = builder.Build();
 
app.UseDbInitializer();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapGrpcService<PlatformService>();
    endpoints.MapGet("/_proto/", async context =>
    {
        context.Response.ContentType = "text/plain";

        await context.Response.WriteAsync(
            File.ReadAllText(Path.Combine(app.Environment.ContentRootPath, "Proto", "platform.proto"))
        );
    });
});

app.Run();