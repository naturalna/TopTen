using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TopTenApp.Services;
using TopTenApp.Models;
using GalaSoft.MvvmLight.Ioc;

namespace TopTenApp.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private User user;      

        public LoginViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();
            this.CreatedUser = new User();

            this.RegisterCommand = new RelayCommand(() =>
            {
                this.navigationService.Navigate(ViewsType.Register);
            });

            this.LoginCommand = new RelayCommand(async() =>
            {
                await dataService.Login(this.CreatedUser.Username, this.CreatedUser.Password);
                if (((App)App.Current).AuthenticatedUser == null)
                {
                    ((App)App.Current).AuthenticatedUser = Parse.ParseUser.CurrentUser;
                }
                if (((App)App.Current).AuthenticatedUser != null && ((App)App.Current).AuthenticatedUser.IsAuthenticated)
                {
                    this.navigationService.Navigate(ViewsType.Groups);
                }              
            });
        }

        public RelayCommand RegisterCommand { get; private set; }
        public RelayCommand LoginCommand { get; private set; }

        public User CreatedUser
        {
            get
            {
                return this.user;
            }
            set
            {
                this.user = value;
                this.RaisePropertyChanged("CreatedUser");
            }
        }
    }
}