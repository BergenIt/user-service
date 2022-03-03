using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Serilog;

namespace UserService.Main
{
    /// <summary>
    /// Program класс
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа сервиса
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            Core.ProjectOptions projectOptions = new();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(Enum.Parse<Serilog.Events.LogEventLevel>(projectOptions.SerilogMinimumLevel))
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Start UserService");

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.ForContext<Program>().Fatal("Fatal exception: {ex}. Service is unavaible.", ex);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// Настройка портов и протоколов
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration configuration = default;

            return Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configureDelegate =>
                {
                    _ = configureDelegate.SetBasePath(Directory.GetCurrentDirectory());
                    _ = configureDelegate.AddJsonFile("appsettings.json", true);
                    _ = configureDelegate.AddEnvironmentVariables();

                    configuration = configureDelegate.Build();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    Core.ProjectOptions projectOptions = new(configuration);

                    _ = webBuilder.ConfigureKestrel(options =>
                       {
                           options.ListenAnyIP(projectOptions.GrpcPort, o =>
                           {
                               o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                           });

                           options.ListenAnyIP(projectOptions.HttpPort, o =>
                           {
                               o.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
                           });
                       });

                    _ = webBuilder.UseStartup<Startup>();
                });
        }
    }
}
