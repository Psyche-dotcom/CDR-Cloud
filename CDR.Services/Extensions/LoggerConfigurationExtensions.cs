using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace CDR.Services.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static IServiceCollection UseCDRLoggerService(this IServiceCollection services, string SeqConnectionUrl, string SeqApiKey, string ProjectName)
        {
            services.AddLogging();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("ProjectName", ProjectName)
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.File("Logs/CDRCloud-.txt", rollingInterval: RollingInterval.Day, outputTemplate : "{Timestamp:dd-MM-yyyy HH:mm:ss.fff zzz} {CorrelationId} {Level:u3} {Username} {Message:lj}{Exception}{NewLine}")
                .WriteTo.Seq(SeqConnectionUrl, apiKey: SeqApiKey)
                .MinimumLevel.Verbose()
                .CreateLogger();

            Log.Information("Logger Created");

            return services;
        }

        public static void UseCDRLoggerFactory(this ILoggerFactory Logger)
        {
            Logger.AddSerilog();
        }
    }
}


