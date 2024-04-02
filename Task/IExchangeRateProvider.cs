using ExchangeRateUpdater.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public interface IExchangeRateProvider
{
    Task<IEnumerable<ExchangeRateResponse>> GetExchangeRates(IEnumerable<Currency> currencies);
}