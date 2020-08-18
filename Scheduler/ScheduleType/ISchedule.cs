using System;
using System.Threading.Tasks;

namespace Scheduler.ScheduleType
{
    public interface ISchedule
    {
        DateTime LastRunTime { get; }
        bool CanRun(DateTime now);
        void Run(DateTime now);
        Action RunAction { get; }
    }
}