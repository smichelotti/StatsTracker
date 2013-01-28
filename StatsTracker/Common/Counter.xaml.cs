using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace StatsTracker.Common
{
    public sealed partial class Counter : UserControl
    {
        public Counter()
        {
            this.InitializeComponent();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            var newValue = int.Parse(tbValue.Text) + 1;
            this.Count = newValue;
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            var newValue = int.Parse(tbValue.Text) - 1;
            if (newValue >= 0)
            {
                this.Count = newValue;
            }
        }

        public static DependencyProperty CountProperty = DependencyProperty.Register("Count", typeof(int), typeof(Counter), new PropertyMetadata(null));

        public int Count
        {
            get
            {
                return (int)GetValue(CountProperty);
            }
            set
            {
                SetValue(CountProperty, value);
                this.tbValue.Text = value.ToString();
            }
        }
       
    }
}
