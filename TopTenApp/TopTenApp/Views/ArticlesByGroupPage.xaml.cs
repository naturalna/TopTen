using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopTenApp.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TopTenApp.Services;
using TopTenApp.ViewModel;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TopTenApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticlesByGroupPage : TopTenApp.Common.LayoutAwarePage
    {
        public ArticlesByGroupPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode =
            Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            if (pageState != null)
            {
                if (pageState.ContainsKey("selectedGroupName") && pageState.ContainsKey("selectedGroupImageURL") && pageState.ContainsKey("selectedGroupObjectId"))
                {
                  var selectedGroupName = pageState["selectedGroupName"].ToString();
                  var selectedGroupImageURL = pageState["selectedGroupImageURL"].ToString();
                  var selectedGroupObjectId = pageState["selectedGroupObjectId"].ToString();
                  Groups group = new Models.Groups();

                      group.Name = selectedGroupName;
                      group.ImageURL = selectedGroupImageURL;
                      group.ObjectId = selectedGroupObjectId;


                  ((App)App.Current).SelectedGroup = new List<object>(){ group};
                    var context = this.DataContext as ArticlesInGroupViewModel;
                    await context.GetAllArticles();
                }   
            }  
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (pageState != null)
            {
                if (((App)App.Current).SelectedGroup != null)
                {
                    Groups group = ((App)App.Current).SelectedGroup[0] as Groups;

                    // var selectedGroup = await JsonConvert.SerializeObjectAsync(group);

                    pageState["selectedGroupName"] = group.Name.ToString();
                    pageState["selectedGroupImageURL"] = group.ImageURL.ToString();
                    pageState["selectedGroupObjectId"] = group.ObjectId.ToString();
                }
            }
        }
    }
}
