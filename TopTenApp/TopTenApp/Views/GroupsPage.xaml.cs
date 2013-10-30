using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopTenApp.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using TopTenApp.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TopTenApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupsPage : TopTenApp.Common.LayoutAwarePage
    {
        static bool isWindowsSizeChangedAttached = false;

        public GroupsPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode =
              Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            if (!isWindowsSizeChangedAttached)
            {
                Window.Current.SizeChanged += OnWindowSizeChanged;
                isWindowsSizeChangedAttached = true;
            }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           
            if (this.AllGroupsList.SelectedItems != null && this.AllGroupsList.SelectedItems.Count > 0)
            {
                this.AllGroupsList.SelectedItem = null;
            }
        }       

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        void OnWindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            HandleViewStateService.HandleWindowSizeChanged(e.Size);
        }
    }
}
