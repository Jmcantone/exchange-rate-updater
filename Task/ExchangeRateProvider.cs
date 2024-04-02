using ExchangeRateUpdater.Model;
using ExchangeRateUpdater.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private const string TargetCurrencyCode = "CZK";
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateProvider(IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRateResponse>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        try
        {
            var responseBody = await _exchangeRateService.GetExchangeRatesData(DateTime.Today);
            var exchangeRates = ParseExchangeRates(responseBody, currencies);
            return exchangeRates;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception($"Error fetching exchange rates: {ex.Message}", ex);
        }
    }

    private IEnumerable<ExchangeRateResponse> ParseExchangeRates(string responseBody, IEnumerable<Currency> currencies)
    {
        var exchangeRates = new List<ExchangeRateResponse>();

        if (string.IsNullOrWhiteSpace(responseBody) || responseBody.Trim() == "[]")
        {
            return exchangeRates;
        }

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var exchangeRateData = JsonSerializer.Deserialize<ExchangeRateData>(responseBody, options);
        if (exchangeRateData?.Rates == null)
        {
            throw new Exception("Exchange rate could not be parsed.");
        }

        var targetCurrency = currencies.FirstOrDefault(c => c.Code == TargetCurrencyCode);
        if (targetCurrency == null)
        {
            throw new Exception($"{TargetCurrencyCode} currency is not provided in the list of currencies.");
        }

        foreach (var rateInfo in exchangeRateData.Rates)
        {
            var sourceCurrency = currencies.FirstOrDefault(c => c.Code == rateInfo.CurrencyCode);
            if (sourceCurrency != null)
            {
                exchangeRates.Add(new ExchangeRateResponse(sourceCurrency, targetCurrency, rateInfo.Rate));
            }
        }

        return exchangeRates;
    }

}