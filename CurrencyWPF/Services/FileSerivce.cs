using CurrencyWPF.Interfaces;
using CurrencyWPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyWPF.Services
{
    public class FileService : IFileService
    {
        public async Task<IEnumerable<SaveRate>> OpenJsonFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            using (StreamReader stream = new StreamReader(filePath, Encoding.UTF8))
            {
                var text = await stream.ReadToEndAsync();

                if (string.IsNullOrEmpty(text))
                    return null;

                var result = JsonConvert.DeserializeObject<IEnumerable<SaveRate>>(text);

                if (result.Count() == 0)
                    return null;

                return result;
            }
        }

        public async Task<bool> SaveJsonFile(string savePath, string json)
        {
            if (string.IsNullOrEmpty(savePath) || string.IsNullOrEmpty(json))
                return false;

            using (StreamWriter stream = new StreamWriter(savePath + $@"\currency_saved_{DateTime.Now.Date}"))
            {
                await stream.WriteAsync(json);

                return true;
            }
        }
    }
}
