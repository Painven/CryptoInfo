namespace CryptoInfo.Models;

public record CryptoCurrencyInfo
{
    public string TranslatedName { get; init; }
    public string ShortCodeName { get; init; }
    public decimal RatioToUSD { get; init; }
    public int TotalMarketCapitalizationInUSD { get; init; }
    public string CurrencyDetailsPage { get; init; }
    public double TotalMarketVolumePercent { get; init; }
    public double DayChangePercent { get; init; }
    public double WeekChangePercent { get; init; }
}
