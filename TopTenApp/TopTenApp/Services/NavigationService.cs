using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTenApp.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TopTenApp.Services
{
    public class NavigationService : INavigationService
    {
        //towa prewkliu4wa mejdu stranicite
        //towa kazwa kakyw tip e podadenta enumeraciq
        private Type GetViewType(ViewsType view)
        {
            switch (view)
            {
                case ViewsType.Login:
                    return typeof(LoginPage);
                case ViewsType.Groups:
                    return typeof(GroupsPage);
                case ViewsType.ArticlesByGroup:
                    return typeof(ArticlesByGroupPage);
                case ViewsType.Article:
                    return typeof(ArticlePage);
                case ViewsType.Comments:
                    return typeof(CommentsPage);
                case ViewsType.Register:
                    return typeof(RegisterPage);
                case ViewsType.Favorite:
                    return typeof(FavoriteSelectionPage);
                case ViewsType.Snap:
                    return typeof(SnapPage);
                case ViewsType.Search:
                    return typeof(SearchResultPage);
                default:
                    break;
            }

            return null;
        }

        public void Navigate(ViewsType sourcePageType)
        {
            var pageType = this.GetViewType(sourcePageType);

            if (pageType != null)
            {
                ((Frame)Window.Current.Content).Navigate(pageType);
            }
        }

        public void Navigate(ViewsType sourcePageType, object parameter)
        {
            var pageType = this.GetViewType(sourcePageType);

            if (pageType != null)
            {
                ((Frame)Window.Current.Content).Navigate(pageType, parameter);
            }
        }

        public void GoBack()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}
