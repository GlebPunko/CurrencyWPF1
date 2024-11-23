using CurrencyWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CurrencyWPF.Interfaces
{
    public interface IApiService
    {
        Task<ObservableCollection<Currency>> GetCurrenciesInfo();
        Task<ObservableCollection<Rate>> GetOnDayExchangeRate(DateTime onDate);
        Task<ObservableCollection<Rate>> GetIntervalExchangeRate(DateTime startDate, DateTime endDate, int id);
    }
}
