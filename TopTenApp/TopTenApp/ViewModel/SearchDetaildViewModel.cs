using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TopTenApp.Models;
using TopTenApp.Services;

namespace TopTenApp.ViewModel
{
    public class SearchDetaildViewModel : ViewModelBase
    {
        private readonly IDataService dataService;
        private readonly INavigationService navigationService;

        public SearchDetaildViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            this.BackCommand = new RelayCommand<string>((s) =>
            {
                this.navigationService.GoBack();
            });


            this.dataService = new ExternalDataService();
        }

        public Article Article { get; set; }

        public async Task GetArticle(string objectId)
        {
            Article foundArticle = await this.dataService.GetArticleByObjectId(objectId);
            this.Article = foundArticle;
            this.RaisePropertyChanged("Article");
        }

        public ICommand BackCommand { get; private set; }
    }
}