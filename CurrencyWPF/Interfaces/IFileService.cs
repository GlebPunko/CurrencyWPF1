using CurrencyWPF.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CurrencyWPF.Interfaces
{
    public interface IFileService
    {
        Task<bool> SaveDayCurrencyJsonFile(string savePath, ObservableCollection<Rate> rates);
        Task<ObservableCollection<Rate>> OpenDayCurrencyJsonFile(string filePath);
        Task<bool> SaveIntervalCurrencyJsonFile(string savePath, ObservableCollection<RateShort> rates);
        Task<ObservableCollection<RateShort>> OpenIntervalJsonFile(string filePath);
    }
}
