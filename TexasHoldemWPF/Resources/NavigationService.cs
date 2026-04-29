using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TexasHoldemWPF.Resources
{
    public class NavigationService
    {
        private readonly Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame;
        }

        public void NavigateTo<T>(object dataContext = null) where T : Page
        {
            var page = Activator.CreateInstance<T>();
            if (dataContext != null)
            {
                page.DataContext = dataContext;
            }
            _mainFrame.Navigate(page);
        }

        public void NavigateTo(Page page, object dataContext = null)
        {
            if (dataContext != null)
            {
                page.DataContext = dataContext;
            }
            _mainFrame.Navigate(page);
        }
    }
}
