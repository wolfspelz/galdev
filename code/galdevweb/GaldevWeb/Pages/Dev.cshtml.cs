using Markdig.Extensions.Tables;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace GaldevWeb.Pages;

public class DevModel : GaldevPageModel
{
    public List<string> Data = new();

    public DevModel(GaldevApp app) : base(app, "Dev")
    {
    }

    public void OnGet()
    {
        foreach (var header in HttpContext.Request.Headers) {
            Data.Add("Header: " + header.Key + "=" + header.Value.ToString());
        }
        foreach (var cookie in HttpContext.Request.Cookies) {
            Data.Add("Cookie: " + cookie.Key + "=" + cookie.Value);
        }

        //Data.Add("Proc: Commandline=" + Environment.CommandLine.ToString());
        //Data.Add("Proc: CurrentDirectory=" + Environment.CurrentDirectory.ToString());
        //Data.Add("Proc: CurrentManagedThreadId=" + Environment.CurrentManagedThreadId.ToString());
        //Data.Add("Proc: CommandLineArgs=" + string.Join(" ", Environment.GetCommandLineArgs()));
        //Data.Add("Proc: Is64BitOperatingSystem=" + Environment.Is64BitOperatingSystem.ToString());
        //Data.Add("Proc: Is64BitProcess=" + Environment.Is64BitProcess.ToString());
        //Data.Add("Proc: MachineName=" + Environment.MachineName.ToString());
        //Data.Add("Proc: OSVersion=" + Environment.OSVersion.ToString());
        //Data.Add("Proc: ProcessorCount=" + Environment.ProcessorCount.ToString());
        //Data.Add("Proc: UserDomainName=" + Environment.UserDomainName.ToString());
        //Data.Add("Proc: UserName=" + Environment.UserName.ToString());
        //Data.Add("Proc: Version=" + Environment.Version.ToString());
        //Data.Add("Proc: UptimeSec=" + (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: ProcessName=" + Process.GetCurrentProcess().ProcessName);
        //Data.Add("Proc: StartTime=" + Process.GetCurrentProcess().StartTime.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: Threads=" + Process.GetCurrentProcess().Threads.Count.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: TotalProcessorTime=" + Process.GetCurrentProcess().TotalProcessorTime.TotalSeconds.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: UserProcessorTime=" + Process.GetCurrentProcess().UserProcessorTime.TotalSeconds.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: PrivilegedProcessorTime=" + Process.GetCurrentProcess().PrivilegedProcessorTime.TotalSeconds.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: NonpagedSystemMemorySize64=" + Process.GetCurrentProcess().NonpagedSystemMemorySize64.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: PagedMemorySize64=" + Process.GetCurrentProcess().PagedMemorySize64.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: PeakPagedMemorySize64=" + Process.GetCurrentProcess().PeakPagedMemorySize64.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: PeakVirtualMemorySize64=" + Process.GetCurrentProcess().PeakVirtualMemorySize64.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: PeakWorkingSet64=" + Process.GetCurrentProcess().PeakWorkingSet64.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: PrivateMemorySize64=" + Process.GetCurrentProcess().PrivateMemorySize64.ToString(CultureInfo.InvariantCulture));
        //Data.Add("Proc: VirtualMemorySize64=" + Process.GetCurrentProcess().VirtualMemorySize64.ToString(CultureInfo.InvariantCulture));

        //var env = Environment.GetEnvironmentVariables();
        //if (env != null) {
        //    foreach (var key in env.Keys) {
        //        Data.Add("Env: " + key.ToString() + "=" + env[key].ToString());
        //    }
        //}
    }
}