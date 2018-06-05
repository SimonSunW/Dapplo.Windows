﻿//  Dapplo - building blocks for desktop applications
//  Copyright (C) 2017-2018  Dapplo
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
using System.ComponentModel;
using Dapplo.Windows.Kernel32;
using Dapplo.Windows.Kernel32.Enums;

namespace Dapplo.Windows.Clipboard.Internals
{
    internal static class ClipboardInfoExtensions
    {
        /// <summary>
        /// Create ClipboardNativeInfo to read
        /// </summary>
        /// <param name="clipboard">IClipboardLock</param>
        /// <param name="format">string</param>
        /// <returns>ClipboardNativeInfo</returns>
        public static ClipboardNativeInfo ReadInfo(this IClipboard clipboard, string format)
        {
            clipboard.ThrowWhenNoAccess();

            var formatId = ClipboardNative.MapFormatToId(format);
            var hGlobal = NativeMethods.GetClipboardData(formatId);
            var memoryPtr = Kernel32Api.GlobalLock(hGlobal);
            if (memoryPtr == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return new ClipboardNativeInfo
            {
                GlobalHandle = hGlobal,
                MemoryPtr = memoryPtr,
                FormatId = formatId
            };
        }

        /// <summary>
        /// Factory for the write information
        /// </summary>
        /// <param name="clipboard">IClipboardLock</param>
        /// <param name="format">string</param>
        /// <param name="size">int with the size of the clipboard area</param>
        /// <returns>ClipboardNativeInfo</returns>
        public static ClipboardNativeInfo WriteInfo(this IClipboard clipboard, string format, long size)
        {
            clipboard.ThrowWhenNoAccess();

            var formatId = ClipboardNative.MapFormatToId(format);

            var hGlobal = Kernel32Api.GlobalAlloc(GlobalMemorySettings.ZeroInit | GlobalMemorySettings.Movable, new UIntPtr((ulong)size));
            if (hGlobal == IntPtr.Zero)
            {
                throw new Win32Exception();
            }
            var memoryPtr = Kernel32Api.GlobalLock(hGlobal);
            if (memoryPtr == IntPtr.Zero)
            {
                throw new Win32Exception();
            }

            return new ClipboardNativeInfo
            {
                GlobalHandle = hGlobal,
                MemoryPtr = memoryPtr,
                NeedsWrite = true,
                FormatId = formatId
            };
        }
    }
}
