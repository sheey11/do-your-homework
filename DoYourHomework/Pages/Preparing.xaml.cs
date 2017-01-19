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
using System.Threading;

namespace DoYourHomework.Pages
{
    /// <summary>
    /// Preparing.xaml 的交互逻辑
    /// </summary>
    public partial class Preparing : UserControl, IPage
    {
        public Preparing()
        {
            InitializeComponent();
        }

        public int PageIndex
        {
            get
            {
                return 2;
            }
        }

        public event EventHandler OnCompleted;

        public void OnLoadCompleted()
        {
            Startup.countdown.SetTime(Convert.ToUInt32(Startup.RestTime) * 60);
            Startup.countdown.OnTimeChange += ((InfomationWindow)Application.Current.MainWindow).Countdown_OnTimeChange;
            Startup.countdown.Run();

            DoubleAnimation outAnimate = new DoubleAnimation();
            outAnimate.To = 0;
            outAnimate.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            outAnimate.Completed += (s,e) =>
            {
                Panel.Children.Remove(Progress);
                Info.Text = "已开始计时";
                Info.FontSize = 50;
                DoubleAnimation inAnimate = new DoubleAnimation();
                inAnimate.To = 1;
                inAnimate.Duration = new Duration(TimeSpan.FromSeconds(0.3));
                inAnimate.Completed += (s_, e_) => {
                    Thread.Sleep(1000);
                    OnCompleted?.Invoke(this, new EventArgs());
                    };

                Panel.BeginAnimation(Grid.OpacityProperty, inAnimate);
            };

            Panel.BeginAnimation(Grid.OpacityProperty, outAnimate);
        }
    }
}
