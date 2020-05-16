namespace TrainsOnline.Infrastructure.CrossCutting.MachineDateTime
{
    using System;
    using System.Runtime.InteropServices;
    using TrainsOnline.Common.Interfaces;

    public class MachineInfoService : IMachineInfoService
    {
        public DateTime Now => DateTime.Now;
        public int CurrentYear => DateTime.Now.Year;

        public DateTime UtcNow => DateTime.UtcNow;

        public bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }
}
