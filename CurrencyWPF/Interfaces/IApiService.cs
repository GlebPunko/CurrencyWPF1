using CurrencyWPF.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyWPF.Interfaces
{
    internal interface IApiService
    {
        Task<IEnumerable<Rate>> GetOnDayExchangeRate(DateTime onDate);
        Task<IEnumerable<RateShort>> GetIntervalExchangeRate(DateTime startDate, DateTime endDate);
    }
}
