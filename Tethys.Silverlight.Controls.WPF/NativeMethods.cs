#region Header
// --------------------------------------------------------------------------
// Tethys                    Basic Services and Resources Development Library
// ==========================================================================
//
// A custom control library for WPF applications.
//
// ==========================================================================
// <copyright file="NativeMethods.cs" company="Tethys">
// Copyright  2014-2015 by T. Graf
//            All rights reserved.
//            Licensed under the Apache License, Version 2.0.
//            Unless required by applicable law or agreed to in writing, 
//            software distributed under the License is distributed on an
//            "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
//            either express or implied. 
// </copyright>
// 
// System ... Microsoft .Net Framework 4.5. 
// Tools .... Microsoft Visual Studio 2013
//
// ---------------------------------------------------------------------------
#endregion

namespace Tethys.Silverlight.Controls.WPF
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    /// <summary>
    /// Windows resize directions.
    /// </summary>
    public enum ResizeDirection
    {
        /// <summary>
        /// The left side.
        /// </summary>
        Left = 1,

        /// <summary>
        /// The right side.
        /// </summary>
        Right = 2,

        /// <summary>
        /// The top side.
        /// </summary>
        Top = 3,

        /// <summary>
        /// The top left side.
        /// </summary>
        TopLeft = 4,

        /// <summary>
        /// The top right side.
        /// </summary>
        TopRight = 5,

        /// <summary>
        /// The bottom side.
        /// </summary>
        Bottom = 6,

        /// <summary>
        /// The bottom left side.
        /// </summary>
        BottomLeft = 7,

        /// <summary>
        /// The bottom right side.
        /// </summary>
        BottomRight = 8,
    } // ResizeDirection()

    /// <summary>
    /// Native method support.
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// Win32 POINT.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable once InconsistentNaming
        private struct Point
        {
            /// <summary>
            /// x coordinate of point.
            /// </summary>
            public int x;

            /// <summary>
            /// y coordinate of point.
            /// </summary>
            public int y;

            /// <summary>
            /// Initializes a new instance of the <see cref="Point"/> struct.
            /// </summary>
            /// <param name="x">The x coordinate.</param>
            /// <param name="y">The y coordinate.</param>
            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        } // Point

        /// <summary>
        /// Win32 MINMAXINFO.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        // ReSharper disable once InconsistentNaming
        private struct MinMaxInfo
        {
            /// <summary>
            /// A reserved field.
            /// </summary>
            public Point ptReserved;

            /// <summary>
            /// The maximum size.
            /// </summary>
            public Point ptMaxSize;

            /// <summary>
            /// The maximum position.
            /// </summary>
            public Point ptMaxPosition;

            /// <summary>
            /// The minimum track size.
            /// </summary>
            public Point ptMinTrackSize;

            /// <summary>
            /// The maximum track size.
            /// </summary>
            public Point ptMaxTrackSize;
        } // MinMaxInfo

        /// <summary> 
        /// Win32 RECT. 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        private struct Rect
        {
            /// <summary>
            /// The left side coordinate.
            /// </summary>
            public int left;

            /// <summary>
            /// The top side coordinate.
            /// </summary>
            public int top;

            /// <summary>
            /// The right side coordinate.
            /// </summary>
            public int right;

            /// <summary>
            /// The bottom side coordinate.
            /// </summary>
            public int bottom;
        } // Rect

        /// <summary>
        /// Win32 MONITORINFO.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        // ReSharper disable once InconsistentNaming
        private class MonitorInfo
        {
            /// <summary>
            /// Class size.
            /// </summary>            
            public int cbSize = Marshal.SizeOf(typeof(MonitorInfo));

            /// <summary>
            /// The monitor rectangle.
            /// </summary>
            public Rect rcMonitor = new Rect();

            /// <summary>
            /// The work area rectangle.
            /// </summary>            
            public Rect rcWork = new Rect();

            /// <summary>
            /// The flags.
            /// </summary>            
            public int dwFlags = 0;
        } // MonitorInfo

        /// <summary>
        /// The WM_SYSCOMMAND message.
        /// </summary>
        public const int SysCommand = 0x112;

        /// <summary>
        /// The WM_GETMINMAXINFO message.
        /// </summary>
        public const int WmGetMinMaxInfo = 0x0024;

        /// <summary>
        /// The SC_SIZE flag.
        /// </summary>
        public const int ScSize = 0xF000;

        /// <summary>
        /// Positions the shortcut menu so that its left side is aligned 
        /// with the coordinate specified by the x parameter.
        /// </summary>
        private const uint TpmLeftAlign = 0x0000;

        /// <summary>
        /// The function returns the menu item identifier of the user's 
        /// selection in the return value.
        /// </summary>
        private const uint TpmReturnCmd = 0x0100;

        /// <summary>
        /// Enable menu item.
        /// </summary>
        private const uint MfEnabled = 0x00000000;

        /// <summary>
        /// Gray menu item.
        /// </summary>
        private const uint MfGrayed = 0x00000001;

        /// <summary>
        /// The SC_MAXIMIZE message.
        /// </summary>
        private const uint ScMaximize = 0xF030;

        /// <summary>
        /// Handler for the <c>WM_GETMINMAXINFO</c> message.
        /// </summary>
        /// <param name="handle">Handle to the window.</param>
        /// <param name="param">Additional message information.</param>
        /// <param name="minWidth">The minimum width.</param>
        /// <param name="minHeight">The minimum height.</param>
        public static void GetMinMaxInfo(IntPtr handle, IntPtr param, 
            int minWidth = 0, int minHeight = 0)
        {
            var mmi = (MinMaxInfo)Marshal.PtrToStructure(param, typeof(MinMaxInfo));

            // Adjust the maximized size and position to fit the work area of the correct monitor
            const int MonitorDefaultToNearest = 0x00000002;
            var monitor = MonitorFromWindow(handle, MonitorDefaultToNearest);

            if (monitor != IntPtr.Zero)
            {
                var monitorInfo = new MonitorInfo();
                GetMonitorInfo(monitor, monitorInfo);
                var workArea = monitorInfo.rcWork;
                var monitorArea = monitorInfo.rcMonitor;
                mmi.ptMaxPosition.x = Math.Abs(workArea.left - monitorArea.left);
                mmi.ptMaxPosition.y = Math.Abs(workArea.top - monitorArea.top);
                mmi.ptMaxSize.x = Math.Abs(workArea.right - workArea.left);
                mmi.ptMaxSize.y = Math.Abs(workArea.bottom - workArea.top);

                mmi.ptMinTrackSize.x = minWidth;
                mmi.ptMinTrackSize.y = minHeight;
            } // if

            Marshal.StructureToPtr(mmi, param, true);
        } // GetMinMaxInfo()

        /// <summary>
        /// Shows the system menu.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <param name="windowState">State of the window.</param>
        public static void ShowSystemMenu(Window window, WindowState windowState)
        {
            var helper = new WindowInteropHelper(window);
            var callingWindow = helper.Handle;
            var wMenu = GetSystemMenu(callingWindow, false);
            
            if (windowState == WindowState.Maximized)
            {
                EnableMenuItem(wMenu, ScMaximize, MfGrayed);
            }
            else
            {
                EnableMenuItem(wMenu, ScMaximize, MfEnabled);
            } // if

            var command = TrackPopupMenuEx(wMenu, TpmLeftAlign | TpmReturnCmd, 
                (int)window.Left + 4, (int)window.Top + 20, callingWindow, IntPtr.Zero);
            if (command == 0)
            {
                return;
            } // if

            PostMessage(callingWindow, SysCommand, new IntPtr(command), IntPtr.Zero);
        } // ShowSystemMenu()

        /// <summary>
        /// Resizes the window.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="direction">The direction.</param>
        public static void ResizeWindow(IntPtr handle, ResizeDirection direction)
        {
            SendMessage(handle, SysCommand, (IntPtr)(ScSize + direction), IntPtr.Zero);
        } // ResizeWindow()

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">The word parameter.</param>
        /// <param name="lParam">The long parameter.</param>
        /// <returns>The result of the message processing; it depends on the 
        /// message sent.</returns>
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Gets the monitor information.
        /// </summary>
        /// <param name="hMonitor">A handle to the display monitor of interest.</param>
        /// <param name="lpmi">A pointer to a <see cref="MonitorInfo"/> structure that 
        /// receives information about the specified display monitor.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, MonitorInfo lpmi);

        /// <summary>
        /// Retrieves a handle to the display monitor that has the largest area of 
        /// intersection with the bounding rectangle of a specified window.
        /// </summary>
        /// <param name="handle">A handle to the window of interest.</param>
        /// <param name="flags">The flags.</param>
        /// <returns>If the window intersects one or more display monitor rectangles, 
        /// the return value is an HMONITOR handle to the display monitor that has 
        /// the largest area of intersection with the window.</returns>
        [DllImport("User32", SetLastError = true)]
        private static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);

        /// <summary>
        /// Gets the system menu.
        /// </summary>
        /// <param name="hWnd">The handle.</param>
        /// <param name="bRevert">The action to be taken.</param>
        /// <returns>
        /// If the bRevert parameter is FALSE, the return value is a handle 
        /// to a copy of the window menu. If the bRevert parameter is TRUE, 
        /// the return value is NULL.</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        /// <summary>
        /// Displays a shortcut menu at the specified location and tracks the 
        /// selection of items on the shortcut menu. The shortcut menu can 
        /// appear anywhere on the screen.
        /// </summary>
        /// <param name="hmenu">The menu handle.</param>
        /// <param name="fuFlags">The flags.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="lptpm">The LPTPM.</param>
        /// <returns>
        /// If you specify TPM_RETURNCMD in the fuFlags parameter, 
        /// the return value is the menu-item identifier of the item that the 
        /// user selected. If the user cancels the menu without making a selection, 
        /// or if an error occurs, the return value is zero.
        /// If you do not specify TPM_RETURNCMD in the fuFlags parameter, the return
        /// value is nonzero if the function succeeds and zero if it fails.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        private static extern int TrackPopupMenuEx(IntPtr hmenu, uint fuFlags,
          int x, int y, IntPtr hwnd, IntPtr lptpm);

        /// <summary>
        /// Posts a message.
        /// </summary>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">The word parameter.</param>
        /// <param name="lParam">The long parameter.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern IntPtr PostMessage(IntPtr hWnd, uint msg, 
            IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Enables, disables, or grays the specified menu item. 
        /// </summary>
        /// <param name="hMenu">The menu handle.</param>
        /// <param name="enableItem">The menu item to be enabled, disabled,
        /// or grayed, as determined by the uEnable parameter. This parameter
        /// specifies an item in a menu bar, menu, or submenu.</param>
        /// <param name="uEnable">Controls the interpretation of the enableItem
        /// parameter and indicate whether the menu item is enabled, disabled, 
        /// or grayed.</param>
        /// <returns>The return value specifies the previous state of the menu 
        /// item (it is either MF_DISABLED, MF_ENABLED, or MF_GRAYED). If the 
        /// menu item does not exist, the return value is -1.</returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint enableItem,
           uint uEnable);
    } // NativeMethods
} // Tethys.Silverlight.Controls.WPF
