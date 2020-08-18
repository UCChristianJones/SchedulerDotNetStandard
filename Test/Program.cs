using Scheduler;
using Scheduler.ScheduleType;
using System;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            ScheduleManager scheduler = new ScheduleManager(
                new Minute(TestAction, TimeSpan.FromHours(22), TimeSpan.FromHours(24), 1, DayOfWeek.Thursday));

            scheduler.Begin();

            Console.Read();
        }

        static void TestAction()
        {
            Console.WriteLine(DateTime.Now);
        }
    }
}
