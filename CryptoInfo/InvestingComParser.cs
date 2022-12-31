using CryptoInfo.Models;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CryptoInfo;

public class InvestingComParser
{
    HttpClient client;
    int draw = 1;

    public InvestingComParser()
    {
        client = new HttpClient(new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            CookieContainer = new System.Net.CookieContainer(),
            UseCookies = true,
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
        });
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        client.DefaultRequestHeaders.Add("pragma", "no-cache");
        client.DefaultRequestHeaders.Add("cache-control", "no-cache");
    }

    public async Task<List<CryptoCurrency>> Parse(int pageCounts)
    {
        var list = new List<CryptoCurrency>();

        int currentPage = 1;

        var doc = await GetDocument("https://uk.investing.com/crypto/currencies");

        do
        {
            string postUri = "https://uk.investing.com/crypto/Service/LoadCryptoCurrencies";
            HttpContent payload = PrepareRequestPayload(currentPage);

            var response = await client.PostAsJsonAsync(postUri, payload);
            var json = await response.Content.ReadAsStringAsync();

            break;

        } while (currentPage < pageCounts);

        return list;
    }

    private HttpContent PrepareRequestPayload(int page)
    {
        const int items_per_page = 100;
        string start = ((page - 1) * items_per_page).ToString();

        var dic = new Dictionary<string, string>()
        {
            ["draw"] = (draw++).ToString(),
            ["columns[0][data]"] = "currencies_order",
            ["columns[0][name]"] = "currencies_order",
            ["columns[0][searchable]"] = "true",
            ["columns[0][orderable]"] = "true",
            ["columns[0][search][value]"] = "",
            ["columns[0][search][regex]"] = "false",
            ["columns[1][data]"] = "function",
            ["columns[1][name]"] = "crypto_id",
            ["columns[1][searchable]"] = "true",
            ["columns[1][orderable]"] = "false",
            ["columns[1][search][value]"] = "",
            ["columns[1][search][regex]"] = "false",
            ["columns[2][data]"] = "function",
            ["columns[2][name]"] = "name",
            ["columns[2][searchable]"] = "true",
            ["columns[2][orderable]"] = "true",
            ["columns[2][search][value]"] = "",
            ["columns[2][search][regex]"] = "false",
            ["columns[3][data]"] = "symbol",
            ["columns[3][name]"] = "symbol",
            ["columns[3][searchable]"] = "true",
            ["columns[3][orderable]"] = "true",
            ["columns[3][search][value]"] = "",
            ["columns[3][search][regex]"] = "false",
            ["columns[4][data]"] = "function",
            ["columns[4][name]"] = "price_usd",
            ["columns[4][searchable]"] = "true",
            ["columns[4][orderable]"] = "true",
            ["columns[4][search][value]"] = "",
            ["columns[4][search][regex]"] = "false",
            ["columns[5][data]"] = "market_cap_formatted",
            ["columns[5][name]"] = "market_cap_usd",
            ["columns[5][searchable]"] = "true",
            ["columns[5][orderable]"] = "true",
            ["columns[5][search][value]"] = "",
            ["columns[5][search][regex]"] = "false",
            ["columns[6][data]"] = "24h_volume_formatted",
            ["columns[6][name]"] = "24h_volume_usd",
            ["columns[6][searchable]"] = "true",
            ["columns[6][orderable]"] = "true",
            ["columns[6][search][value]"] = "",
            ["columns[6][search][regex]"] = "false",
            ["columns[7][data]"] = "total_volume",
            ["columns[7][name]"] = "total_volume",
            ["columns[7][searchable]"] = "true",
            ["columns[7][orderable]"] = "true",
            ["columns[7][search][value]"] = "",
            ["columns[7][search][regex]"] = "false",
            ["columns[8][data]"] = "change_percent_formatted",
            ["columns[8][name]"] = "change_percent",
            ["columns[8][searchable]"] = "true",
            ["columns[8][orderable]"] = "true",
            ["columns[8][search][value]"] = "",
            ["columns[8][search][regex]"] = "false",
            ["columns[9][data]"] = "percent_change_7d_formatted",
            ["columns[9][name]"] = "percent_change_7d",
            ["columns[9][searchable]"] = "true",
            ["columns[9][orderable]"] = "true",
            ["columns[9][search][value]"] = "",
            ["columns[9][search][regex]"] = "false",
            ["order[0][column]"] = "currencies_order",
            ["order[0][dir]"] = "asc",
            ["start"] = start,
            ["length"] = "100",
            ["search[value]"] = "",
            ["search[regex]"] = "false",
            ["currencyId"] = "12",
        };

        return new FormUrlEncodedContent(dic);
    }

    private async Task<HtmlDocument> GetDocument(string uri)
    {
        var doc = new HtmlDocument();
        var str = await client.GetStringAsync(uri);
        doc.LoadHtml(str);
        return doc;
    }
}
