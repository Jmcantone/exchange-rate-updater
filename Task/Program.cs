using ExchangeRateUpdater.Service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = ConfigureServices();

        try
        {
            var provider = serviceProvider.GetRequiredService<IExchangeRateProvider>();
            var currencies = CurrencyInitializer.InitializeCurrencies();
            var rates = await provider.GetExchangeRates(currencies);

            Console.WriteLine($"{rates.Count()} exchange rates have been found:");

            foreach (var rate in rates)
            {
                Console.WriteLine(rate.ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exchange rates could not be recovered");
            Console.WriteLine(e.Message);
        }

        Console.ReadLine();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
        services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();

        return services.BuildServiceProvider();
    }
}
