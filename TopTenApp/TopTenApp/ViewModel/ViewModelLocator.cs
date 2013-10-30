/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TopTenApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using TopTenApp.Services;

namespace TopTenApp.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        //Lokatora si ima defolten navigator
        public INavigationService DefaultNavigationService { get; private set; }

        public static ViewModelLocator Default { get; private set; }
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            Registers();
        }

        private void Registers()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<INavigationService, NavigationService>();
            SimpleIoc.Default.Register<IDataService, ExternalDataService>();
            SimpleIoc.Default.Register<IRoamingSettingsService, RoamingSettingsService>();

            SimpleIoc.Default.Register<CommentsViewModel>();
            SimpleIoc.Default.Register<ArticlesInGroupViewModel>();
            SimpleIoc.Default.Register<GroupsViewModel>();
            SimpleIoc.Default.Register<ArticleViewModel>();
            SimpleIoc.Default.Register<RegisterViewModel>();
            SimpleIoc.Default.Register<SearchDetaildViewModel>();
            SimpleIoc.Default.Register<UserControlNavigation>();
            SimpleIoc.Default.Register<FavoriteSelectionViewModel>();

            DefaultNavigationService = SimpleIoc.Default.GetInstance<INavigationService>();
        }

        public ViewModelBase SearchDetaildViewModel
        {
            get
            {
                SimpleIoc.Default.Unregister<SearchDetaildViewModel>();
                SimpleIoc.Default.Register<SearchDetaildViewModel>();
                return ServiceLocator.Current.GetInstance<SearchDetaildViewModel>();
            }
        }

        public ViewModelBase LoginViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginViewModel>();
            }
        }

        public ViewModelBase GroupsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<GroupsViewModel>();
            }
        }

        public ViewModelBase ArticlesInGroupViewModel
        {
            get
            {
                SimpleIoc.Default.Unregister<ArticlesInGroupViewModel>();
                SimpleIoc.Default.Register<ArticlesInGroupViewModel>();
                return ServiceLocator.Current.GetInstance<ArticlesInGroupViewModel>();
               
            }
        }

        public ViewModelBase ArticleViewModel
        {
            get
            {
                SimpleIoc.Default.Unregister<ArticleViewModel>();
                SimpleIoc.Default.Register<ArticleViewModel>();
                return ServiceLocator.Current.GetInstance<ArticleViewModel>();
            }
        }

        public ViewModelBase CommentsViewModel
        {
            get
            {
                SimpleIoc.Default.Unregister<CommentsViewModel>();
                SimpleIoc.Default.Register<CommentsViewModel>();
                return ServiceLocator.Current.GetInstance<CommentsViewModel>();
            }
        }

        public ViewModelBase RegisterViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterViewModel>();
            }
        }

        public ViewModelBase UserControlNavigation
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserControlNavigation>();
            }
        }
        //FavoriteSelectionViewModel

        public ViewModelBase FavoriteSelectionViewModel
        {
            get
            {
                SimpleIoc.Default.Unregister<FavoriteSelectionViewModel>();
                SimpleIoc.Default.Register<FavoriteSelectionViewModel>();
                return ServiceLocator.Current.GetInstance<FavoriteSelectionViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}