using CurrencyWPF.Models;
using CurrencyWPF.Services;
using CurrencyWPF.Services.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;

namespace CurrencyWPF.ViewModels
{
    public class IntervalCurrencuViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly FileService _fileService;

        private DateTime _startDate;
        private DateTime _endDate;
        private Currency _selectedCurrency;

        private ObservableCollection<Rate> _rates;
        private ObservableCollection<Currency> _currencies;
        private ObservableCollection<Rate> _rateDay;
        private bool _isLoading;
        private bool _isInitializedDayRates;
        private bool _isInitializedCurrencies;

        public IntervalCurrencuViewModel()
        {
            _apiService = new ApiService();
            _fileService = new FileService();

            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
            _isInitializedCurrencies = false;
            _isInitializedDayRates = false;

            LoadCurrenciesCommand = new RelayCommand(LoadCurrencies);
            LoadIntervalRatesShortCommand = new RelayCommand(LoadIntervalShortRates);
            SaveToJsonCommand = new RelayCommand(SaveToJson);
            LoadFromJsonCommand = new RelayCommand(LoadFromJson);

            if(!_isInitializedCurrencies)
            {
                _isInitializedCurrencies = true;
                LoadCurrencies();
            }   
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                RaisePropertyChanged();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Rate> Rates
        {
            get => _rates;
            set => SetProperty(ref _rates, value);
        }

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set => SetProperty(ref _currencies, value);
        }

        public ObservableCollection<Rate> DayRate
        {
            get => _rateDay;
            set
            {
                SetProperty(ref _rateDay, value);
                RaisePropertyChanged();
            }
        }

        public Currency SelectedCurrency
        {
            get => _selectedCurrency;
            set
            {
                _selectedCurrency = value;
                RaisePropertyChanged();
                LoadIntervalShortRates();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadCurrenciesCommand { get; }
        public ICommand LoadIntervalRatesShortCommand { get; }
        public ICommand SaveToJsonCommand { get; }
        public ICommand LoadFromJsonCommand { get; }

        private async void LoadCurrencies()
        {
            if (!_isInitializedDayRates)
                LoadExchangeRates();

            IsLoading = true;

            try
            {
                Currencies = await _apiService.GetCurrenciesInfo();

                var exchangeRatesIds = new HashSet<int>(DayRate.Select(r => r.Cur_ID));
                var currenciesWithExchangeRates = Currencies.Where(c => exchangeRatesIds.Contains(c.Cur_ID)).ToList();

                Currencies = new ObservableCollection<Currency>(currenciesWithExchangeRates);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки валют: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void LoadIntervalShortRates()
        {
            IsLoading = true;

            try
            {
                Rates = await _apiService.GetIntervalExchangeRate(_startDate, _endDate, SelectedCurrency.Cur_ID);

                var enrichedRates = Rates.Select(x =>
                {
                    x.Cur_Scale = SelectedCurrency.Cur_Scale;
                    x.Cur_Name = SelectedCurrency.Cur_Name;

                    return x;
                }).ToList();

                Rates = new ObservableCollection<Rate>(enrichedRates);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async void SaveToJson()
        {
            if (Rates == null || Rates.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения.");

                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    await _fileService.SaveIntervalCurrencyJsonFile(saveFileDialog.FileName, Rates);

                    MessageBox.Show("Данные успешно сохранены в JSON файл.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения данных: {ex.Message}");
                }
            }
        }

        private async void LoadFromJson()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var exchangeRates = await _fileService.OpenIntervalJsonFile(openFileDialog.FileName);

                    Rates = exchangeRates;
                    StartDate = exchangeRates.First().Date;
                    EndDate = exchangeRates.Last().Date;
                    SelectedCurrency = Currencies.FirstOrDefault(x => x.Cur_ID == exchangeRates.First().Cur_ID);

                    MessageBox.Show("Данные успешно загружены из JSON файла.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                }
            }
        }

        private async void LoadExchangeRates()
        {
            if (_isInitializedDayRates)
                return;

            IsLoading = true;

            try
            {
                DayRate = await _apiService.GetOnDayExchangeRate(DateTime.Now.Date);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                _isInitializedDayRates = true;
            }
        }
    }
}
