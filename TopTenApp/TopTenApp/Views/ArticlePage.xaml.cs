using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TopTenApp.Models;
using TopTenApp.ViewModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TopTenApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ArticlePage : TopTenApp.Common.LayoutAwarePage
    {

        PrintDocument document = null;//tuk si dobawqme kontenta
        IPrintDocumentSource source = null;
        List<UIElement> pages = null;//tuk se zapazwat indiwidualnite stranici na documenta ni 
        FrameworkElement page1;//tuwa pazi samo na4alnata ni stranica
        protected event EventHandler pagesCreated;// pokazwa dali wsy6tnost imame nqkykwi syzdadeni stranici
        protected const double left = 0.075;//margin-left
        protected const double top = 0.03;
        private PrintManager manager;
        private ArticleViewModel context;

        public ArticlePage()
        {
            this.InitializeComponent();
            // here we must disable NavigationCacheMode because there the content is different for each 
            //group
            this.NavigationCacheMode =
                 Windows.UI.Xaml.Navigation.NavigationCacheMode.Disabled;
            this.context = this.DataContext as ArticleViewModel;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //printer
            document = new PrintDocument();
            source = document.DocumentSource;

            document.Paginate += printDocument_Paginate;
            document.GetPreviewPage += printDocument_GetPreviewPage;
            document.AddPages += printDocument_AddPages;

            //za wsqka stranica ot koqto iska6 da printira6 pi6e6 menigyr za printiraneto 
            manager = PrintManager.GetForCurrentView();
            manager.PrintTaskRequested += manager_PrintTaskRequested;


            pages = new List<UIElement>();

            PrepareContent();
            
        }

        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            if (pageState != null)
            {
                if (pageState.ContainsKey("selectedArticleName")
                    && pageState.ContainsKey("selectedArticleImage") 
                    && pageState.ContainsKey("selectedArticleObjectId"))
                {
                   var selectedArticleName = pageState["selectedArticleName"].ToString();
                   var selectedGroupImage = pageState["selectedArticleImage"].ToString();
                   var selectedArticleObjectId = pageState["selectedArticleObjectId"].ToString();

                   ArticlesInGroup article = new ArticlesInGroup();

                   article.Name = selectedArticleName;
                   article.Image = selectedGroupImage;
                   article.ObjectId = selectedArticleObjectId;


                    ((App)App.Current).SelectedArticle = new List<object>() { article };
                    var context = this.DataContext as ArticleViewModel;
                    await context.GetArticle();
                }
            }

            DataTransferManager.GetForCurrentView().DataRequested += OnDataRequested;
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
                if (((App)App.Current).SelectedArticle != null)
                {
                    ArticlesInGroup article = ((App)App.Current).SelectedArticle[0] as ArticlesInGroup;

                    pageState["selectedArticleName"] = article.Name.ToString();
                    pageState["selectedArticleImage"] = article.Image.ToString();
                    pageState["selectedArticleObjectId"] = article.ObjectId.ToString();

                }
            }

            DataTransferManager.GetForCurrentView().DataRequested -= OnDataRequested;
        }

        private void manager_PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs args)
        {
            PrintTask task = null;
            task = args.Request.CreatePrintTask("Print Job", sourceRequested =>
            {
                sourceRequested.SetSource(this.source);
            });
        }

        private void PrepareContent()
        {
            if (page1 == null)
            {
                page1 = new PageForPrinting();
            }

            //this xaml canvas 
            PrintContainer.Children.Add(page1);

            PrintContainer.InvalidateMeasure();

            PrintContainer.UpdateLayout();

        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            manager.PrintTaskRequested -= manager_PrintTaskRequested;
        }

        void printDocument_Paginate(object sender, PaginateEventArgs e)
        {
            pages.Clear();
            PrintContainer.Children.Clear();

            RichTextBlockOverflow lastRTBOOnPage;
            PrintTaskOptions printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);
            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            lastRTBOOnPage = AddOnePrintPreviewPage(null, pageDescription);

            while (lastRTBOOnPage.HasOverflowContent && lastRTBOOnPage.Visibility == Windows.UI.Xaml.Visibility.Visible)
            {
                lastRTBOOnPage = AddOnePrintPreviewPage(lastRTBOOnPage, pageDescription);
            }

            if (pagesCreated != null)
            {
                pagesCreated.Invoke(pages, null);
            }

            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPageCount(pages.Count, PreviewPageCountType.Intermediate);
        }

        //This method does all of the heavy lifting for us, calculating the size of the page,
        //setting margins, and saving them to the pages List<>.
        private RichTextBlockOverflow AddOnePrintPreviewPage(RichTextBlockOverflow lastRTBOAdded, PrintPageDescription printPageDescription)
        {
            //This method does all of the heavy lifting for us, calculating the size of the 
            //page, setting margins, and saving them to the pages List<>.
            FrameworkElement page;
            RichTextBlockOverflow link;

            if (lastRTBOAdded == null)
            {
                page = page1;
            }
            else
            {
                page = new ContinuesPage(lastRTBOAdded);
            }

            page.Width = printPageDescription.PageSize.Width;
            page.Height = printPageDescription.PageSize.Height;

            Grid printableArea = (Grid)page.FindName("printableArea");

            double marginWidth = Math.Max(printPageDescription.PageSize.Width - printPageDescription.ImageableRect.Width, printPageDescription.PageSize.Width * left * 2);
            double marginHeight = Math.Max(printPageDescription.PageSize.Height - printPageDescription.ImageableRect.Height, printPageDescription.PageSize.Height * top * 2);

            printableArea.Width = page1.Width - marginWidth;
            printableArea.Height = page1.Height - marginHeight;

            PrintContainer.Children.Add(page);
            PrintContainer.InvalidateMeasure();
            PrintContainer.UpdateLayout();

            // Find the last text container and see if the content is overflowing
            link = (RichTextBlockOverflow)page.FindName("continuationPageLinkedContainer");

            // Add the page to the page preview collection
            pages.Add(page);

            return link;
        }

        void printDocument_AddPages(object sender, AddPagesEventArgs e)
        {
            for (int i = 0; i < pages.Count; i++)
            {
                document.AddPage(pages[i]);
            }

            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.AddPagesComplete();
        }

        void printDocument_GetPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, pages[e.PageNumber - 1]);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                var request = args.Request;
                request.Data.Properties.Title = this.context.Article.Title;
                request.Data.Properties.Description = this.context.Article.Content;

                var recipe = this.context.Article.Content;
                if (recipe != null && recipe != "")
                {
                    request.Data.SetText(recipe);
                }               
            }
            catch(Exception e)
            {
               // new MessageDialog("Error. Place try again later").ShowAsync();
            }
        }
    }
}
