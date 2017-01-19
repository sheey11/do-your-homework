using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DoYourHomework
{
    public class Watch
    {
        private DispatcherTimer Timer = new DispatcherTimer();
        private uint seconds = 0;

        public uint TotalSeconds { get { return seconds; } }
        public uint RestHour { get { return Convert.ToUInt32(seconds / 3600); } }
        public uint RestMinute { get { return Convert.ToUInt32((seconds / 60) % 60); } }
        public uint RestSecond { get { return Convert.ToUInt32(seconds % 60); } }

        public event EventHandler PerSeconds;

        public Watch()
        {
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            seconds++;
            PerSeconds?.Invoke(this, new EventArgs());
        }

        public void Clear()
        {
            seconds = 0;
        }
        public void Start()
        {
            Timer.Start();
        }
        public void Pause()
        {
            Timer.Stop();
        }
        public void Stop()
        {
            Timer.Stop();
            seconds = 0;
        }
        public override string ToString()
        {
            return String.Format("{0:D2}:{1:D2}:{2:D2}", new object[] { RestHour, RestMinute, RestSecond });
        }
    }
}
