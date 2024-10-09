using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ProcessWatchDog
{
    public class CustomTimer
    {

        private Timer timer;
        private int interval;
        private int elapsedTime = 0;
        private DateTime startTime;

        public CustomTimer(int interval, ElapsedEventHandler handler) {

            this.interval = interval;
            timer = new Timer(interval);
            timer.Elapsed += handler;
            timer.Start();

        }

        public void Start()
        {
            startTime = DateTime.Now;
            timer.Start();
            Console.WriteLine("Timer start");
        }

        public void Pause()
        {
            elapsedTime += (int)(DateTime.Now - startTime).TotalMilliseconds;
            timer.Stop();
            Console.WriteLine("Timer paused");
        }

        public void Resume() 
        {
            int remainingTime = interval - elapsedTime;
            timer.Interval = remainingTime;
            elapsedTime = 0;
            startTime = DateTime.Now;
            timer.Start();
            Console.WriteLine($"Timer resumed with {remainingTime}ms remaining.");
        }

        public void Stop()
        {
            timer.Stop();
        }


    }
}
