using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TopTenApp.Models;
using TopTenApp.Services;

namespace TopTenApp.ViewModel
{
    public class FavoriteSelectionViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IRoamingSettingsService roamingService;
        private readonly IDataService dataService;
        private ObservableCollection<Article> allFavorite;
        private List<string> favoriteIds;

        public FavoriteSelectionViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.roamingService = SimpleIoc.Default.GetInstance<IRoamingSettingsService>();
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();
            this.AllFavorite = new ObservableCollection<Article>();

            GetAllFavoriteIds();

            this.BackCommand = new RelayCommand<string>((s) =>
            {
                this.navigationService.GoBack();
            });
        }

        private async void GetAllFavoriteIds()
        {
            this.favoriteIds = await this.roamingService.GetAllFavorite();
            await GetAllArticlesByListOfIds();
        }

        public async Task GetAllArticlesByListOfIds()
        {
            foreach (var id in this.favoriteIds)
            {
                Article model = await this.dataService.GetArticleByObjectId(id);
                this.AllFavorite.Add(model);
            }
        }

        public ICommand BackCommand { get; private set; }

        public ICollection<Article> AllFavorite
        {
            get
            {
                return this.allFavorite;
            }
            set
            {
                if (this.allFavorite == null)
                {
                    this.allFavorite = new ObservableCollection<Article>();
                }

                this.allFavorite.Clear();
                foreach (var item in value)
                {
                    this.allFavorite.Add(item);
                }
            }
        } 
    }
}
