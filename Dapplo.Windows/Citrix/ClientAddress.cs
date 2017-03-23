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

using System.Net.Sockets;
using System.Runtime.InteropServices;

#endregion

namespace Dapplo.Windows.Citrix
{
    /// <summary>
    ///     This structure is returned when WFQuerySessionInformation is called with WFInfoClasses.ClientAddress
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ClientAddress
    {
        private readonly int _adressFamily;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)] private readonly byte[] _address;

        /// <summary>
        ///     Address Family
        /// </summary>
        public AddressFamily AddressFamily => (AddressFamily) _adressFamily;

        /// <summary>
        ///     IP Address used
        /// </summary>
        public string IpAddress => $"{_address[2]}.{_address[3]}.{_address[4]}.{_address[5]}";
    }
}