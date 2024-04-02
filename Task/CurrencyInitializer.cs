using ExchangeRateUpdater.Model;
using System.Collections.Generic;

namespace ExchangeRateUpdater;

public static class CurrencyInitializer
{
    public static IEnumerable<Currency> InitializeCurrencies()
    {
        return [
            new("USD"),
            new("EUR"),
            new("CZK"),
            new("JPY"),
            new("KES"),
            new("RUB"),
            new("THB"),
            new("TRY"),
            new("XYZ")
        ];
    }
}
