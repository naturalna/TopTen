using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TopTenApp.Models;
using TopTenApp.Services;
using Windows.UI.Xaml.Controls;

namespace TopTenApp.ViewModel
{
    public class ArticlesInGroupViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private ObservableCollection<ArticlesInGroup> allArticlesInGroup;

        public ArticlesInGroupViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();
            this.AllArticlesInGroup = new ObservableCollection<ArticlesInGroup>();

            GetAllArticles();

            this.BackCommand = new RelayCommand<string>((s) =>
            {
                this.navigationService.GoBack();
            });
        }

        public async Task GetAllArticles()
        {
            IEnumerable<ArticlesInGroup> all = new List<ArticlesInGroup>();

            Groups group = ((App)App.Current).SelectedGroup[0] as Groups;
            string objectId = group.ObjectId;

            var parseGroup = await this.dataService.GetGroupByObjectId(objectId);

            if (parseGroup != null)
            {
                all = await this.dataService.GetAllArticlesByGroup(parseGroup);
                this.AllArticlesInGroup = all;
            }
            else
            {
                throw new ArgumentNullException("Group not selected");
            } 
        }

        private ICommand selectionChange;

        public ICommand SelectionChange
        {
            get
            {
                if (this.selectionChange == null)
                {
                    this.selectionChange = new RelayCommand<object>((param) => { this.HandleSelectionChangeCommand(param); });
                }
                return this.selectionChange;
            }
        }

        private void HandleSelectionChangeCommand(object parameter)
        {
            var args = parameter as SelectionChangedEventArgs;
            var selected = args.AddedItems;
            ((App)App.Current).SelectedArticle = selected;
            this.navigationService.Navigate(ViewsType.Article);
        }

        public ICommand BackCommand { get; private set; }

        public IEnumerable<ArticlesInGroup> AllArticlesInGroup
        {
            get
            {
                return this.allArticlesInGroup;
            }
            set
            {
                if (this.allArticlesInGroup == null)
                {
                    this.allArticlesInGroup = new ObservableCollection<ArticlesInGroup>();
                }

                this.allArticlesInGroup.Clear();
                foreach (var item in value)
                {
                    this.allArticlesInGroup.Add(item);
                }
            }
        }
    }
}
