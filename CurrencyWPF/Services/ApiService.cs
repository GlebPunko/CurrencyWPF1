using CurrencyWPF.Interfaces;
using CurrencyWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

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
            var linkBase = ConfigurationManager.AppSettings["currenciesUrl"];

            var currencyList = await _httpClient.GetStringAsync(linkBase);

            if (string.IsNullOrEmpty(currencyList))
            {
                MessageBox.Show($"Ошибка: доступные валюты не загружены.");

                throw new ArgumentNullException();
            }    

            var currencies = JsonConvert.DeserializeObject<ObservableCollection<Currency>>(currencyList);

            return currencies;
        }

        public async Task<ObservableCollection<Rate>> GetIntervalExchangeRate(DateTime startDate, DateTime endDate, int id)
        {
            if (startDate > endDate)
            {
                MessageBox.Show($"Ошибка: Дата начала интервала не может быть больше даты конца интервала.");

                throw new ArgumentException();
            }

            if(startDate == default || endDate == default)
            {
                MessageBox.Show($"Ошибка: Дата начала или конца интервала не могут быть нулевыми. Назначте даты.");

                throw new ArgumentNullException();
            }

            var link = $"{ConfigurationManager.AppSettings["intervalCurrenciesUrl"]}{id}?startDate={startDate.Date:yyyy-MM-dd}&endDate={endDate.Date:yyyy-MM-dd}";

            var rateShortsJson = await _httpClient.GetStringAsync(link);

            if (string.IsNullOrEmpty(rateShortsJson))
            {
                MessageBox.Show($"Ошибка: доступные изменения валют не загружены.");

                throw new ArgumentNullException();
            }

            var rateShorts = JsonConvert.DeserializeObject<ObservableCollection<Rate>>(rateShortsJson);

            return rateShorts;
        }

        public async Task<ObservableCollection<Rate>> GetOnDayExchangeRate(DateTime onDate)
        {
            if (onDate == default)
                onDate = DateTime.Now;

            var link = $"{ConfigurationManager.AppSettings["onDayCurrenciesUrl"]}?onDate={onDate.Date:yyyy-MM-dd}&periodicity=0";

            var ratesJson = await _httpClient.GetStringAsync(link);

            if (string.IsNullOrEmpty(ratesJson) || ratesJson == "[]")
            {
                MessageBox.Show($"Ошибка: доступные изменения валют не загружены.");

                throw new ArgumentNullException();
            }    

            var rates = JsonConvert.DeserializeObject<ObservableCollection<Rate>>(ratesJson);

            return rates;
        }
    }
}
