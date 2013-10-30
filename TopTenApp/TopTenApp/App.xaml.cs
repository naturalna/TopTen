using Parse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
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
using TopTenApp.Views;
using TopTenApp.Services;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Media.Animation;
using TopTenApp.Common;


// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TopTenApp
{
    sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            ParseClient.Initialize("Igww6Me5fYwp***", "nofabgvfYsdtCKTS7TZw9hyR9EKKg***");
        }

        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;

            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                //lokal settings
                SuspensionManager.RegisterFrame(rootFrame, "mainAppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    await SuspensionManager.RestoreAsync();
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {                
                INavigationService navigationService = new NavigationService();

                if (this.AuthenticatedUser == null)
                {
                    this.AuthenticatedUser = Parse.ParseUser.CurrentUser;
                }

                if (Parse.ParseUser.CurrentUser != null && Parse.ParseUser.CurrentUser.IsAuthenticated == true)
                {
                    navigationService.Navigate(ViewsType.Groups);
                }
                else if (!rootFrame.Navigate(typeof(LoginPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            Window.Current.Activate();

            // Track this application being launched
            try
            {
                await ParseAnalytics.TrackAppOpenedAsync(args);
            }
            catch(Exception)
            {

            }
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand command = new SettingsCommand("about", "About This App", (handler) =>
            {
                Popup popup = BuildSettingsItem(new SettingsPage(), 646);
                popup.IsOpen = true;
            });

            args.Request.ApplicationCommands.Add(command);
        }

        private Popup BuildSettingsItem(UserControl u, int w)
        {
            Popup p = new Popup();
            p.IsLightDismissEnabled = true;
            p.ChildTransitions = new TransitionCollection();
            p.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ?
                        EdgeTransitionLocation.Right :
                        EdgeTransitionLocation.Left
            });

            u.Width = w;
            u.Height = Window.Current.Bounds.Height;
            p.Child = u;

            p.SetValue(Canvas.LeftProperty, SettingsPane.Edge == SettingsEdgeLocation.Right ? (Window.Current.Bounds.Width - w) : 0);
            p.SetValue(Canvas.TopProperty, 0);

            return p;
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            await SuspensionManager.SaveAsync();           
            deferral.Complete();
        }

        public ParseUser AuthenticatedUser { get; set; }

        public IList<object> SelectedGroup { get; set; }

        public IList<object> SelectedArticle { get; set; }

        /// <summary>
        /// Invoked when the application is activated to display search results.
        /// </summary>
        /// <param name="args">Details about the activation request.</param>
        protected async override void OnSearchActivated(Windows.ApplicationModel.Activation.SearchActivatedEventArgs args)
        {           
            var previousContent = Window.Current.Content;
            var frame = previousContent as Frame;

            if (frame == null)
            {

                frame = new Frame();
                SuspensionManager.RegisterFrame(frame, "searchFrame");

                TopTenApp.Common.SuspensionManager.RegisterFrame(frame, "AppFrame");

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Restore the saved session state only when appropriate
                    try
                    {
                        await TopTenApp.Common.SuspensionManager.RestoreAsync();
                    }
                    catch (TopTenApp.Common.SuspensionManagerException)
                    {
                        //Something went wrong restoring state.
                        //Assume there is no state and continue
                    }
                }
            }

            frame.Navigate(typeof(SearchResultsPage), args.QueryText);
            Window.Current.Content = frame;

            // Ensure the current window is active
            Window.Current.Activate();
        }
    }
}
