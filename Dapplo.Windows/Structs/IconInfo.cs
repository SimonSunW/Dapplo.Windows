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

#region using

using System;
using System.Runtime.InteropServices;

#endregion

namespace Dapplo.Windows.Structs
{
    /// <summary>
    ///     See
    ///     <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ms648052(v=vs.85).aspx">ICONINFO structure</a>
    ///     Contains information about an icon or a cursor.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct IconInfo
    {
        private int _xHotspot;
        private int _yHotspot;

        /// <summary>
        ///     Specifies whether this structure defines an icon or a cursor.
        ///     A value of TRUE specifies an icon; FALSE specifies a cursor.
        /// </summary>
        public bool IsIcon { get; set; }

        /// <summary>
        ///     The x and y coordinates of a cursor's hot spot.
        ///     If this structure defines an icon, the hot spot is always in the center of the icon,
        ///     and this member is ignored.
        /// </summary>
        public POINT Hotspot
        {
            get { return new POINT(_xHotspot, _yHotspot); }
            set
            {
                _xHotspot = value.X;
                _yHotspot = value.Y;
            }
        }


        /// <summary>
        ///     The icon bitmask bitmap.
        ///     If this structure defines a black and white icon, this bitmask is formatted so that the upper half is the icon AND
        ///     bitmask and the lower half is the icon XOR bitmask.
        ///     Under this condition, the height should be an even multiple of two.
        ///     If this structure defines a color icon, this mask only defines the AND bitmask of the icon.
        /// </summary>
        public IntPtr BitmaskBitmapHandle { get; }

        /// <summary>
        ///     A handle to the icon color bitmap.
        ///     This member can be optional if this structure defines a black and white icon.
        ///     The AND bitmask of hbmMask is applied with the SRCAND flag to the destination;
        ///     subsequently, the color bitmap is applied (using XOR) to the destination by using the SRCINVERT flag.
        /// </summary>
        public IntPtr ColorBitmapHandle { get; }
    }
}