using CurrencyWPF.Models;
using CurrencyWPF.Services.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using CurrencyWPF.Services;
using Microsoft.Win32;

namespace CurrencyWPF.ViewModels
{
    public class DayCurrencyViewModel : BaseViewModel
    {
        private readonly ApiService _apiService;
        private readonly FileService _fileService;

        private DateTime _selectedDate;

        private ObservableCollection<Rate> _rates;
        private bool _isLoading;


        public DayCurrencyViewModel()
        {
            _apiService = new ApiService();
            _fileService = new FileService();

            _selectedDate = DateTime.Today;

            LoadDayRatesCommand = new RelayCommand(LoadExchangeRates);
            SaveToJsonCommand = new RelayCommand(SaveToJson);
            LoadFromJsonCommand = new RelayCommand(LoadFromJson);
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
        public ICommand SaveToJsonCommand { get; }
        public ICommand LoadFromJsonCommand { get; }

        private async void LoadExchangeRates()
        {
            IsLoading = true;
            try
            {
                var rates = await _apiService.GetOnDayExchangeRate(_selectedDate);
                
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
                    await _fileService.SaveDayCurrencyJsonFile(saveFileDialog.FileName, Rates);

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
                    Rates = await _fileService.OpenDayCurrencyJsonFile(openFileDialog.FileName);

                    SelectedDate = Rates.First().Date;

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
