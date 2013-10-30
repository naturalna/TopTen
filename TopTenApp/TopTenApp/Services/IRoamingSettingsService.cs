using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTenApp.Models;

namespace TopTenApp.Services
{
    public interface IRoamingSettingsService
    {
        Task<List<string>> GetAllFavorite();
        void WriteToRoamingSettings(Article foundArticle);
    }
}
