using CurrencyWPF.Interfaces;
using CurrencyWPF.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWPF.Services
{
    public class FileService : IFileService
    {
        public async Task<ObservableCollection<Rate>> OpenDayCurrencyJsonFile(string filePath)
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

        public async Task<ObservableCollection<RateShort>> OpenIntervalJsonFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            using (StreamReader stream = new StreamReader(filePath, Encoding.UTF8))
            {
                var text = await stream.ReadToEndAsync();

                if (string.IsNullOrEmpty(text))
                    return null;

                var result = JsonConvert.DeserializeObject<ObservableCollection<RateShort>>(text);

                if (result.Count() == 0)
                    return null;

                return result;
            }
        }

        public async Task<bool> SaveDayCurrencyJsonFile(string savePath, ObservableCollection<Rate> rates)
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

        public async Task<bool> SaveIntervalCurrencyJsonFile(string savePath, ObservableCollection<RateShort> shortRates)
        {
            if (string.IsNullOrEmpty(savePath) || shortRates.Count == 0)
                return false;

            var json = JsonConvert.SerializeObject(shortRates);

            using (StreamWriter stream = new StreamWriter(savePath))
            {
                await stream.WriteAsync(json);

                return true;
            }
        }
    }
}
