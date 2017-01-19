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
using System.Windows.Controls.Primitives;
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Animation;
using System.Xml;

namespace DoYourHomework.Pages
{
    /// <summary>
    /// Settings.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : UserControl, IPage
    {
        List<ToggleButton> days = new List<ToggleButton>();
        public Settings()
        {
            InitializeComponent();

            days.Add(Monday);
            days.Add(Tuesday);
            days.Add(Wednesday);
            days.Add(Thursday);
            days.Add(Friday);
            days.Add(Saturday);
            days.Add(Sunday);
        }

        public int PageIndex
        {
            get
            {
                return 1;
            }
        }

        public event EventHandler OnCompleted;

        public void OnLoadCompleted()
        {
            
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            var selectedDays = from day in days where day.IsChecked.Value select day;
            if (selectedDays.ToList().Count == 0)
            {
                ShowInfomation("请至少选中一天。");
                return;
            }
            ClearInfomation();
            var writer = XmlWriter.Create("config.xml");
            writer.WriteStartElement("DoYourHomework");
            writer.WriteString("\n");
            writer.WriteElementString("RestTime", RestTime.Value.ToString());
            writer.WriteString("\n");
            writer.WriteElementString("HomeworkTime", HomeworkTime.Value.ToString());
            writer.WriteString("\n");
            writer.WriteElementString("AllowPunish", AllowPunish.IsChecked.Value ? "True" : "False");
            writer.WriteString("\n");
            writer.WriteElementString("AllowOvertime", AllowOvertime.IsChecked.Value ? "True" : "False");
            writer.WriteString("\n");
            foreach (ToggleButton day in days)
            {
                writer.WriteElementString(day.Name, day.IsChecked.Value ? "True" : "False");
                writer.WriteString("\n");
            }
            writer.Flush();
            writer.Dispose();

            Startup.HomeworkTime = Convert.ToInt32(HomeworkTime.Value);
            Startup.RestTime = Convert.ToInt32(RestTime.Value);
            Startup.AllowOvertime = AllowOvertime.IsChecked.Value;
            Startup.AllowPunish = AllowPunish.IsChecked.Value;
            for(int i = 0; i < 7; i++)
            {
                Startup.EnableDays[i] = days[i].IsChecked.Value;
            }

            OnCompleted?.Invoke(this, new EventArgs());
        }

        void ClearInfomation()
        {
            if (Infomation.Margin.Left == 238) return;
            var animate = new ThicknessAnimation()
            {
                To = new Thickness(238, 0, 0, 0),
                AccelerationRatio = 1,
                DecelerationRatio = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
            };
            Infomation.BeginAnimation(Grid.MarginProperty, animate);
        }

        void ShowInfomation(string info)
        {
            if(Infomation.Margin.Left != 0)
            {
                InfoMsg.Text = info;
                var animate = new ThicknessAnimation()
                {
                    To = new Thickness(0),
                    AccelerationRatio = 0,
                    DecelerationRatio = 1,
                    Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                };
                Infomation.BeginAnimation(Grid.MarginProperty, animate);
            }else
            {
                var outAnimate = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.15)));
                outAnimate.Completed += (s, e) =>
                {
                    InfoMsg.Text = info;
                    var inAnimate = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.15)));
                    InfoMsg.BeginAnimation(Grid.OpacityProperty, inAnimate);
                };
                InfoMsg.BeginAnimation(Grid.OpacityProperty, outAnimate);
            }
        }
    }
}
