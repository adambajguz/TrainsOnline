namespace TrainsOnline.Infrastructure.MachineDateTime
{
    using System;
    using TrainsOnline.Common.Interfaces;

    public class MachineDateTimeService : IMachineDateTimeService
    {
        public DateTime Now => DateTime.Now;

        public int CurrentYear => DateTime.Now.Year;
    }
}
