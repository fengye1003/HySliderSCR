using Playsis.Essencial_Repos;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR;

public class LockScreenHelper
{
    [DllImport("user32.dll")]
    private static extern bool LockWorkStation();

    // ========= WinAPI =========
    [DllImport("user32.dll")]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter,
        int X, int Y, int cx, int cy, uint uFlags);

    const int SW_SHOWMAXIMIZED = 3;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;
    const uint SWP_SHOWWINDOW = 0x0040;

    static IntPtr hwnd;
    static IntPtr previewParent;

    public static bool isPreview = false;
    public static bool isConfig = false;
    public static bool isSmall = false;

    [DllImport("user32.dll")]
    static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll")]
    static extern bool SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("user32.dll")]
    static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

    struct RECT
    {
        public int left, top, right, bottom;
    }

    const int GWL_STYLE = -16;
    const int WS_VISIBLE = 0x10000000;
    const int WS_CHILD = 0x40000000;

    public static void Lock()
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        LockWorkStation();
#endif
    }

    public static void ForceTopMost()
    {
        // Unity 层面
        var r = Screen.currentResolution;
        //Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        Screen.SetResolution(r.width, r.height, true);
        hwnd = Process.GetCurrentProcess().MainWindowHandle;
        // Win32 强制 TopMost
        SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0,
            SWP_NOMOVE | SWP_NOSIZE | SWP_SHOWWINDOW);

    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void ParseArgs()
    {
        string[] args = Environment.GetCommandLineArgs();

        foreach (var arg in args)
        {
            //Log.SaveLog(arg);
            if (arg.StartsWith("/p") || arg.StartsWith("-p"))
            {
                isPreview = true;
                var r = Screen.currentResolution;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(r.width, r.height, true);
            }


            if (arg.StartsWith("/c") || arg.StartsWith("-c"))
                isConfig = true;
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(640, 380, false);

            if (arg.StartsWith("/s") || arg.StartsWith("-s"))
            {
                var r = Screen.currentResolution;
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Screen.SetResolution(r.width, r.height, true);
            }

            if (arg.StartsWith("/p"))
            {

                isSmall = true;
                for (int i = 0; i < args.Length; i++)
                {
                    if ((args[i] == "/p" || args[i] == "-p") && i + 1 < args.Length)
                    {
                        long handle = 0;
                        long.TryParse(args[i + 1], out handle);
                        previewParent = new IntPtr(handle);
                        isPreview = true;
                    }
                }
            }
        }
    }


    public static void AttachToPreviewWindow()
    {
        IntPtr hwnd = Process.GetCurrentProcess().MainWindowHandle;

        if (hwnd == IntPtr.Zero || previewParent == IntPtr.Zero)
            return;

        // 设置为子窗口
        SetParent(hwnd, previewParent);

        // 修改窗口样式（关键！否则会异常）
        SetWindowLong(hwnd, GWL_STYLE, WS_VISIBLE | WS_CHILD);

        // 适配父窗口大小
        RECT rect;
        GetClientRect(previewParent, out rect);

        int width = rect.right - rect.left;
        int height = rect.bottom - rect.top;

        MoveWindow(hwnd, 0, 0,
            rect.right - rect.left,
            rect.bottom - rect.top,
            true);

        Screen.SetResolution(width, height, false);
    }
}