using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Dental.Services
{
    public class DataPersistence<T> : IDataPersistence<T> where T : class
    {
        private readonly string fileName = $"{typeof(T).Name}.json";

        public async Task LoadAsync(ICollection<T> collection)
        {
            if (!File.Exists(fileName))
            {
                return;
            }

            try
            {
                var content = await File.ReadAllTextAsync(fileName);
                var items = JsonConvert.DeserializeObject<ICollection<T>>(content);

                foreach(var item in items!)
                {
                    collection.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task SaveAsync(ICollection<T> collection)
        {
            try
            {
                var items = JsonConvert.SerializeObject(collection, Formatting.Indented);   
                await File.WriteAllTextAsync(fileName, items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
