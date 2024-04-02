namespace ExchangeRateUpdater.Model;

public class ExchangeRateData
{
    public List<ExchangeRateInfo> Rates { get; set; }
}

public class ExchangeRateInfo
{
    public string CurrencyCode { get; set; }
    public decimal Rate { get; set; }
}
