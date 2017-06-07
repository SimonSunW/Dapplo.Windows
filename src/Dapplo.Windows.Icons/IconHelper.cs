﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2016-2017 Dapplo
// 
//  For more information see: http://dapplo.net/
//  Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
//  This file is part of Dapplo.Windows
// 
//  Dapplo.Windows is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  Dapplo.Windows is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
// 
//  You should have a copy of the GNU Lesser General Public License
//  along with Dapplo.Windows. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using Dapplo.Windows.App;
using Dapplo.Windows.Desktop;
using Dapplo.Windows.User32;

namespace Dapplo.Windows.Icons
{
    /// <summary>
    /// Helper code for icons
    /// </summary>
    public static class IconHelper
    {
        /// <summary>
        /// Helper method to get the app logo from the mandest
        /// </summary>
        /// <typeparam name="TBitmap"></typeparam>
        /// <param name="interopWindow">IInteropWindow</param>
        /// <param name="scale">int with scale, 100 is default</param>
        /// <returns>instance of TBitmap or null if nothing found</returns>
        public static TBitmap GetAppLogo<TBitmap>(IInteropWindow interopWindow, int scale = 100) where TBitmap : class
        {
            // get folder where actual app resides
            var exePath = GetAppProcessPath(interopWindow);
            if (exePath == null)
            {
                return default(TBitmap);
            }
            var dir = Path.GetDirectoryName(exePath);
            if (!Directory.Exists(dir))
            {
                return default(TBitmap);
            }
            var manifestPath = Path.Combine(dir, "AppxManifest.xml");
            if (!File.Exists(manifestPath))
            {
                return default(TBitmap);
            }
            // this is manifest file
            string pathToLogo;
            using (var fs = File.OpenRead(manifestPath))
            {
                var manifest = XDocument.Load(fs);
                const string ns = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";
                // rude parsing - take more care here
                var propertiesNamespace = XName.Get("Properties", ns);
                var logoNamespace = XName.Get("Logo", ns);
                pathToLogo = manifest.Root?.Element(propertiesNamespace)?.Element(logoNamespace)?.Value;
            }
            var logoDirectoryName = Path.GetDirectoryName(pathToLogo);
            if (logoDirectoryName == null)
            {
                return default(TBitmap);
            }
            var logoDirectory = Path.Combine(dir, logoDirectoryName);

            var logoExtension = Path.GetExtension(pathToLogo);
            var possibleLogos = Directory.GetFiles(logoDirectory, Path.GetFileNameWithoutExtension(pathToLogo) + "*" + logoExtension);

            // Find the best matching logo, this could be one with a scale in it, or just the first
            string finalLogoPath = possibleLogos.FirstOrDefault(logoFile => logoFile.EndsWith($".scale-{scale}.{logoExtension}")) ?? possibleLogos.FirstOrDefault();

            if (!File.Exists(finalLogoPath))
            {
                return default(TBitmap);
            }
            using (var fileStream = File.OpenRead(finalLogoPath))
            {
                if (typeof(BitmapSource).IsAssignableFrom(typeof(TBitmap)))
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.StreamSource = fileStream;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    return img as TBitmap;
                }
                if (typeof(Bitmap) == typeof(TBitmap)) {
                    using (var bitmap = Image.FromStream(fileStream))
                    {
                        return bitmap.Clone() as TBitmap;
                    }
                }
            }
            return default(TBitmap);
        }

        /// <summary>
        /// Get the path for the real modern app process belonging to the window
        /// </summary>
        /// <param name="interopWindow">IInteropWindow</param>
        /// <returns></returns>
        private static string GetAppProcessPath(IInteropWindow interopWindow)
        {
            int pid;
            User32Api.GetWindowThreadProcessId(interopWindow.Handle, out pid);
            if (string.Equals(interopWindow.GetClassname(), AppQuery.AppFrameWindowClass))
            {
                pid = interopWindow.GetChildren().FirstOrDefault(window => string.Equals(AppQuery.AppWindowClass, window.GetClassname()))?.GetProcessId() ?? 0;
            }
            if (pid <= 0)
            {
                return null;
            }
            using (var process = Process.GetProcessById(pid))
            {
                return process.MainModule.FileName;
            }
        }

        /// <summary>
        ///     See: http://msdn.microsoft.com/en-us/library/windows/desktop/ms648069%28v=vs.85%29.aspx
        /// </summary>
        /// <typeparam name="TIcon"></typeparam>
        /// <param name="location">The file (EXE or DLL) to get the icon from</param>
        /// <param name="index">Index of the icon</param>
        /// <param name="useLargeIcon">true if the large icon is wanted</param>
        /// <returns>Icon</returns>
        public static TIcon ExtractAssociatedIcon<TIcon>(string location, int index, bool useLargeIcon = true) where TIcon : class
        {
            IntPtr large;
            IntPtr small;
            NativeInvokes.ExtractIconEx(location, index, out large, out small, 1);
            TIcon returnIcon = null;
            var isLarge = false;
            var isSmall = false;
            try
            {
                if (useLargeIcon && !IntPtr.Zero.Equals(large))
                {
                    returnIcon = IconHandleTo<TIcon>(large);
                    isLarge = true;
                }
                else if (!IntPtr.Zero.Equals(small))
                {
                    returnIcon = IconHandleTo<TIcon>(small);
                    isSmall = true;
                }
                else if (!IntPtr.Zero.Equals(large))
                {
                    returnIcon = IconHandleTo<TIcon>(large);
                    isLarge = true;
                }
            }
            finally
            {
                if (isLarge && !IntPtr.Zero.Equals(small))
                {
                    User32Api.DestroyIcon(small);
                }
                if (isSmall && !IntPtr.Zero.Equals(large))
                {
                    User32Api.DestroyIcon(large);
                }
            }
            return returnIcon;
        }

        /// <summary>
        ///     Get the number of icon in the file
        /// </summary>
        /// <param name="location">Location of the EXE or DLL</param>
        /// <returns></returns>
        public static int CountAssociatedIcons(string location)
        {
            IntPtr large;
            IntPtr small;
            return NativeInvokes.ExtractIconEx(location, -1, out large, out small, 0);
        }

        /// <summary>
        /// Create a TIcon from the specified iconHandle
        /// </summary>
        /// <typeparam name="TIcon">Bitmap, Icon or BitmapSource</typeparam>
        /// <param name="iconHandle">IntPtr</param>
        /// <returns>TIcon</returns>
        public static TIcon IconHandleTo<TIcon>(IntPtr iconHandle) where TIcon : class
        {
            if (iconHandle == IntPtr.Zero)
            {
                return default(TIcon);
            }
            if (typeof(TIcon) == typeof(Icon))
            {
                return Icon.FromHandle(iconHandle) as TIcon;
            }
            if (typeof(TIcon) == typeof(Bitmap))
            {
                using (var icon = Icon.FromHandle(iconHandle))
                {
                    return icon.ToBitmap() as TIcon;
                }
            }
            if (typeof(TIcon) != typeof(BitmapSource))
            {
                return default(TIcon);
            }
            using (var icon = Icon.FromHandle(iconHandle))
            {
                return icon.ToImageSource() as TIcon;
            }
        }

        private static class NativeInvokes
        {

            /// <summary>
            ///     Get the Icon from a file
            /// </summary>
            /// <param name="sFile"></param>
            /// <param name="iIndex"></param>
            /// <param name="piLargeVersion"></param>
            /// <param name="piSmallVersion"></param>
            /// <param name="amountIcons"></param>
            /// <returns></returns>
            [DllImport("shell32", CharSet = CharSet.Unicode)]
            internal static extern int ExtractIconEx(string sFile, int iIndex, out IntPtr piLargeVersion, out IntPtr piSmallVersion, int amountIcons);

        }

    }
}