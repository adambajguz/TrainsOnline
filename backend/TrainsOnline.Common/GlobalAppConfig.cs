namespace TrainsOnline.Common
{
    using System.Runtime.InteropServices;

    public static partial class GlobalAppConfig
    {
        public const bool DEV_MODE_SW = true;
        public const bool USE_COMPILATION_CONFIGURATION_TO_DETERMINE_ENVIRONMENT = true;
        public static bool DEV_MODE
        {
            get
            {
#pragma warning disable CS0162 // Unreachable code detected
                if (USE_COMPILATION_CONFIGURATION_TO_DETERMINE_ENVIRONMENT)
                {
#if DEBUG
                    return true;
#else
                    return false;
#endif
                }

                return DEV_MODE_SW;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public static string DEV_APPSETTINGS => "appsettings.Development.json";
        public static string PROD_APPSETTINGS => "appsettings.json";

        public static string AppSettingsFileName
        {
            get
            {
#pragma warning disable CS0162 // Unreachable code detected
                if (DEV_MODE)
                    return DEV_APPSETTINGS;
                return PROD_APPSETTINGS;
#pragma warning restore CS0162 // Unreachable code detected
            }
        }

        public static bool IsWindows { get; } = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static int MIN_PASSWORD_LENGTH => 8;
        public static int MIN_USERNAME_LENGTH => 3;
    }
}
