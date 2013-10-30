using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTenApp.Models;
using Windows.UI.Popups;

namespace TopTenApp.Services
{
    public class RoamingSettingsService : IRoamingSettingsService
    {
        public async Task<List<string>> GetAllFavorite()
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            List<string> allFavoriteSelection = new List<string>();
            string favoriteSelectionFromRoaming = (string)roamingSettings.Values["favorite"];
            roamingSettings.Values["favorite"] = null;
            if (favoriteSelectionFromRoaming != null && favoriteSelectionFromRoaming != "")
            {
                allFavoriteSelection = await JsonConvert.DeserializeObjectAsync<List<string>>(favoriteSelectionFromRoaming);
            }

            if (allFavoriteSelection.Count == 0 || allFavoriteSelection == null)
            {
                await new MessageDialog("Вашият лист с любими класации е празен").ShowAsync();
            }
            return allFavoriteSelection;
        }

        public async void WriteToRoamingSettings(Article foundArticle)
        {
            var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;

            List<string> allFavoriteSelection = new List<string>();
            string favoriteSelectionFromRoaming = (string)roamingSettings.Values["favorite"];
            if (favoriteSelectionFromRoaming != null && favoriteSelectionFromRoaming != "")
            {
                allFavoriteSelection = await JsonConvert.DeserializeObjectAsync<List<string>>(favoriteSelectionFromRoaming);
            }

            allFavoriteSelection.Add(foundArticle.ObjectId);
            roamingSettings.Values["favorite"] = await JsonConvert.SerializeObjectAsync(allFavoriteSelection);
            await new MessageDialog("Успешно добавяне.").ShowAsync();
        }

    }
}
