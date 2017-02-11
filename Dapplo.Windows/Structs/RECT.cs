﻿#region Dapplo 2016 - GNU Lesser General Public License

// Dapplo - building blocks for .NET applications
// Copyright (C) 2017 Dapplo
// 
// For more information see: http://dapplo.net/
// Dapplo repositories are hosted on GitHub: https://github.com/dapplo
// 
// This file is part of Dapplo.Windows
// 
// Dapplo.Windows is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Dapplo.Windows is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have a copy of the GNU Lesser General Public License
// along with Dapplo.Windows. If not, see <http://www.gnu.org/licenses/lgpl.txt>.

#endregion

#region Usings

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

#endregion

namespace Dapplo.Windows.Structs
{
	/// <summary>
	///     See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd162897.aspx
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	public struct RECT
	{
		private int _left;
		private int _top;
		private int _right;
		private int _bottom;

		/// <summary>
		/// Constructor from a S.W.Rect
		/// </summary>
		/// <param name="rectangle">S.W.Rect</param>
		public RECT(Rect rectangle)
			: this((int) rectangle.Left, (int) rectangle.Top, (int) rectangle.Right, (int) rectangle.Bottom)
		{
		}

		/// <summary>
		/// Constructor from a S.D.Rectangle
		/// </summary>
		/// <param name="rectangle">S.D.Rectangle</param>
		public RECT(Rectangle rectangle) : this(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom)
		{
		}

		/// <summary>
		/// Constructor from left, right, top, bottom
		/// </summary>
		/// <param name="left">int</param>
		/// <param name="top">int</param>
		/// <param name="right">int</param>
		/// <param name="bottom">int</param>
		public RECT(int left, int top, int right, int bottom)
		{
			_left = left;
			_top = top;
			_right = right;
			_bottom = bottom;
		}

		/// <summary>
		/// X value
		/// </summary>
		public int X
		{
			get { return _left; }
			set { _left = value; }
		}

		/// <summary>
		/// X location of the rectangle
		/// </summary>
		public int Y
		{
			get { return _top; }
			set { _top = value; }
		}

		/// <summary>
		/// Left value of the rectangle
		/// </summary>
		public int Left
		{
			get { return _left; }
			set { _left = value; }
		}

		/// <summary>
		/// Top of the rectangle
		/// </summary>
		public int Top
		{
			get { return _top; }
			set { _top = value; }
		}

		/// <summary>
		/// Right of the rectangle
		/// </summary>
		public int Right
		{
			get { return _right; }
			set { _right = value; }
		}

		/// <summary>
		/// Bottom of the rectangle
		/// </summary>
		public int Bottom
		{
			get { return _bottom; }
			set { _bottom = value; }
		}

		/// <summary>
		///     Heigh of the RECT
		/// </summary>
		public int Height
		{
			get { return unchecked(_bottom - _top); }
			set { _bottom = unchecked(value - _top); }
		}

		/// <summary>
		///     Width of the RECT
		/// </summary>
		public int Width
		{
			get { return unchecked(_right - _left); }
			set { _right = unchecked(value + _left); }
		}

		/// <summary>
		///     Location for this RECT
		/// </summary>
		public Point Location
		{
			get { return new Point(Left, Top); }
			set
			{
				_left = (int) value.X;
				_top = (int) value.Y;
			}
		}

		/// <summary>
		///     Size for this RECT
		/// </summary>
		public Size Size
		{
			get { return new Size(Width, Height); }
			set
			{
				_right = unchecked((int) value.Width + _left);
				_bottom = unchecked((int) value.Height + _top);
			}
		}

		/// <summary>
		///     Cast RECT to Rect
		/// </summary>
		/// <param name="rectangle">RECT</param>
		/// <returns>Rect</returns>
		public static implicit operator Rect(RECT rectangle)
		{
			return new Rect(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		/// <summary>
		///     Cast Rect to RECT
		/// </summary>
		/// <param name="rectangle">Rect</param>
		/// <returns>RECT</returns>
		public static implicit operator RECT(Rect rectangle)
		{
			return new RECT((int) rectangle.Left, (int) rectangle.Top, (int) rectangle.Right, (int) rectangle.Bottom);
		}

		/// <summary>
		///     Cast RECT to Rectangle
		/// </summary>
		/// <param name="rectangle">RECT</param>
		/// <returns>Rectangle</returns>
		public static implicit operator Rectangle(RECT rectangle)
		{
			return new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
		}

		/// <summary>
		///     Cast Rectangle to RECT
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns>RECT</returns>
		public static implicit operator RECT(Rectangle rectangle)
		{
			return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
		}

		/// <summary>
		///     Equals for RECT
		/// </summary>
		/// <param name="rectangle1">RECT</param>
		/// <param name="rectangle2">RECT</param>
		/// <returns>bool true if they are equal</returns>
		public static bool operator ==(RECT rectangle1, RECT rectangle2)
		{
			return rectangle1.Equals(rectangle2);
		}

		/// <summary>
		/// Not is operator
		/// </summary>
		/// <param name="rectangle1"></param>
		/// <param name="rectangle2"></param>
		/// <returns>bool</returns>
		public static bool operator !=(RECT rectangle1, RECT rectangle2)
		{
			return !rectangle1.Equals(rectangle2);
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "{Left: " + _left + "; " + "Top: " + _top + "; Right: " + _right + "; Bottom: " + _bottom + "}";
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		/// <summary>
		/// Equalss
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns>bool</returns>
		public bool Equals(RECT rectangle)
		{
			return rectangle.Left == _left && rectangle.Top == _top && rectangle.Right == _right && rectangle.Bottom == _bottom;
		}

		/// <summary>
		/// Checks if this RECT is empty
		/// </summary>
		/// <returns>true when empty</returns>
		public bool IsEmpty => Width * Height == 0;
		
		/// <inheritdoc />
		public override bool Equals(object Object)
		{
			if (Object is RECT)
			{
				return Equals((RECT) Object);
			}
			if (Object is Rect)
			{
				return Equals(new RECT((Rect) Object));
			}
			if (Object is Rectangle)
			{
				return Equals(new RECT((Rectangle) Object));
			}
			return false;
		}

		/// <summary>
		/// Empty RECT
		/// </summary>
		public static RECT Empty => new RECT();
	}
}