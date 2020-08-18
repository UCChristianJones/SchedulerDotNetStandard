# SchedulerDotNetStandard
a cross platform action scheduler.


### How to use:
```C# 
// Instantiate class with all of the schedule you would like to run
ScheduleManager scheduler = new ScheduleManager(
    new Minute(
        ActionToRun/*run this method when it's time*/, 
        TimeSpan.FromHours(22)/*start at 10pm*/, 
        TimeSpan.FromHours(24)/*finish at midnight*/, 
        1/*repeat every one minute*/, 
        DayOfWeek.Thursday/*on a thursday*/));

// now start the schedule manager - this starts processing in a different thread so you can do other stuff after this method
scheduler.Begin();
```
