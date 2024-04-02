namespace ExchangeRateUpdater.Service;
public class ExchangeRateService : IExchangeRateService
{
    private const string ApiBaseUrl = "https://api.cnb.cz/cnbapi";
    private const string DateFormat = "yyyy-MM-dd";
    
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<string> GetExchangeRatesData(DateTime date)
    {
        var dateString = date.ToString(DateFormat);
        var queryString = $"/exrates/daily?date={dateString}&lang=EN";
        var requestUri = new Uri($"{ApiBaseUrl}/{queryString}");

        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
