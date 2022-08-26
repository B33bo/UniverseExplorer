using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEditor;

namespace Btools.windowTransparency
{
    /// <summary>Makes the window transparent</summary>
    /// <credit>Code Monkey</credit>
    public static class WindowTransparency
    {
        public static bool IsTransparent;
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("Dwmapi.dll")]
        private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        const int GWL_EXSTYLE = -20;

        const uint WS_EX_LAYERED = 0x00080000;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members",
            Justification = "is actually used, only in builds")]

        const uint WS_EX_TRANSPARENT = 0x00000020;

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        /// <summary>Makes the window transparent</summary>
        public static void MakeTransparent()
        {
            SetTransparency(true, true);
        }

        /// <summary>Makes the window transparent</summary>
        /// <param name="MouseIsBehind">Can you click through the window?</param>
        public static void MakeTransparent(bool MouseIsBehind)
        {
            SetTransparency(MouseIsBehind, true);
        }

        /// <summary>
        /// Make the window transparent
        /// </summary>
        /// <param name="MouseIsBehind">Can you click through the window?</param>
        /// <param name="StayOnTop">Should it stay above all other programs</param>
        public static void MakeTransparent(bool MouseIsBehind, bool StayOnTop)
        {
            SetTransparency(MouseIsBehind, StayOnTop);
        }

        [DllImport("user32.dll")]
        public static extern int SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        const uint LWA_COLORKEY = 0x000000001;

#pragma warning disable CS0162 // Unreachable code detected
        static void SetTransparency(bool MouseIsBehind, bool StayOnTop)
        {
#if UNITY_EDITOR
            if (PlayerSettings.useFlipModelSwapchain)
                Debug.LogError("Error setting window transparency: Flip Model Swapchain is activated!\n Go to" +
                    "\"File/Build settings/Player settings/Resolution and Presentation/Standalone Player Options\" and disable " +
                    "<b>Use DXGI Flip Model Swapchain for D3D11</b>");

            Debug.Log("Window transparency cannot happen inside the unity editor, please make a build to preview your changes!");
            return;
#endif

#if UNITY_WEBGL
            Debug.LogError("Cannot make window transparent in WEBGL");
            return;
#endif

            IsTransparent = true;
            Application.runInBackground = true;
            MARGINS margins = new MARGINS { cxLeftWidth = -1 };
            IntPtr hWnd = GetActiveWindow();

            DwmExtendFrameIntoClientArea(hWnd, ref margins);

            if (MouseIsBehind)
            {
                SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
                SetLayeredWindowAttributes(hWnd, 0, 0, LWA_COLORKEY);
            }

            if (StayOnTop)
            {
                SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, 0);
            }
        }
#pragma warning restore CS0162 // Unreachable code detected
    }
}
