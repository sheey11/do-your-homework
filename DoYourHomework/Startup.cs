using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Xml;
using System.IO;

namespace DoYourHomework
{
    class Startup
    {
        static public int HomeworkTime { get; set; }
        static public int RestTime { get; set; }
        static public bool AllowPunish { get; set; }
        static public bool[] EnableDays { get; private set; }
        static public bool AllowOvertime { get; set; }

        static internal Countdown countdown = new Countdown();

        [STAThread]
        public static void Main(string[] args)
        {
            EnableDays = new bool[7];
            Application app = new Application();
            app.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            #region Resource
            app.Resources.MergedDictionaries.Add(
                new ResourceDictionary(){
                    Source = new Uri(@"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml"),
                });
            app.Resources.MergedDictionaries.Add(
                new ResourceDictionary()
                {
                    Source = new Uri(@"pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml"),
                });
            app.Resources.MergedDictionaries.Add(
                new ResourceDictionary()
                {
                    Source = new Uri(@"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.DeepPurple.xaml"),
                });
            app.Resources.MergedDictionaries.Add(
                new ResourceDictionary()
                {
                    Source = new Uri(@"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml"),
                });
            #endregion
            
            var i = new InfomationWindow();
            app.MainWindow = i;

            if (File.Exists("config.xml"))
            {
                var reader = XmlReader.Create(new StreamReader("config.xml"));
                while (reader.Read())
                {
                    switch (reader.LocalName)
                    {
                        case "HomeworkTime":
                            HomeworkTime = reader.ReadElementContentAsInt();
                            break;
                        case "RestTime":
                            RestTime = reader.ReadElementContentAsInt();
                            break;
                        case "AllowPunish":
                            AllowPunish = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "AllowOvertime":
                            AllowOvertime = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "Monday":
                            EnableDays[0] = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "Tuesday":
                            EnableDays[1] = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "Wednesday":
                            EnableDays[2] = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "Thursday":
                            EnableDays[3] = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "Friday":
                            EnableDays[4] = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "Saturday":
                            EnableDays[5] = StringToBool(reader.ReadElementContentAsString());
                            break;
                        case "Sunday":
                            EnableDays[6] = StringToBool(reader.ReadElementContentAsString());
                            break;
                    }
                }
                countdown.SetTime(Convert.ToUInt32(RestTime) * 60);
                countdown.OnTimeChange += i.Countdown_OnTimeChange;
                countdown.Run();
            } else
            {
                new MainWindow().ShowDialog();
            }

            //new HomeworkTime().Show();

            i.Top = 30;
            i.Left = -i.Width;
            app.Run(i);
        }
        static bool StringToBool(string str)
        {
            str = str.ToLower();
            return str == "true" ? true : false;
        }
    }
}
