using StatsTracker.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StatsTracker
{
    public class ItemsTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GameTemplate { get; set; }
        public DataTemplate PlayerTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is Game)
            {
                return this.GameTemplate;
            }
            else if (item is Player)
            {
                return this.PlayerTemplate;
            }
            else
            {
                throw new InvalidOperationException("Unknown object type - cannot select data template.");
            }
        }
    }
}
