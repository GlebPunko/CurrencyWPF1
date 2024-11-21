using CurrencyWPF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyWPF.Interfaces
{
    public interface IFileService
    {
        Task<bool> SaveJsonFile(string savePath, string json);
        Task<IEnumerable<SaveRate>> OpenJsonFile(string filePath);
    }
}
