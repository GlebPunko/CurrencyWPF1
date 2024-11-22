using CurrencyWPF.Interfaces;
using CurrencyWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWPF.Services
{
    public class FileService : IFileService
    {
        public async Task<ObservableCollection<Rate>> OpenJsonFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            using (StreamReader stream = new StreamReader(filePath, Encoding.UTF8))
            {
                var text = await stream.ReadToEndAsync();

                if (string.IsNullOrEmpty(text))
                    return null;

                var result = JsonConvert.DeserializeObject<ObservableCollection<Rate>>(text);

                if (result.Count() == 0)
                    return null;

                return result;
            }
        }

        public async Task<bool> SaveJsonFile(string savePath, ObservableCollection<Rate> rates)
        {
            if (string.IsNullOrEmpty(savePath) || rates.Count == 0)
                return false;

            var json = JsonConvert.SerializeObject(rates);

            using (StreamWriter stream = new StreamWriter(savePath))
            {
                await stream.WriteAsync(json);

                return true;
            }
        }
    }
}
