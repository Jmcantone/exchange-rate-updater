namespace ExchangeRateUpdater.Service;
public interface IExchangeRateService
{
    Task<string> GetExchangeRatesData(DateTime date);
}
