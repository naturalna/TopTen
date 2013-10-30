using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TopTenApp.Models;
using TopTenApp.Services;
using Windows.UI.Popups;

namespace TopTenApp.ViewModel
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;

        public RegisterViewModel(INavigationService navigationService)
        {
            this.User = new User();
            this.navigationService = navigationService;
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();

            this.BackCommand = new RelayCommand<string>((s) =>
            {
                this.navigationService.GoBack();
            });

            this.RegisterCommand = new RelayCommand(async() =>
            {
                if (this.User.Password == this.User.RepeatedPassword)
                {
                    await dataService.CreateUser(this.User.Username, this.User.Password, this.User.Email);

                    //if registration is faild
                    if (((App)App.Current).AuthenticatedUser != null && ((App)App.Current).AuthenticatedUser.IsAuthenticated)
                    {
                        this.navigationService.Navigate(ViewsType.Groups);
                    }
                }
                else
                {
                    new MessageDialog("Двете пароли не съвпадат").ShowAsync();
                }
            });
        }

        public RelayCommand RegisterCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        public User User { get; set; }
    }
}
