using Microsoft.EntityFrameworkCore;
using Mng.PlatformService;
using Mng.PlatformService.Data;
using Mng.PlatformService.Options;
using Mng.PlatformService.Services.DataSync;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<PlatformContext>(o => o.UseInMemoryDatabase(nameof(PlatformContext)));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<HttpCommandSyncServiceOptions>()
    .Bind(builder.Configuration.GetSection(HttpCommandSyncServiceOptions.SectionName));

builder.Services.AddHttpClient<HttpCommandSyncService>();

builder.Services.AddTransient<ICommandSyncService>(sp => sp.GetRequiredService<HttpCommandSyncService>());

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

app.UseAuthorization();

app.MapControllers();

app.Run();