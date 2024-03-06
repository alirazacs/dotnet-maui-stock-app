using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DotnetTrainingStockApp
{
    public class PreferenceService
    {
        public bool DoesContainsKey(string preferenceStorageKey)
        {
            return Preferences.ContainsKey(preferenceStorageKey);
        }

        public void SetDataInPreferences(string preferenceStorageKey, string preferenceStorageValue)
        {
            Preferences.Set(preferenceStorageKey, preferenceStorageValue);
        }

        public T GetDataFromPreferences<T>(string preferenceStorageKey)
        {
            T UnpackedValue = default;
            string keyvalue = Preferences.Get(preferenceStorageKey, string.Empty);

            if (keyvalue != null && !string.IsNullOrEmpty(keyvalue))
            {
                UnpackedValue = JsonSerializer.Deserialize<T>(keyvalue);
            }
            return UnpackedValue;
        }
    }
}
