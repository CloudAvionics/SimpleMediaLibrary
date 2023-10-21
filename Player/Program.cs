using DataAccessService;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Player;
using Player.Services;
using SimpleLibrary.Persistence;
using SimpleMediaLibrary.Common;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddSingleton<IMediaFileEventMediator, MediaFileEventMediator>();
builder.Services.AddSingleton<IFileSyncService, FileSyncService>();
builder.Services.AddSingleton<IMediaFileMetaDataService, MediaFileMetaDataService>();
builder.Services.AddSingleton<IMediaFileConfiguration, MediaFileConfiguration>();
builder.Services.AddScoped<IBlazorEventService, BlazorEventService>();
builder.Services.AddDataAccesServices(builder.Configuration);
builder.Services.AddHostedService<FileManagementBackgroundService>();

(string password, string certPath) = CertificateBootstrapperRegistration.CheckAndCreateCertificate(builder.Configuration);

builder.WebHost.UseKestrel(options =>
{
    options.ConfigureHttpsDefaults(opt =>
    {
        opt.ServerCertificate = new X509Certificate2(certPath, password);
        opt.SslProtocols = SslProtocols.Tls12;
    });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SimpleLibraryDbContext>();
        // this will apply any pending migrations
        //dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{DateTime.Now}: Exception: {ex}");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// Add middleware to the pipeline.
app.Use(async (context, next) =>
{
    var mediaFileConfig = app.Services.GetRequiredService<IMediaFileConfiguration>();
    var path = context.Request.Path.Value;

    Console.WriteLine($"{DateTime.Now}: Requested path: {path}");  // Debugging line


    // Use the path from the injected AudioFileConfiguration
    var mediaFilePath = mediaFileConfig.MediaFilesPath;

    if (path.StartsWith("/media", StringComparison.OrdinalIgnoreCase))
    {
        var filename = path.Split('/').Last();

        var physicalPath = System.IO.Path.Combine(mediaFilePath, filename);

        Console.WriteLine($"{DateTime.Now}: Physical path: {physicalPath}");  // Debugging line


        if (System.IO.File.Exists(physicalPath))
        {
            await context.Response.SendFileAsync(physicalPath);
            return;
        }
        else
        {
            Console.WriteLine($"{DateTime.Now}: File not found: {physicalPath}");  // Debugging line
        }
    }

    await next();
});


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();