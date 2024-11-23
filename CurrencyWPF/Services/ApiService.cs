using CurrencyWPF.Interfaces;
using CurrencyWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyWPF.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ObservableCollection<Currency>> GetCurrenciesInfo()
        {
            var currencyList = await _httpClient.GetStringAsync("https://api.nbrb.by/exrates/currencies");

            if (string.IsNullOrEmpty(currencyList))
                return null;

            var currencies = JsonConvert.DeserializeObject<ObservableCollection<Currency>>(currencyList);

            return currencies;
        }

        public async Task<ObservableCollection<Rate>> GetIntervalExchangeRate(DateTime startDate, DateTime endDate, int id)
        {
            if (startDate > endDate || startDate == default || endDate == default)
                return null;
            string s = $"{startDate.Date:yyyy-MM-dd}";
            string e = $"{endDate.Date:yyyy-MM-dd}";
            string link = $"https://api.nbrb.by/exrates/rates/dynamics/{id}?startDate={s}&endDate={e}";
            var rateShortsJson = await _httpClient
                .GetStringAsync(link);

            if (string.IsNullOrEmpty(rateShortsJson))
                return null;

            var rateShorts = JsonConvert.DeserializeObject<ObservableCollection<Rate>>(rateShortsJson);

            return rateShorts;
        }

        public async Task<ObservableCollection<Rate>> GetOnDayExchangeRate(DateTime onDate)
        {
            if (onDate == default)
                onDate = DateTime.Now;

            var ratesJson = await _httpClient
                .GetStringAsync($"https://api.nbrb.by/exrates/rates?ondate={onDate.Date:yyyy-MM-dd}&periodicity=0");

            if (string.IsNullOrEmpty(ratesJson) || ratesJson == "[]")
                return null;

            var rates = JsonConvert.DeserializeObject<ObservableCollection<Rate>>(ratesJson);

            return rates;
        }
    }
}
