using JobManagerClient;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo
    .Console()
    .CreateLogger();

var host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging => logging.ClearProviders().AddSerilog())
    .ConfigureServices(
        services =>
        {
            services.AddHostedService<Worker>();
            services.AddHostedService<Worker1>();
            services.AddHostedService<Worker2>();
            services.AddHostedService<Worker3>();
            services.AddHostedService<Worker4>();
            services.AddHostedService<Worker5>();
        })
    .Build();

await host.RunAsync();