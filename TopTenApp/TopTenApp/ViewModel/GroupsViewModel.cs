using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using TopTenApp.Services;
using TopTenApp.Models;
using GalaSoft.MvvmLight.Ioc;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace TopTenApp.ViewModel
{
    public class GroupsViewModel : ViewModelBase
    {   
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private ObservableCollection<Groups> allGroups;

        public GroupsViewModel(INavigationService navigationService)
        {
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();
            this.AllGroups = new ObservableCollection<Groups>();
            GetAllGroupsAsync();
            this.navigationService = navigationService;
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
            ((App)App.Current).SelectedGroup = selected;
            this.navigationService.Navigate(ViewsType.ArticlesByGroup); 
        }

        public IEnumerable<Groups> AllGroups
        {
            get
            {
                return this.allGroups;
            }
            set
            {
                if (this.allGroups == null)
                {
                    this.allGroups = new ObservableCollection<Groups>();
                }

                this.allGroups.Clear();
                foreach (var item in value)
                {
                    this.allGroups.Add(item);
                }
            }
        }

        public async Task GetAllGroupsAsync()
        {
            IEnumerable<Groups> all = new List<Groups>();
            all = await this.dataService.GetAllGroups();
            this.AllGroups = all;
        }
    }
}
