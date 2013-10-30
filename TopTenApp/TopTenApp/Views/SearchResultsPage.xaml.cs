using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopTenApp.ViewModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Search Contract item template is documented at http://go.microsoft.com/fwlink/?LinkId=234240

namespace TopTenApp.Views
{
    /// <summary>
    /// This page displays search results when a global search is directed to this application.
    /// </summary>
    public sealed partial class SearchResultsPage : TopTenApp.Common.LayoutAwarePage
    {
        SearchViewModel currentViewModel = null;
        public SearchResultsPage()
        {
            this.InitializeComponent();
            this.currentViewModel = new SearchViewModel();

            this.DataContext = this.currentViewModel;
        }

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            var queryText = navigationParameter as String;
            this.currentViewModel.QueryText = queryText;
            await this.currentViewModel.GetAllFoundResults();
        }

        private void ResultsGridViewItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(SearchResultPage), e.ClickedItem);
        }
    }
}
