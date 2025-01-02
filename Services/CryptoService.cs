using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoQuoteApp20.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CryptoQuoteApp20.Services
{
    public class CryptoService
    {
        private readonly HttpClient _httpClient;
        private readonly string _coinMarketCapApiKey;
        private readonly string _exchangeRatesApiKey;

        public CryptoService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _coinMarketCapApiKey = configuration["CoinMarketCap:ApiKey"];
            _exchangeRatesApiKey = configuration["ExchangeRates:ApiKey"];
        }

        public async Task<Dictionary<string, decimal>> GetCryptoQuote(string cryptoCode)
        {
            var quoteResponse = await GetCryptoQuoteFromCoinMarketCap(cryptoCode);
            var exchangeRates = await GetExchangeRates();

            var results = new Dictionary<string, decimal>();
            foreach (var currency in exchangeRates.Keys)
            {
                results[currency] = quoteResponse[cryptoCode] * exchangeRates[currency];
            }

            return results;
        }



        private async Task<Dictionary<string, decimal>> GetCryptoQuoteFromCoinMarketCap(string cryptoCode)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _coinMarketCapApiKey);
            var response = await _httpClient.GetStringAsync($"https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?symbol={cryptoCode}");
            dynamic data = JsonConvert.DeserializeObject(response);
            return new Dictionary<string, decimal>
            {
                { cryptoCode, data.data[cryptoCode].quote.USD.price }
            };
        }

        private async Task<Dictionary<string, decimal>> GetExchangeRates()
        {
            var response = await _httpClient.GetStringAsync($"https://api.exchangeratesapi.io/latest?access_key={_exchangeRatesApiKey}&base=USD");
            dynamic data = JsonConvert.DeserializeObject(response);
            var rates = new Dictionary<string, decimal>();
            rates["EUR"] = data.rates.EUR;
            rates["BRL"] = data.rates.BRL;
            rates["GBP"] = data.rates.GBP;
            rates["AUD"] = data.rates.AUD;
            return rates;
        }
    

}
}
