using CurrencyWPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CurrencyWPF.Interfaces
{
    public interface IFileService
    {
        Task<bool> SaveJsonFile(string savePath, ObservableCollection<Rate> rates);
        Task<ObservableCollection<Rate>> OpenJsonFile(string filePath);
    }
}
