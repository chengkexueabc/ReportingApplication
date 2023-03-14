using System.ComponentModel;
using System;

namespace ReportingApplication
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression)
        {
            this.JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
            CronExpression = cronExpression ?? throw new ArgumentNullException(nameof(cronExpression));
        }

        public Type JobType { get; private set; }

        public string CronExpression { get; private set; }

        public JobStatus JobStatu { get; set; } = JobStatus.Init;
    }

    public enum JobStatus : byte
    {
        [Description("Init")]
        Init = 0,
        [Description("Running")]
        Running = 1,
        [Description("Scheduling")]
        Scheduling = 2,
        [Description("Stopped")]
        Stopped = 3,

    }
}
