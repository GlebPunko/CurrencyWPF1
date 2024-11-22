using CurrencyWPF.Interfaces;
using CurrencyWPF.Models;
using CurrencyWPF.Services.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using CurrencyWPF.Services;

namespace CurrencyWPF.ViewModels
{
    public class DayCurrencyViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private DateTime _selectedDate;
        private ObservableCollection<Rate> _rates;
        private bool _isLoading;

        public DayCurrencyViewModel()
        {
            _apiService = new ApiService();
            _selectedDate = DateTime.Today;
            
            LoadDayRatesCommand = new RelayCommand(LoadExchangeRates);
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Rate> Rates
        {
            get => _rates;
            set => SetProperty(ref _rates, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public ICommand LoadDayRatesCommand { get; }

        private async void LoadExchangeRates()
        {
            IsLoading = true;
            try
            {
                var rates = await _apiService.GetOnDayExchangeRate(_selectedDate);
                if (rates.Count() != 0)
                {
                    MessageBox.Show(rates.Count().ToString());
                }
                Rates = new ObservableCollection<Rate>(rates);
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
    }
}
