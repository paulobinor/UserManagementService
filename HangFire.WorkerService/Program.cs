using Hangfire;
using HangFire.WorkerService;
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHangfire(options =>
        options.UseSqlServerStorage(configuration.GetConnectionString("UserMgtConn"))
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings());
        services.AddHangfireServer();
    });

host.Build().Run();
