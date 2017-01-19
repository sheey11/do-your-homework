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
using System.Threading;
using System.Windows.Media.Animation;
using DoYourHomework;

namespace DoYourHomework.Pages
{
    /// <summary>
    /// Hint.xaml 的交互逻辑
    /// </summary>
    public partial class Hint : UserControl, IPage
    {
        int IPage.PageIndex { get { return 0; } }
        event EventHandler IPage.OnCompleted
        {
            add
            {
                _OnCompleted += value;
            }

            remove
            {
                _OnCompleted -= value;
            }
        }
        event EventHandler _OnCompleted;

        public Hint()
        {
            InitializeComponent();
        }

        public void OnLoadCompleted()
        {
            RunAnimationAsync();
        }

        async private void RunAnimationAsync()
        {
            await Task.Run(() => RunAnimation());
        }
        private void RunAnimation()
        {
            //Thread.Sleep(1000);

            //var da = new DoubleAnimation();
            //da.To = 0;
            //da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            //this.Dispatcher.Invoke(() => Text.BeginAnimation(Grid.OpacityProperty, da));

            //Thread.Sleep(500);
            //Text.Text = "那就让我来拯救你吧~(傲娇脸)";

            //da = new DoubleAnimation();
            //da.To = 1;
            //da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            //Text.Dispatcher.Invoke(() => Text.BeginAnimation(Grid.OpacityProperty, da));

            Thread.Sleep(2000);

            _OnCompleted?.Invoke(this,new EventArgs() { });
        }
    }
}
