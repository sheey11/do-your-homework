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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Threading;

namespace DoYourHomework
{
    /// <summary>
    /// InfomationWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InfomationWindow : Window
    {
        bool IsHided = false;

        public InfomationWindow()
        {
            InitializeComponent();
        }
        #region Window styles
        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }

        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            } else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion

        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
            IsHided = false;
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = 0;
            animation.AccelerationRatio = 0.7;
            animation.DecelerationRatio = 0.3;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            this.BeginAnimation(Window.LeftProperty, animation);
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = -this.Width + 1;
            animation.AccelerationRatio = 0.3;
            animation.DecelerationRatio = 0.7;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            animation.Completed += (s, e_) => IsHided = true;
            this.BeginAnimation(Window.LeftProperty, animation);
        }
        public void ApplyTime(string time, int seconds)
        {
            if (IsHided) return;
            Time.Text = time;
            Second.Value = (60d - Convert.ToDouble(seconds)) / 60d * 100d;
        }
        public void Countdown_OnTimeChange(object sender, EventArgs e)
        {
            ApplyTime(Startup.countdown.ToStringWithoutSecond(), Startup.countdown.RestSecond);
        }

        public void OnTimeArrived(object sender, EventArgs e)
        {
            this.Hide();
            var w = new HomeworkTime();
            w.ShowDialog();
            Startup.countdown.SetTime(Convert.ToUInt32(
                Math.Floor(w.OvertimeTime * 1.5)) +
                Convert.ToUInt32(Startup.RestTime) * 60);
            Startup.countdown.Run();
            this.Show();
            AttentionAnimation();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);

            AttentionAnimation();
        }
        public async void AttentionAnimation()
        {
            ShowAnimation();
            await Task.Run(() => Thread.Sleep(3500));
            HideAnimation();
        }
        private void ShowAnimation()
        {
            Window_MouseEnter(null, null);
        }
        private void HideAnimation()
        {
            Window_MouseLeave(null, null);
        }
    }
}
