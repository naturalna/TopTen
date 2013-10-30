using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTenApp.Models;
using TopTenApp.Services;

namespace TopTenApp.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        private readonly IDataService dataService;

        public SearchViewModel()
        {
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();
            this.Articles = new ObservableCollection<ArticlesInGroup>();
        }

        public string QueryText { get; set; }
        public ObservableCollection<ArticlesInGroup> Articles { get; set; }
        
        public async Task GetAllFoundResults()
        {
            var all = await this.dataService.GetAllArticlesByQuery(this.QueryText);
            this.Articles.Clear();

            foreach (var item in all)
            {
                this.Articles.Add(item);
            }
        }
    }
}
