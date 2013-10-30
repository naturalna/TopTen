using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TopTenApp.Models;
using TopTenApp.Services;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.UI.Popups;

namespace TopTenApp.ViewModel
{
    public class ArticleViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IDataService dataService;
        private readonly IRoamingSettingsService roamingSettings;

        public ArticleViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.dataService = SimpleIoc.Default.GetInstance<IDataService>();
            this.roamingSettings = SimpleIoc.Default.GetInstance<IRoamingSettingsService>();
     
            GetArticle();

            this.ShowComments = new RelayCommand(() =>
            {
                this.navigationService.Navigate(ViewsType.Comments);
            });

            this.BackCommand = new RelayCommand<string>((s) =>
            {
                this.navigationService.GoBack();
            });

            this.AddToFavorite = new RelayCommand(() =>
            {
               this.roamingSettings.WriteToRoamingSettings(this.Article);
            });

            this.ShowFavorite = new RelayCommand(() =>
            {
                this.navigationService.Navigate(ViewsType.Favorite);
            });

            this.SaveRanglist = new RelayCommand(async () =>
            {
                try
                {
                    var savePicker = new Windows.Storage.Pickers.FileSavePicker();

                    var plainTextFileTypes = new List<string>(new string[] { ".txt" });

                    savePicker.FileTypeChoices.Add(
                        new KeyValuePair<string, IList<string>>("Лист", plainTextFileTypes)
                        );

                    var saveFile = await savePicker.PickSaveFileAsync();

                    if (saveFile != null)
                    {
                        StringBuilder text = new StringBuilder();
                        foreach (var item in this.Article.Ranglist)
                        {
                            text.AppendLine(item.Palce);
                        }

                        await Windows.Storage.FileIO.WriteTextAsync(saveFile, text.ToString());
                        await new Windows.UI.Popups.MessageDialog("Файлът е запазен.").ShowAsync();
                    }
                }
                catch(Exception e)
                {
                    new MessageDialog("Файлът НЕ беше създаден успешно.").ShowAsync();
                }
            });
        }

        public ICommand BackCommand { get; private set; }
        public ICommand AddToFavorite { get; private set; }
        public ICommand ShowFavorite { get; private set; }
        public ICommand SaveRanglist { get; private set; }
        public RelayCommand ShowComments { get; private set; }

        public Article Article { get; set; }

        public async Task GetArticle()
        {
            ArticlesInGroup article = ((App)App.Current).SelectedArticle[0] as ArticlesInGroup;
            string objectId = article.ObjectId;

            Article foundArticle = await this.dataService.GetArticleByObjectId(objectId);
            this.Article = foundArticle;
            this.RaisePropertyChanged("Article");
        } 
    }
}
