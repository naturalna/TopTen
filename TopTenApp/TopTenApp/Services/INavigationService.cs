using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTenApp.Services
{
    public interface INavigationService
    {
        void Navigate(ViewsType view);
        void Navigate(ViewsType view, object parameter);
        void GoBack();
    }
}
