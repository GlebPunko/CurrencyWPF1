using CurrencyWPF.Models;
using CurrencyWPF.Services;
using CurrencyWPF.Services.Commands;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _currentCurrencyName;
        private Currency _currentCurrency;

        private ObservableCollection<RateShort> _rateShorts;
        private ObservableCollection<Currency> _currencies;
        private bool _isLoading;

        public IntervalCurrencuViewModel()
        {
            _apiService = new ApiService();
            _fileService = new FileService();

            _startDate = DateTime.Now;
            _endDate = DateTime.Now;
            _currentCurrency = new Currency();

            LoadIntervalRatesShortCommand = new RelayCommand(LoadIntervalShortRates);
            SaveToJsonCommand = new RelayCommand(SaveToJson);
            LoadFromJsonCommand = new RelayCommand(LoadFromJson);
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

        public string CurrentNameCurrency
        {
            get => _currentCurrencyName;
            set
            {
                _currentCurrencyName = value;
                _currentCurrency = _currencies.Select(x => x).Where(x => x.Cur_Name == value).FirstOrDefault(); ;
                 
                RaisePropertyChanged();
            }
        }

        public Currency CurrentCurrency
        {
            get => _currentCurrency;
            set
            {
                _currentCurrency = value;

                RaisePropertyChanged();
            }
        }

        public ObservableCollection<RateShort> RateShort
        {
            get => _rateShorts;
            set => SetProperty(ref _rateShorts, value);
        }

        public ObservableCollection<Currency> Currencies
        {
            get => _currencies;
            set => SetProperty(ref _currencies, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadIntervalRatesShortCommand { get; }
        public ICommand SaveToJsonCommand { get; }
        public ICommand LoadFromJsonCommand { get; }

        private async void LoadIntervalShortRates()
        {
            IsLoading = true;

            try
            {
                var rateShort = await _apiService.GetIntervalExchangeRate(_startDate, _endDate);

                RateShort = new ObservableCollection<RateShort>(rateShort);
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
            if (RateShort == null || RateShort.Count == 0)
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
                    await _fileService.SaveIntervalCurrencyJsonFile(saveFileDialog.FileName, RateShort);

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

                    RateShort = exchangeRates;
                    StartDate = exchangeRates.First().Date;
                    EndDate = exchangeRates.Last().Date;

                    MessageBox.Show("Данные успешно загружены из JSON файла.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
                }
            }
        }
    }
}
