using Scheduler.ScheduleType;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler
{
    public class ScheduleManager
    {
        bool _running = true;
        bool _isStillRunning = false;
        int _sleepFor = 60000;
        object _runningLock = new object();
        object _isStillRunningLock = new object();

        List<ISchedule> _schedules = new List<ISchedule>();

        public bool IsStillRunning { get { lock(_isStillRunningLock) return _isStillRunning; } set { lock (_isStillRunningLock) _isStillRunning = value; } }
        bool Running { get { lock (_runningLock) return _running; } set { lock (_runningLock) _running = value; } }

        public ScheduleManager(params ISchedule[] schedules)
        {
            _schedules.AddRange(schedules);
        }

        public void Begin()
        {
            Task.Run(() =>
            {
                IsStillRunning = true;
                while (Running)
                {
                    DateTime now = DateTime.Now;
                    TimeSpan next = now.TimeOfDay.Add(TimeSpan.FromMinutes(1));
                    _schedules.ForEach(sch =>
                    {
                        if (sch.CanRun(now)) sch.Run(now);
                    });
                    double timeToWait = (next - DateTime.Now.TimeOfDay).TotalMilliseconds;
                    if (timeToWait > 0) Thread.Sleep((int)timeToWait);
                }
                IsStillRunning = false;
            });
        }

        public void Stop()
        {
            Running = false;
            while (IsStillRunning) Thread.Sleep(15);
        }
    }
}
