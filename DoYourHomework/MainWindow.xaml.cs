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
using DoYourHomework.Pages;
using System.Windows.Media.Animation;
using System.Threading;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;

namespace DoYourHomework
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PaletteHelper helper = new PaletteHelper();
            var swatches = new SwatchesProvider().Swatches;
            var blueSwatch = from s in swatches where s.Name == "blue" select s;
            helper.ReplacePrimaryColor(blueSwatch.FirstOrDefault());
            helper.ReplaceAccentColor(blueSwatch.FirstOrDefault());
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RollPage(new Hint());
        }
        async void RollPage(IPage page)
        {
            if (mGrid.Children.Count != 0)
            {
                var da = new DoubleAnimation();
                da.To = 0;
                da.Duration = new Duration(TimeSpan.FromSeconds(1));
                da.Completed += (s, e) => this.Dispatcher?.Invoke(() => mGrid.Children.Remove(mGrid.Children[0]));
                mGrid.Children[0].BeginAnimation(Grid.OpacityProperty, da);
            }

            ((UserControl)page).Opacity = 0;
            mGrid.Children.Add((UserControl)page);

            var da_ = new DoubleAnimation();
            da_.To = 1;
            da_.Duration = new Duration(TimeSpan.FromSeconds(1));

            page.OnCompleted += Page_OnCompleted;

            ((UserControl)page).BeginAnimation(Grid.OpacityProperty, da_);
            await Task.Run(() => Thread.Sleep(500));
            page.OnLoadCompleted();
        }

        private void Page_OnCompleted(object sender, EventArgs e)
        {
            switch (((IPage)sender).PageIndex)
            {
                case 0:
                    this.Dispatcher.Invoke(() => RollPage((IPage)new Settings()));
                    break;
                case 1:
                    this.Dispatcher.Invoke(() => RollPage((IPage)new Preparing()));
                    break;
                case 2:
                    this.Close();
                    break;
            }
        }
    }
}
