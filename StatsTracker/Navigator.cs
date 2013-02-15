using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StatsTracker
{
    static class Navigator
    {
        /// <summary>
        /// Simple convenience method for navigation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void NavigateTo<T>()
        {
            var pageType = typeof(T);
            ((Frame)Window.Current.Content).Navigate(pageType);
        }

        public static void NavigateBack()
        {
            var frame = (Frame)Window.Current.Content;
            if (frame.CanGoBack)
            {
                frame.GoBack();
            }
        }
    }
}
