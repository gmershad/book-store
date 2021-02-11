using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Catalog.API.Extensions;
using Catalog.API.Infrastructure;
using IntegrationEventLog;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Catalog.API
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);

        public static void Main(string[] args)
        {
            //var configuration = GetConfiguration();

            //try
            //{
            //    var host = CreateHostBuilder(configuration, args);
            //    host.MigrateDbContext<IntegrationEventLogContext>((_, __) => { });

            //    Log.Information("Starting web host ({ApplicationContext})...", AppName);
            //    host.Run();

            //    return 0;
            //}
            //catch (Exception ex)
            //{
            //    return 1;
            //}

            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseIIS();
            });


        //private static IWebHost CreateHostBuilder(IConfiguration configuration, string[] args) =>
        //     WebHost.CreateDefaultBuilder(args)
        //         .UseConfiguration(configuration)
        //         .CaptureStartupErrors(false)
        //         .ConfigureKestrel(options =>
        //         {
        //             var ports = GetDefinedPorts(configuration);
        //             options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
        //             {
        //                 listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
        //             });
        //             options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
        //             {
        //                 listenOptions.Protocols = HttpProtocols.Http2;
        //             });

        //         })
        //         .UseStartup<Startup>()
        //         .ConfigureServices(services => services.AddAutofac())
        //         .UseContentRoot(Directory.GetCurrentDirectory())
        //         .UseWebRoot("Pics")
        //         .Build();

        //private static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        //{
        //    var grpcPort = config.GetValue("GRPC_PORT", 81);
        //    var port = config.GetValue("PORT", 80);
        //    return (port, grpcPort);
        //}

        //private static IConfiguration GetConfiguration()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //        .AddEnvironmentVariables();

        //    var config = builder.Build();

        //    return builder.Build();
        //}

    }
}
