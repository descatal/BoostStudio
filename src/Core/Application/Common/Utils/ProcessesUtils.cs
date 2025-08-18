using System.Diagnostics;

namespace BoostStudio.Application.Common.Utils;

public static class ProcessesUtils
{
    public static Process? GetRpcs3Process()
    {
        if (OperatingSystem.IsWindows())
        {
            return Process.GetProcessesByName("rpcs3").FirstOrDefault();
        }
        else if (OperatingSystem.IsLinux())
        {
            // on linux, rpcs3 is ran in AppImages, and the actual process handle is wrapped in
            return Process.GetProcessesByName("AppRun.wrapped").FirstOrDefault();
        }
        else
        {
            return null;
        }
    }
}
