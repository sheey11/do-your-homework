using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace DoYourHomework
{
    /// <summary>
    /// TwoSectionProgress.xaml 的交互逻辑
    /// </summary>
    public partial class TwoSectionProgress : UserControl
    {
        public double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                if(value == 0 || value == 1)
                {
                    Rect.Margin = new Thickness(0, 0, 0, this.Width);
                    if (value == 0)
                        return;
                }
                double ActualValue = value > 100 ? 100 : value;
                ActualValue = value < 0 ? 0 : value;
                ActualValue *= 0.01;
                
                var right = this.Width * (1 - 2 * ActualValue);
                // left = -right

                var animate = new ThicknessAnimation();
                animate.To = new Thickness(-right < 0 ? 0 : -right, 0, right < 0 ? 0 : right, 0);
                animate.AccelerationRatio = 0.5;
                animate.DecelerationRatio = 0.5;
                animate.Duration = new Duration(TimeSpan.FromSeconds(0.5));

                Rect.BeginAnimation(Grid.MarginProperty, animate);
            }
        }
        private double _Value;

        public TwoSectionProgress()
        {
            InitializeComponent();
        }
    }
}
