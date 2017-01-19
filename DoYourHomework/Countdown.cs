using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DoYourHomework
{
    class Countdown
    {
        private uint _Seconds { get; set; }
        public DateTime RestTime
        {
            get
            {
                return new DateTime(1, 1, 1, RestSecond, RestMinute, RestHour);
            }
        }
        public int RestHour { get { return Convert.ToInt32(_Seconds / 3600); } }
        public int RestMinute { get { return Convert.ToInt32((_Seconds / 60) % 60); } }
        public int RestSecond { get { return Convert.ToInt32(_Seconds % 60); } }

        private DispatcherTimer Timer = new DispatcherTimer();

        public event EventHandler OnArrive;
        public event EventHandler OnTimeChange;

        public Countdown()
        {
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_Seconds-- == 0) {
                _Seconds = 0;
                OnArrive?.Invoke(this, new EventArgs());
                Timer.IsEnabled = false;
            } else
            {
                OnTimeChange?.Invoke(this,new EventArgs());
            }
        }

        public override string ToString()
        {
            return String.Format("{0:D2}:{1:D2}:{2:D2}", new object[] { RestHour, RestMinute, RestSecond });
        }
        public string ToStringWithoutSecond()
        {
            return String.Format("{0:D2}:{1:D2}", new object[] { RestHour, RestMinute });
        }

        public void SetTime(uint second)
        {
            _Seconds = second;
        }
        public void Run()
        {
            Timer.IsEnabled = true;
        }
        public void Pause()
        {
            Timer.IsEnabled = false;
        }
    }
}
