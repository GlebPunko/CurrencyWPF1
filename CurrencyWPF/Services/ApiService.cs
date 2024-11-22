using CurrencyWPF.Interfaces;
using CurrencyWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        public async Task<IEnumerable<RateShort>> GetIntervalExchangeRate(DateTime startDate, DateTime endDate)
        {
            if (startDate < endDate || startDate == default || endDate == default)
                return null;

            var rateShortsJson = await _httpClient
                .GetStringAsync($"https://api.nbrb.by/exrates/rates/dynamics/440?startDate={startDate.Date}&endDate={endDate.Date}");

            if (string.IsNullOrEmpty(rateShortsJson))
                return null;

            var rateShorts = JsonConvert.DeserializeObject<IEnumerable<RateShort>>(rateShortsJson);

            return rateShorts;
        }

        public async Task<IEnumerable<Rate>> GetOnDayExchangeRate(DateTime onDate)
        {
            if (onDate == default)
                onDate = DateTime.Now;

            var ratesJson = await _httpClient
                .GetStringAsync($"https://api.nbrb.by/exrates/rates?ondate={onDate.Date:yyyy-MM-dd}&periodicity=0");

            if (string.IsNullOrEmpty(ratesJson))
                return null;

            var rates = JsonConvert.DeserializeObject<IEnumerable<Rate>>(ratesJson);

            return rates;
        }
    }
}
