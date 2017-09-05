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

using System.Drawing;
using Dapplo.Windows.Common.Enums;
using Dapplo.Windows.Common.Structs;

#endregion

namespace Dapplo.Windows.Common.Extensions
{
    /// <summary>
    ///     Helper method for the RECT struct
    /// </summary>
    public static class NativeRectExensions
    {
        /// <summary>
        ///     Test if this RECT contains the specified NativePoint
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point">NativePoint</param>
        /// <returns>true if it contains</returns>
        public static bool Contains(this NativeRect rect, NativePoint point)
        {
            return IsBetween(point.X, rect.Left, rect.Right) && IsBetween(point.Y, rect.Top, rect.Bottom);
        }

        /// <summary>
        ///     True if small rectangle is entirely contained within the larger rectangle
        /// </summary>
        /// <param name="largerRectangle">The larger rectangle</param>
        /// <param name="smallerRectangle">The smaller rectangle</param>
        /// <returns>True if small rectangle is entirely contained within the larger rectangle, false otherwise</returns>
        public static bool Contains(this NativeRect largerRectangle, NativeRect smallerRectangle)
        {
            return
                largerRectangle.Left <= smallerRectangle.Left &&
                smallerRectangle.Right <= largerRectangle.Right &&
                largerRectangle.Top <= smallerRectangle.Top &&
                smallerRectangle.Bottom <= largerRectangle.Bottom;
        }

        /// <summary>
        ///     Check that two rectangles overlap with each other
        /// </summary>
        /// <param name="rect1">The first rectangle</param>
        /// <param name="rect2">The second rectangle</param>
        /// <returns>The rectangles overlap</returns>
        public static bool HasOverlap(this NativeRect rect1, NativeRect rect2)
        {
            if (rect1.IsAdjacent(rect2) != AdjacentTo.None)
            {
                // If it's adjacent than there is no overlap?
                return true;
            }

            var leftOfRect1InsideRect2Width = IsBetween(rect1.X, rect2.Left, rect2.Right);
            var leftOfRect2InsideRect1Width = IsBetween(rect2.X, rect1.Left, rect1.Right);
            var xOverlap = leftOfRect1InsideRect2Width || leftOfRect2InsideRect1Width;

            var topOfRect1InsideRect2Height = IsBetween(rect1.Y, rect2.Y, rect2.Y + rect2.Height);
            var topOfRect2InsideRect1Height = IsBetween(rect2.Y, rect1.Y, rect1.Y + rect1.Height);
            var yOverlap = topOfRect1InsideRect2Height || topOfRect2InsideRect1Height;

            var rectanglesIntersect = xOverlap && yOverlap && !(rect1.Contains(rect2) || rect2.Contains(rect1));

            return rectanglesIntersect;
        }

        /// <summary>
        ///     True if either rectangle is adjacent to the other rectangle
        /// </summary>
        /// <param name="rect1">The first rectangle</param>
        /// <param name="rect2">The second rectangle</param>
        /// <returns>At least one rectangle is adjacent to the other rectangle</returns>
        public static AdjacentTo IsAdjacent(this NativeRect rect1, NativeRect rect2)
        {
            if (rect1.Left.Equals(rect2.Right) && (IsBetween(rect1.Top, rect2.Top, rect2.Bottom) || IsBetween(rect2.Top, rect1.Top, rect1.Bottom)))
            {
                return AdjacentTo.Left;
            }
            if (rect1.Right.Equals(rect2.Left) && (IsBetween(rect1.Top, rect2.Top, rect2.Bottom) || IsBetween(rect2.Top, rect1.Top, rect1.Bottom)))
            {
                return AdjacentTo.Right;
            }
            if (rect1.Top.Equals(rect2.Bottom) && (IsBetween(rect1.Left, rect2.Left, rect2.Right) || IsBetween(rect2.Left, rect1.Left, rect1.Right)))
            {
                return AdjacentTo.Top;
            }
            if (rect1.Bottom.Equals(rect2.Top) && (IsBetween(rect1.Left, rect2.Left, rect2.Right) || IsBetween(rect2.Left, rect1.Left, rect1.Right)))
            {
                return AdjacentTo.Bottom;
            }
            return AdjacentTo.None;
        }

        /// <summary>
        ///     Simple helper to specify if value is inside min and max
        /// </summary>
        /// <param name="value">double to check</param>
        /// <param name="min">lowest allowed value</param>
        /// <param name="max">highest allowed value</param>
        /// <returns>bool true if the value is between</returns>
        private static bool IsBetween(int value, int min, int max)
        {
            return value >= min && value < max;
        }

        /// <summary>
        ///     Test if a RECT is docked to the left of another RECT
        /// </summary>
        /// <param name="rect1">RECT to test if it's docked</param>
        /// <param name="rect2">RECT rect to be docked to</param>
        /// <returns>bool with true if they are docked</returns>
        public static bool IsDockedToLeftOf(this NativeRect rect1, NativeRect rect2)
        {
            // Test if the right is one pixel to the left, and if top or bottom is within the rect2 height.
            return rect1.Right == rect2.Left - 1 && (IsBetween(rect1.Top, rect2.Top, rect2.Bottom) || IsBetween(rect1.Bottom, rect2.Top, rect2.Bottom));
        }

        /// <summary>
        ///     Test if a RECT is docked to the right of another RECT
        /// </summary>
        /// <param name="rect1">RECT to test if it's docked</param>
        /// <param name="rect2">RECT rect to be docked to</param>
        /// <returns>bool with true if they are docked</returns>
        public static bool IsDockedToRightOf(this NativeRect rect1, NativeRect rect2)
        {
            // Test if the right is one pixel to the left, and if top or bottom is within the rect2 height.
            return rect1.Left == rect2.Right + 1 && (IsBetween(rect1.Top, rect2.Top, rect2.Bottom) || IsBetween(rect1.Bottom, rect2.Top, rect2.Bottom));
        }

        /// <summary>
        /// Creates a new NativeRect which is the union of rect1 and rect2
        /// </summary>
        /// <param name="rect1">NativeRect</param>
        /// <param name="rect2">NativeRect</param>
        /// <returns>NativeRect which is the intersection of rect1 and rect2</returns>
        public static NativeRect Union(this NativeRect rect1, NativeRect rect2)
        {
            // TODO: Replace logic with own code
            return Rectangle.Union(rect1, rect2);
        }

        /// <summary>
        /// Creates a new NativeRect which is the intersection of rect1 and rect2
        /// </summary>
        /// <param name="rect1">NativeRect</param>
        /// <param name="rect2">NativeRect</param>
        /// <returns>NativeRect which is the intersection of rect1 and rect2</returns>
        public static NativeRect Intersect(this NativeRect rect1, NativeRect rect2)
        {
            // TODO: Replace logic with own code
            return Rectangle.Intersect(rect1, rect2);
        }

        /// <summary>
        /// Creates a new NativeRect which is rect but inflated with the specified width and height
        /// </summary>
        /// <param name="rect">NativeRect</param>
        /// <param name="width">int</param>
        /// <param name="height">int</param>
        /// <returns>NativeRect</returns>
        public static NativeRect Inflate(this NativeRect rect, int width, int height)
        {
            // TODO: Replace logic with own code
            return Rectangle.Inflate(rect, width, height);
        }

        /// <summary>
        /// Test if the current rectangle intersects with the specified.
        /// </summary>
        /// <param name="rect1">NativeRect</param>
        /// <param name="rect2">NativeRect</param>
        /// <returns>bool</returns>
        public static bool IntersectsWith(this NativeRect rect1, NativeRect rect2)
        {
            return rect2.X < rect1.X + rect1.Width &&
                   rect1.X < rect2.X + rect2.Width &&
                   rect2.Y < rect1.Y + rect1.Height &&
                   rect1.Y < rect2.Y + rect2.Height;
        }

        /// <summary>
        /// Create a new NativeRect by offsetting the specified one
        /// </summary>
        /// <param name="rect">NativeRect</param>
        /// <param name="offset">NativePoint</param>
        /// <returns>NativeRect</returns>
        public static NativeRect Offset(this NativeRect rect, NativePoint offset)
        {
            return new NativeRect(rect.Location.Offset(offset), rect.Size);
        }

        /// <summary>
        /// Create a new NativeRect by offsetting the specified one
        /// </summary>
        /// <param name="rect">NativeRect</param>
        /// <param name="offsetX">int</param>
        /// <param name="offsetY">int</param>
        /// <returns>NativeRect</returns>
        public static NativeRect Offset(this NativeRect rect, int offsetX, int offsetY)
        {
            return new NativeRect(rect.Location.Offset(offsetX, offsetY), rect.Size);
        }


        /// <summary>
        /// Create a new NativeRectFloat by offsetting the specified one
        /// </summary>
        /// <param name="rect">NativeRectFloat</param>
        /// <param name="offset">NativeRectFloat</param>
        /// <returns>NativeRectFloat</returns>
        public static NativeRectFloat Offset(this NativeRectFloat rect, NativePointFloat offset)
        {
            return new NativeRectFloat(rect.Location.Offset(offset), rect.Size);
        }

        /// <summary>
        /// Create a new NativeRectFloat by offsetting the specified one
        /// </summary>
        /// <param name="rect">NativeRectFloat</param>
        /// <param name="offsetX">float</param>
        /// <param name="offsetY">float</param>
        /// <returns>NativeRectFloat</returns>
        public static NativeRectFloat Offset(this NativeRectFloat rect, float offsetX, float offsetY)
        {
            return new NativeRectFloat(rect.Location.Offset(offsetX, offsetY), rect.Size);
        }
    }
}