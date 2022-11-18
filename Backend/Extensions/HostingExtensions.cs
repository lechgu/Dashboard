using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dashboard.Backend.GrpcServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Dashboard.Backend.Facilities;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Security.Cryptography.X509Certificates;
using Dashboard.Backend.Hubs;

namespace Dashboard.Backend.Extensions;

public static class HostingExtensions
{
    public static void ConfigureHosting(this ConfigureWebHostBuilder builder, IConfiguration config)
    {
        if (!int.TryParse(config["PORT"], out int port))
        {
            port = 8080;
        }

        var cert = X509Certificate2.CreateFromPemFile(config["CERT_FILE"], config["KEY_FILE"]);
        builder.ConfigureKestrel(opts =>
        {
            opts.ListenAnyIP(port, lo =>
            {
                lo.Protocols = HttpProtocols.Http1AndHttp2;
                lo.UseHttps(cert);
            });
        });
    }

    public static void ConfigureDependencies(this IServiceCollection services, IConfiguration config)
    {
        services.AddGrpc();
        services.AddCors(opts =>
        {
            opts.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(policy => true)
                    .AllowCredentials()
                    .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
            });
        });
        services.AddSignalR();
        services.AddScoped<EventCache>();
    }

    public static void ConfigurePipeline(this WebApplication app, IConfiguration config)
    {
        var staticDir = config["STATIC_DIR"];
        if (!string.IsNullOrEmpty(staticDir) && Directory.Exists(staticDir))
        {
            var fileProvider = new PhysicalFileProvider(staticDir);
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = fileProvider
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                FileProvider = fileProvider
            });
        }
        app.UseCors();
        app.UseRouting();
        app.UseGrpcWeb(new GrpcWebOptions
        {
            DefaultEnabled = true
        });
        app.MapHub<NotificationHub>("/hubs/notifications");
        app.MapGrpcService<CollectorService>();
        app.MapGrpcService<ReporterService>();
    }
}
