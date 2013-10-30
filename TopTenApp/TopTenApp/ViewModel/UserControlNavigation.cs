using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TopTenApp.Services;

namespace TopTenApp.ViewModel
{
    public class UserControlNavigation : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private const int MaxFileSize = 100 * 1024;

        public UserControlNavigation(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();

            this.LogoutCommand = new RelayCommand<string>((s) =>
            {
                this.dataService.LogOut();
                this.navigationService.Navigate(ViewsType.Login);
            });

            this.ShowFavorite = new RelayCommand<string>((s) =>
            {
                this.navigationService.Navigate(ViewsType.Favorite);
            });

            this.SendArticle = new RelayCommand(async () =>
            {
                var openPicker = new Windows.Storage.Pickers.FileOpenPicker();
                openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
                openPicker.CommitButtonText = "Send";
                openPicker.ViewMode = Windows.Storage.Pickers.PickerViewMode.List;             

                openPicker.FileTypeFilter.Add(".txt");

                var file = await openPicker.PickSingleFileAsync();

                if (file != null)
                {
                    var fileSize = await file.GetBasicPropertiesAsync();

                    if (fileSize.Size <= MaxFileSize)
                    {
                        var text = await Windows.Storage.FileIO.ReadTextAsync(file);
                        await this.dataService.CreateArticle(((App)App.Current).AuthenticatedUser, text);
                        await new Windows.UI.Popups.MessageDialog("Файлът беше изпратен успешно").ShowAsync();
                    }
                    else
                    {
                        await new Windows.UI.Popups.MessageDialog("Файлът е твърде голям.Максималният размер на файла е 100KB").ShowAsync();
                    }   
                }                
            });
        }

        public ICommand ChangePasswordCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand ShowFavorite { get; private set; }
        public ICommand SendArticle { get; private set; }
    }
}
