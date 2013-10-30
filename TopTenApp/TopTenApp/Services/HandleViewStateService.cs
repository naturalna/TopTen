using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace TopTenApp.Services
{
    public class HandleViewStateService
    {
        private static bool isPreviousSnapped;
        private static INavigationService navigator;

        static HandleViewStateService()
        {
            navigator = new NavigationService();
        }

        static public void HandleWindowSizeChanged(Size size)
        {
            if (size.Width == 320)
            {
                isPreviousSnapped = true;
                navigator.Navigate(ViewsType.Snap);
            }
            else
            {
                if (isPreviousSnapped)
                {
                    navigator.GoBack();
                }
                isPreviousSnapped = false;
            }
        }
    }
}
