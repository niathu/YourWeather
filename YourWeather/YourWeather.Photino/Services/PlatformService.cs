﻿using System.Diagnostics;

namespace YourWeather.Photino.Services
{
    public class PlatformService : Rcl.Services.PlatformService
    {
        public override Task OpenBrowserUrl(string url)
        {
            if (OperatingSystem.IsWindows()) 
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
            }

            if (OperatingSystem.IsLinux()) 
            {
                Process.Start("xdg-open",url);
            }

            if(OperatingSystem.IsMacCatalyst())
            {
                Process.Start("open", url);
            }

            return Task.CompletedTask;
        }

        public override string GetVersion()
        {
            return base.GetVersion();
        }
    }
}