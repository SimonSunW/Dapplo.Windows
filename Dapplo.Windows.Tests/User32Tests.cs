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

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dapplo.Log;
using Dapplo.Log.XUnit;
using Dapplo.Windows.Desktop;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Dapplo.Windows.Tests
{
    public class User32Tests
    {
        private static readonly LogSource Log = new LogSource();

        public User32Tests(ITestOutputHelper testOutputHelper)
        {
            LogSettings.RegisterDefaultLogger<XUnitLogger>(LogLevels.Verbose, testOutputHelper);
        }

        /// <summary>
        ///     Test GetWindow
        /// </summary>
        /// <returns></returns>
        //[Fact]
        private void TestDetectChanges()
        {
            var foundWindow = false;
            IList<IInteropWindow> initialWindows = InteropWindowQuery.GetTopWindows().Where(window => window.IsVisible()).ToList();

            while (true)
            {
                Thread.Sleep(1000);
                var newWindow = InteropWindowQuery.GetTopWindows().FirstOrDefault(window => window.IsVisible() && !initialWindows.Contains(window));
                if (newWindow != null)
                {
                    foundWindow = true;
                    Log.Debug().WriteLine("{0}", newWindow.Dump());
                    break;
                }
            }

            Assert.True(foundWindow);
        }

        /// <summary>
        ///     Test GetWindow
        /// </summary>
        /// <returns></returns>
        [Fact]
        private void TestGetTopLevelWindows()
        {
            var foundWindow = false;
            foreach (var window in InteropWindowQuery.GetTopWindows().Where(window => window.IsVisible()))
            {
                foundWindow = true;

                Log.Debug().WriteLine("{0}", window.Dump());
                break;
            }
            Assert.True(foundWindow);
        }
    }
}