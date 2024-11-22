using CurrencyWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CurrencyWPF.Interfaces
{
    public interface IApiService
    {
        Task<IEnumerable<Currency>> GetCurrenciesInfo();
        Task<IEnumerable<Rate>> GetOnDayExchangeRate(DateTime onDate);
        Task<IEnumerable<RateShort>> GetIntervalExchangeRate(DateTime startDate, DateTime endDate);
    }
}
