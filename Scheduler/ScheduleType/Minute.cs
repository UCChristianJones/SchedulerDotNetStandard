using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.ScheduleType
{
    public class Minute : ISchedule
    {
        bool _isStillRunning = false;
        object _isStillRunningLock = new object();
        public bool IsStillRunning { get { lock (_isStillRunningLock) return _isStillRunning; } set { lock (_isStillRunningLock) _isStillRunning = value; } }

        public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.MaxValue;
        public int RecurEveryXMinutes { get; set; } = 1;
        public DayOfWeek[] OnTheDays { get; set; } = new DayOfWeek[] { };
        public TimeSpan StartTime { get; set; } = TimeSpan.FromHours(9);
        public TimeSpan EndTime { get; set; } = TimeSpan.FromHours(17);
        public Action RunAction { get; set; }

        public DateTime LastRunTime { get; private set; }

        public Minute(Action actionToRun)
        {
            RunAction = actionToRun;
        }
        public Minute(Action actionToRun, int recurEveryXMins, params DayOfWeek[] onTheDays) : this(actionToRun)
        {
            RecurEveryXMinutes = recurEveryXMins;
            OnTheDays = onTheDays;
        }
        public Minute(Action actionToRun, TimeSpan startTime, TimeSpan endTime, int recurEveryXMins, params DayOfWeek[] onTheDays) 
            : this(actionToRun,recurEveryXMins, onTheDays)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool CanRun(DateTime now)
        {
            if (Start > now || End < now) return false;
            if (now.TimeOfDay < StartTime || now.TimeOfDay > EndTime) return false;
            if (!OnTheDays.Contains(now.DayOfWeek)) return false;
            var nextRunTime = LastRunTime.TimeOfDay.Add(TimeSpan.FromMinutes(RecurEveryXMinutes));
            if (now.TimeOfDay < nextRunTime) return false;
            return true;
        }

        public void Run(DateTime now)
        {
            if (IsStillRunning) return;
            LastRunTime = now;
            Task.Run(() =>
            {
                IsStillRunning = true;
                try
                {
                    RunAction?.Invoke();
                }
                catch { }
                IsStillRunning = false;
            });
        }
    }
}
