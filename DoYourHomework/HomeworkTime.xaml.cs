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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Drawing;
using System.Drawing.Imaging;

namespace DoYourHomework
{
    /// <summary>
    /// HomeworkTime.xaml 的交互逻辑
    /// </summary>
    public partial class HomeworkTime : Window
    {
        Countdown countdown = new Countdown();
        Watch overtimeWatch;
        bool IsOvertime = false;
        bool CanClose = false;
        public uint OvertimeTime { get { return overtimeWatch == null ? 0 : overtimeWatch.TotalSeconds; } }

        public HomeworkTime()
        {
            InitializeComponent();
            this.Opacity = 0;
            this.Left = this.Top = 0;
            BackgroundGrid.Background = new ImageBrush(CopyScreen());
        }
        private static BitmapSource CopyScreen()
        {
            var left = Screen.AllScreens.Min(screen => screen.Bounds.X);
            var top = Screen.AllScreens.Min(screen => screen.Bounds.Y);
            var right = Screen.AllScreens.Max(screen => screen.Bounds.X + screen.Bounds.Width);
            var bottom = Screen.AllScreens.Max(screen => screen.Bounds.Y + screen.Bounds.Height);
            var width = right - left;
            var height = bottom - top;

            using (var screenBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (var bmpGraphics = Graphics.FromImage(screenBmp))
                {
                    bmpGraphics.CopyFromScreen(left, top, 0, 0, new System.Drawing.Size(width, height));
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        screenBmp.GetHbitmap(),
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CanClose) return;
            e.Cancel = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            countdown.SetTime(Convert.ToUInt32(Startup.HomeworkTime * 60));
            countdown.OnTimeChange += Countdown_OnTimeChange;
            countdown.OnArrive += Countdown_OnArrive;

            DoubleAnimation da = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1)));
            da.Completed += (s, ea) => countdown.Run();
            this.BeginAnimation(OpacityProperty, da);
        }

        private void Countdown_OnArrive(object sender, EventArgs e)
        {
            if (IsOvertime)
            {
                Overtime();
                return;
            }
            AnimatedClose();
        }
        private void AnimatedClose()
        {
            DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(1)));
            da.Completed += (s, ea) =>
            {
                Startup.countdown.SetTime(Convert.ToUInt32(Startup.RestTime) * 60);
                Startup.countdown.Run();
                ((InfomationWindow)System.Windows.Application.Current.MainWindow).Show();
                ((InfomationWindow)System.Windows.Application.Current.MainWindow).AttentionAnimation();
                CanClose = true;
                this.Close();
            };
            this.BeginAnimation(OpacityProperty, da);
        }
        private void Overtime()
        {
            DoubleAnimation da = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1)));
            OvertimeText.BeginAnimation(OpacityProperty, da);
            overtimeWatch = new Watch();
            overtimeWatch.PerSeconds += (s, e) =>
            {
                Time.Text = overtimeWatch.ToString();
            };
            overtimeWatch.Start();
            OvertimeHint.Text = "完成";
            OvertimeButton.IsEnabled = true;
        }
        private void Countdown_OnTimeChange(object sender, EventArgs e)
        {
            Time.Text = countdown.ToString();
            if (countdown.RestHour == 0 && countdown.RestMinute == 0 && countdown.RestSecond <= 30)
            {
                if (OvertimeButton.IsEnabled) return;
                IsEnabled = true;
            }
        }

        private void Overtime_Click(object sender, RoutedEventArgs e)
        {
            if(IsOvertime)
            {
                AnimatedClose();
                return;
            }
            IsOvertime = true;
            OvertimeButton.IsEnabled = false;
            OvertimeHint.Text = "即将加时";
        }
    }
}
