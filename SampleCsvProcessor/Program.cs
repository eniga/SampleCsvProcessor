using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleCsvProcessor;

public class Program
{
    static async Task Main()
    {
        var serviceProvider = new ServiceCollection()
            .AddScoped<IDbConnectionStringProvider, DbConnectionStringProvider>()
            .AddScoped<ICsvService, CsvService>()
            .BuildServiceProvider();

        var service = serviceProvider.GetService<ICsvService>();
        await service.ProcessAsync();

    }
}