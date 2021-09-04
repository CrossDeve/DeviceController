using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class DeviceControler
{
    [DllImport("user32")]
    public static extern void LockWorkStation();
    [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);
    [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi)]
    protected static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

    static bool IsDVDromOpen;

    /// <summary>
    /// Shutdown sytem.
    /// </summary>
    public static void ShutDown()
    {
        StartProcess(new ProcessStartInfo("shutdown", "/s /t 0"));
    }
    /// <summary>
    /// Restart system
    /// </summary>
    public static void Restart()
    {
        StartProcess(new ProcessStartInfo("shutdown", "/r /t 0"));
    }
    /// <summary>
    /// Lock windows
    /// </summary>
    public static void LockWindows()
    {
        LockWorkStation();
    }
    /// <summary>
    /// Sleep mode system
    /// </summary>
    public static void Sleep()
    {
        SetSuspendState(false, true, true);
    }
    /// <summary>
    /// Open or close DVD rom
    /// </summary>
    public static void ForceDVDRom()
    {
        IsDVDromOpen = ProcessCDTray(!IsDVDromOpen);
    }

    static void StartProcess(ProcessStartInfo PSI)
    {
        PSI.CreateNoWindow = true;
        PSI.UseShellExecute = false;
        Process.Start(PSI);
    }
    static bool ProcessCDTray(bool open)
    {
        int ret = 0;

        switch (open)
        {
            case true:
                ret = mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                return true;
            //break;
            case false:
                ret = mciSendString("set cdaudio door closed", null, 0, IntPtr.Zero);
                return false;
            //break;
            default:
                ret = mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
                return true;
                //break;
        }
    }
}