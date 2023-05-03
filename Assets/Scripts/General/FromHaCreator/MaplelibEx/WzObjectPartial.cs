using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using MapleLib.WzLib;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms;
using ms.Helper;
using Texture = ms.Texture;

namespace MapleLib.WzLib
{
	public partial class WzObject : IEnumerable<WzImageProperty>
	{
		#region implicit operator

		public static implicit operator int (WzObject wzObject)
		{
			return wzObject?.GetInt () ?? 0;
		}

		public static implicit operator int? (WzObject wzObject)
		{
			return wzObject?.GetInt ();
		}

		public static implicit operator short (WzObject wzObject)
		{
			return wzObject?.GetShort () ?? 0;
		}

		public static implicit operator long (WzObject wzObject)
		{
			return wzObject?.GetLong () ?? 0;
		}

		public static implicit operator float (WzObject wzObject)
		{
			return wzObject?.GetFloat () ?? 0;
		}

		public static implicit operator float? (WzObject wzObject)
		{
			return wzObject?.GetFloat ();
		}

		public static implicit operator double (WzObject wzObject)
		{
			return wzObject?.GetDouble () ?? 0;
		}

		/*public static implicit operator string(WzObject wzObject)
		{
		    return wzObject.Name;
		}*/

		public static implicit operator ms.Point_short (WzObject wzObject)
		{
			return wzObject?.GetPoint ().ToMSPoint () ?? Point_short.zero;
			//return wzObject?.GetPoint().ToMSPoint() ?? Point_short.zero;
		}

		/*  public static implicit operator Bitmap(WzObject wzObject)
		  {
			  return wzObject?.GetBitmap();
		  }*/

		public static implicit operator byte[] (WzObject wzObject)
		{
			return wzObject.GetBytes ();
		}

		public static implicit operator ushort (WzObject wzObject)
		{
			return (ushort)(wzObject?.GetShort () ?? 0);
		}

		public static implicit operator byte (WzObject wzObject)
		{
			return (byte)(wzObject?.GetShort () ?? 0);
		}

		public static implicit operator bool (WzObject wzObject)
		{
			//return wzObject?.GetShort() == 1;
			return wzObject?.GetInt () == 1;
		}

		public static implicit operator Animation (WzObject wzObject)
		{
			if (wzObject != null)
			{
				return new Animation (wzObject);
			}
			else
			{
				return new Animation ();
			}
		}

		public static implicit operator Sprite (WzObject wzObject)
		{
			if (wzObject != null)
			{
				return new Sprite (wzObject);
			}
			else
			{
				return new Sprite ();
			}
		}

		public static implicit operator Texture (WzObject wzObject)
		{
			if (wzObject != null)
			{
				return new Texture (wzObject);
			}
			else
			{
				return new Texture ();
			}
		}

		public static implicit operator Sound (WzObject wzObject)
		{
			if (wzObject != null)
			{
				return new Sound (wzObject);
			}
			else
			{
				return new Sound ();
			}
		}

		public static implicit operator List<WzImageProperty> (WzObject wzObject)
		{
			if (wzObject is WzImageProperty imageProperty)
			{
				return imageProperty.WzProperties;
			}

			return default;
		}

		public static implicit operator WzObjectType (WzObject wzObject)
		{
			return wzObject.ObjectType;
		}

		public static implicit operator WzPropertyType (WzObject wzObject)
		{
			if (wzObject is WzImageProperty wzImageProperty)
			{
				return wzImageProperty.PropertyType;
			}

			return WzPropertyType.Null;
		}

		#endregion

		public WzObject resolve (string path)
		{
			var subPaths = path.Split ('/');
			WzObject result = this;
			for (int i = 0; i < subPaths.Length; i++)
			{
				var currentPath = subPaths[i];
				result = result[currentPath];
			}

			return result;
		}

		public bool IsTexture ()
		{
			//return GetBitmap ()?.isValid ?? false;

			return GetBitmapConsideringLink () != null;
			//return GetPngDataConsideringLink(out var pngInfo) != null;
			//return this is WzCanvasProperty;//todo why wzObject IsTexture ,texture may be WzCanvasProperty,may be subProperty, has a child WzUolProperty
		}

		private static List< WzImageProperty> emptyWzImagePropertyList = new List<WzImageProperty>();
		public IEnumerator<WzImageProperty> GetEnumerator ()
		{
			if (this is WzImage wzImage)
			{
				return wzImage.WzProperties.GetEnumerator ();
			}
			else if (this is WzImageProperty imageProperty && imageProperty.WzProperties != null)
			{
				return imageProperty.WzProperties.GetEnumerator ();
			}
		/*	else if (this is WzDirectory wzImage)
			{

				return imageProperty.WzProperties.GetEnumerator ();
			}*/
			else
			{
				//return default;
				return emptyWzImagePropertyList.GetEnumerator();
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		/*public virtual byte[] GetPngData(out PngInfo info)
        {
            info = default;
            return null;
        }*/
		public virtual byte[] GetWavData (out WavInfo info)
		{
			info = default;
			return null;
		}
		/*public byte[] GetPngDataConsideringLink(out PngInfo pngInfo)
        {
            if (this is WzCanvasProperty canvasProperty)
            {
                return canvasProperty.GetLinkedWzCanvasPngData(out pngInfo);
            }
            else
            {
                return GetPngData(out pngInfo);
            }
        }*/

		public Bitmap GetBitmapConsideringLink ()
		{
			if (this is WzCanvasProperty canvasProperty)
			{
				return canvasProperty.GetLinkedWzCanvasBitmap ();
			}
			else if (this is WzPngProperty pngProperty)
			{
				return pngProperty.GetBitmap ();
			}
			else
			{
				return GetBitmap ();
			}
		}

		public Texture asTexture => new Texture (this);
		public WzObject getChildByPath(string path, bool checkFirstDirectoryName = true)
        {
            string[] seperatedPath = path.Split("/".ToCharArray());

            if (seperatedPath.Length == 1)
                return this[path];
            WzObject curObj = this;
            for (int i = 1; i < seperatedPath.Length; i++)
            {
                if (curObj == null)
                {
                    return null;
                }
                switch (curObj.ObjectType)
                {
                    case WzObjectType.Directory:
                        curObj = ((WzDirectory)curObj)[seperatedPath[i]];
                        continue;
                    case WzObjectType.Image:
                        curObj = ((WzImage)curObj)[seperatedPath[i]];
                        continue;
                    case WzObjectType.Property:
                        switch (((WzImageProperty)curObj).PropertyType)
                        {
                            case WzPropertyType.Canvas:
                                curObj = ((WzCanvasProperty)curObj)[seperatedPath[i]];
                                continue;
                            case WzPropertyType.Convex:
                                curObj = ((WzConvexProperty)curObj)[seperatedPath[i]];
                                continue;
                            case WzPropertyType.SubProperty:
                                curObj = ((WzSubProperty)curObj)[seperatedPath[i]];
                                continue;
                            case WzPropertyType.Vector:
                                if (seperatedPath[i] == "X")
                                    return ((WzVectorProperty)curObj).X;
                                else if (seperatedPath[i] == "Y")
                                    return ((WzVectorProperty)curObj).Y;
                                else
                                    return null;
                            default: // Wut?
                                return null;
                        }
                }
            }
            if (curObj == null)
            {
                return null;
            }
            return curObj;
        }

		public List<WzImageProperty> Children
		{
			get
			{
				if (this is WzImage wzImage)
				{
					return wzImage.WzProperties;
				}
				else if (this is WzImageProperty imageProperty && imageProperty.WzProperties != null)
				{
					return imageProperty.WzProperties;
				}
				/*	else if (this is WzDirectory wzImage)
					{
			
						return imageProperty.WzProperties.GetEnumerator ();
					}*/
				else
				{
					//return default;
					return emptyWzImagePropertyList;
				}
			}
			}

		public WzPropertyType Type
		{
			get
			{
				if (this is WzImageProperty p)
				{
					return p.PropertyType;
				}
				else
				{
					return WzPropertyType.Null;
				}
			}
			
		}
			
	}

	public struct PngInfo
	{
		public int width;
		public int height;
		public int format;
	}

	public struct WavInfo
	{
		public int length;

	}

	public struct Point
	{
		public int X;
		public int Y;

		public Point (int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Point Empty;
	}

	public struct PointF
	{
		public float x;
		public float y;
		public float X => x;
		public float Y => y;
		public PointF (float x, float y)
		{
			this.x = x;
			this.y = y;
		}
	}

	public struct Color
	{
		public byte R;
		public byte G;
		public byte B;
		public byte A;

		public Color (byte a, byte r, byte g, byte b)
		{
			A = a;
			R = r;
			G = g;
			B = b;
		}
		public static Color White => new Color (1, 255, 255, 255);
		public static Color Black => new Color (0, 0, 0, 0);
		public static Color FromArgb (int a, int r, int g, int b)
		{
			return new Color ((byte)a, (byte)r, (byte)g, (byte)b);
		}

		public static Color FromArgb (int alpha, Color baseColor)
		{
			return new Color ((byte)alpha, (byte)baseColor.R, (byte)baseColor.G, (byte)baseColor.B);
		}

		public static Color FromArgb (int red, int green, int blue) => Color.FromArgb ((int)byte.MaxValue, red, green, blue);

	}
	public struct Rectangle
	{
		public static readonly Rectangle Empty;
		private int x;
		private int y;
		private int width;
		private int height;

		public Rectangle (int x, int y, int width, int height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public int X
		{
			get => this.x;
			set => this.x = value;
		}

		public int Y
		{
			get => this.y;
			set => this.y = value;
		}

		public int Width
		{
			get => this.width;
			set => this.width = value;
		}

		public int Height
		{
			get => this.height;
			set => this.height = value;
		}

		[Browsable (false)]
		public int Left => this.X;

		[Browsable (false)]
		public int Top => this.Y;

		[Browsable (false)]
		public int Right => this.X + this.Width;

		[Browsable (false)]
		public int Bottom => this.Y + this.Height;

		[Browsable (false)]
		public bool IsEmpty => this.height == 0 && this.width == 0 && this.x == 0 && this.y == 0;
	}

	public struct Size/* : IEquatable<Size>*/
	{
		public static readonly Size Empty;
		private int width;
		private int height;

		public Size (Point pt)
		{
			this.width = pt.X;
			this.height = pt.Y;
		}

		public Size (int width, int height)
		{
			this.width = width;
			this.height = height;
		}

		public int Width
		{
			get => this.width;
			set => this.width = value;
		}

		public int Height
		{
			get => this.height;
			set => this.height = value;
		}

		/*  public bool Equals(Size other)
          {
              throw new NotImplementedException();
          }*/

		/*public static Size Add(Size sz1, Size sz2) => new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);

        public static Size Ceiling(SizeF value) => new Size((int)Math.Ceiling((double)value.Width), (int)Math.Ceiling((double)value.Height));

        public static Size Subtract(Size sz1, Size sz2) => new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);

        public static Size Truncate(SizeF value) => new Size((int)value.Width, (int)value.Height);

        public static Size Round(SizeF value) => new Size((int)Math.Round((double)value.Width), (int)Math.Round((double)value.Height));

        public override bool Equals(object obj) => obj is Size other && this.Equals(other);

        public bool Equals(Size other) => this == other;

        public override int GetHashCode() => HashHelpers.Combine(this.Width, this.Height);*/

		public override string ToString () => "{Width=" + this.width.ToString () + ", Height=" + this.height.ToString () + "}";
	}
	/*[DebuggerDisplay("{NameAndARGBValue}")]
	[Serializable]
	public readonly struct Color : IEquatable<Color>
	{
		public static readonly Color Empty;
		private readonly string name;
		private readonly long value;
		private readonly short knownColor;
		private readonly short state;

		public static Color Transparent => new Color(KnownColor.Transparent);

		public static Color AliceBlue => new Color(KnownColor.AliceBlue);

		public static Color AntiqueWhite => new Color(KnownColor.AntiqueWhite);

		public static Color Aqua => new Color(KnownColor.Aqua);

		public static Color Aquamarine => new Color(KnownColor.Aquamarine);

		public static Color Azure => new Color(KnownColor.Azure);

		public static Color Beige => new Color(KnownColor.Beige);

		public static Color Bisque => new Color(KnownColor.Bisque);

		public static Color Black => new Color(KnownColor.Black);

		public static Color BlanchedAlmond => new Color(KnownColor.BlanchedAlmond);

		public static Color Blue => new Color(KnownColor.Blue);

		public static Color BlueViolet => new Color(KnownColor.BlueViolet);

		public static Color Brown => new Color(KnownColor.Brown);

		public static Color BurlyWood => new Color(KnownColor.BurlyWood);

		public static Color CadetBlue => new Color(KnownColor.CadetBlue);

		public static Color Chartreuse => new Color(KnownColor.Chartreuse);

		public static Color Chocolate => new Color(KnownColor.Chocolate);

		public static Color Coral => new Color(KnownColor.Coral);

		public static Color CornflowerBlue => new Color(KnownColor.CornflowerBlue);

		public static Color Cornsilk => new Color(KnownColor.Cornsilk);

		public static Color Crimson => new Color(KnownColor.Crimson);

		public static Color Cyan => new Color(KnownColor.Cyan);

		public static Color DarkBlue => new Color(KnownColor.DarkBlue);

		public static Color DarkCyan => new Color(KnownColor.DarkCyan);

		public static Color DarkGoldenrod => new Color(KnownColor.DarkGoldenrod);

		public static Color DarkGray => new Color(KnownColor.DarkGray);

		public static Color DarkGreen => new Color(KnownColor.DarkGreen);

		public static Color DarkKhaki => new Color(KnownColor.DarkKhaki);

		public static Color DarkMagenta => new Color(KnownColor.DarkMagenta);

		public static Color DarkOliveGreen => new Color(KnownColor.DarkOliveGreen);

		public static Color DarkOrange => new Color(KnownColor.DarkOrange);

		public static Color DarkOrchid => new Color(KnownColor.DarkOrchid);

		public static Color DarkRed => new Color(KnownColor.DarkRed);

		public static Color DarkSalmon => new Color(KnownColor.DarkSalmon);

		public static Color DarkSeaGreen => new Color(KnownColor.DarkSeaGreen);

		public static Color DarkSlateBlue => new Color(KnownColor.DarkSlateBlue);

		public static Color DarkSlateGray => new Color(KnownColor.DarkSlateGray);

		public static Color DarkTurquoise => new Color(KnownColor.DarkTurquoise);

		public static Color DarkViolet => new Color(KnownColor.DarkViolet);

		public static Color DeepPink => new Color(KnownColor.DeepPink);

		public static Color DeepSkyBlue => new Color(KnownColor.DeepSkyBlue);

		public static Color DimGray => new Color(KnownColor.DimGray);

		public static Color DodgerBlue => new Color(KnownColor.DodgerBlue);

		public static Color Firebrick => new Color(KnownColor.Firebrick);

		public static Color FloralWhite => new Color(KnownColor.FloralWhite);

		public static Color ForestGreen => new Color(KnownColor.ForestGreen);

		public static Color Fuchsia => new Color(KnownColor.Fuchsia);

		public static Color Gainsboro => new Color(KnownColor.Gainsboro);

		public static Color GhostWhite => new Color(KnownColor.GhostWhite);

		public static Color Gold => new Color(KnownColor.Gold);

		public static Color Goldenrod => new Color(KnownColor.Goldenrod);

		public static Color Gray => new Color(KnownColor.Gray);

		public static Color Green => new Color(KnownColor.Green);

		public static Color GreenYellow => new Color(KnownColor.GreenYellow);

		public static Color Honeydew => new Color(KnownColor.Honeydew);

		public static Color HotPink => new Color(KnownColor.HotPink);

		public static Color IndianRed => new Color(KnownColor.IndianRed);

		public static Color Indigo => new Color(KnownColor.Indigo);

		public static Color Ivory => new Color(KnownColor.Ivory);

		public static Color Khaki => new Color(KnownColor.Khaki);

		public static Color Lavender => new Color(KnownColor.Lavender);

		public static Color LavenderBlush => new Color(KnownColor.LavenderBlush);

		public static Color LawnGreen => new Color(KnownColor.LawnGreen);

		public static Color LemonChiffon => new Color(KnownColor.LemonChiffon);

		public static Color LightBlue => new Color(KnownColor.LightBlue);

		public static Color LightCoral => new Color(KnownColor.LightCoral);

		public static Color LightCyan => new Color(KnownColor.LightCyan);

		public static Color LightGoldenrodYellow => new Color(KnownColor.LightGoldenrodYellow);

		public static Color LightGreen => new Color(KnownColor.LightGreen);

		public static Color LightGray => new Color(KnownColor.LightGray);

		public static Color LightPink => new Color(KnownColor.LightPink);

		public static Color LightSalmon => new Color(KnownColor.LightSalmon);

		public static Color LightSeaGreen => new Color(KnownColor.LightSeaGreen);

		public static Color LightSkyBlue => new Color(KnownColor.LightSkyBlue);

		public static Color LightSlateGray => new Color(KnownColor.LightSlateGray);

		public static Color LightSteelBlue => new Color(KnownColor.LightSteelBlue);

		public static Color LightYellow => new Color(KnownColor.LightYellow);

		public static Color Lime => new Color(KnownColor.Lime);

		public static Color LimeGreen => new Color(KnownColor.LimeGreen);

		public static Color Linen => new Color(KnownColor.Linen);

		public static Color Magenta => new Color(KnownColor.Magenta);

		public static Color Maroon => new Color(KnownColor.Maroon);

		public static Color MediumAquamarine => new Color(KnownColor.MediumAquamarine);

		public static Color MediumBlue => new Color(KnownColor.MediumBlue);

		public static Color MediumOrchid => new Color(KnownColor.MediumOrchid);

		public static Color MediumPurple => new Color(KnownColor.MediumPurple);

		public static Color MediumSeaGreen => new Color(KnownColor.MediumSeaGreen);

		public static Color MediumSlateBlue => new Color(KnownColor.MediumSlateBlue);

		public static Color MediumSpringGreen => new Color(KnownColor.MediumSpringGreen);

		public static Color MediumTurquoise => new Color(KnownColor.MediumTurquoise);

		public static Color MediumVioletRed => new Color(KnownColor.MediumVioletRed);

		public static Color MidnightBlue => new Color(KnownColor.MidnightBlue);

		public static Color MintCream => new Color(KnownColor.MintCream);

		public static Color MistyRose => new Color(KnownColor.MistyRose);

		public static Color Moccasin => new Color(KnownColor.Moccasin);

		public static Color NavajoWhite => new Color(KnownColor.NavajoWhite);

		public static Color Navy => new Color(KnownColor.Navy);

		public static Color OldLace => new Color(KnownColor.OldLace);

		public static Color Olive => new Color(KnownColor.Olive);

		public static Color OliveDrab => new Color(KnownColor.OliveDrab);

		public static Color Orange => new Color(KnownColor.Orange);

		public static Color OrangeRed => new Color(KnownColor.OrangeRed);

		public static Color Orchid => new Color(KnownColor.Orchid);

		public static Color PaleGoldenrod => new Color(KnownColor.PaleGoldenrod);

		public static Color PaleGreen => new Color(KnownColor.PaleGreen);

		public static Color PaleTurquoise => new Color(KnownColor.PaleTurquoise);

		public static Color PaleVioletRed => new Color(KnownColor.PaleVioletRed);

		public static Color PapayaWhip => new Color(KnownColor.PapayaWhip);

		public static Color PeachPuff => new Color(KnownColor.PeachPuff);

		public static Color Peru => new Color(KnownColor.Peru);

		public static Color Pink => new Color(KnownColor.Pink);

		public static Color Plum => new Color(KnownColor.Plum);

		public static Color PowderBlue => new Color(KnownColor.PowderBlue);

		public static Color Purple => new Color(KnownColor.Purple);

		public static Color Red => new Color(KnownColor.Red);

		public static Color RosyBrown => new Color(KnownColor.RosyBrown);

		public static Color RoyalBlue => new Color(KnownColor.RoyalBlue);

		public static Color SaddleBrown => new Color(KnownColor.SaddleBrown);

		public static Color Salmon => new Color(KnownColor.Salmon);

		public static Color SandyBrown => new Color(KnownColor.SandyBrown);

		public static Color SeaGreen => new Color(KnownColor.SeaGreen);

		public static Color SeaShell => new Color(KnownColor.SeaShell);

		public static Color Sienna => new Color(KnownColor.Sienna);

		public static Color Silver => new Color(KnownColor.Silver);

		public static Color SkyBlue => new Color(KnownColor.SkyBlue);

		public static Color SlateBlue => new Color(KnownColor.SlateBlue);

		public static Color SlateGray => new Color(KnownColor.SlateGray);

		public static Color Snow => new Color(KnownColor.Snow);

		public static Color SpringGreen => new Color(KnownColor.SpringGreen);

		public static Color SteelBlue => new Color(KnownColor.SteelBlue);

		public static Color Tan => new Color(KnownColor.Tan);

		public static Color Teal => new Color(KnownColor.Teal);

		public static Color Thistle => new Color(KnownColor.Thistle);

		public static Color Tomato => new Color(KnownColor.Tomato);

		public static Color Turquoise => new Color(KnownColor.Turquoise);

		public static Color Violet => new Color(KnownColor.Violet);

		public static Color Wheat => new Color(KnownColor.Wheat);

		public static Color White => new Color(KnownColor.White);

		public static Color WhiteSmoke => new Color(KnownColor.WhiteSmoke);

		public static Color Yellow => new Color(KnownColor.Yellow);

		public static Color YellowGreen => new Color(KnownColor.YellowGreen);

		internal Color(KnownColor knownColor)
		{
			this.value = 0L;
			this.state = (short)1;
			this.name = (string)null;
			this.knownColor = (short)knownColor;
		}

		private Color(long value, short state, string name, KnownColor knownColor)
		{
			this.value = value;
			this.state = state;
			this.name = name;
			this.knownColor = (short)knownColor;
		}

		public byte R => (byte)(this.Value >> 16);

		public byte G => (byte)(this.Value >> 8);

		public byte B => (byte)this.Value;

		public byte A => (byte)(this.Value >> 24);

		public bool IsKnownColor => ((uint)this.state & 1U) > 0U;

		public bool IsEmpty => this.state == (short)0;

		public bool IsNamedColor => ((int)this.state & 8) != 0 || this.IsKnownColor;

		public bool IsSystemColor => this.IsKnownColor && Color.IsKnownColorSystem((KnownColor)this.knownColor);

		internal static bool IsKnownColorSystem(KnownColor knownColor) => knownColor <= KnownColor.WindowText || knownColor > KnownColor.YellowGreen;

		private string NameAndARGBValue => string.Format("{{Name={0}, ARGB=({1}, {2}, {3}, {4})}}", (object)this.Name, (object)this.A, (object)this.R, (object)this.G, (object)this.B);

		public string Name
		{
			get
			{
				if (((int)this.state & 8) != 0)
					return this.name;
				return this.IsKnownColor ? KnownColorNames.KnownColorToName((KnownColor)this.knownColor) : Convert.ToString(this.value, 16);
			}
		}

		private long Value
		{
			get
			{
				if (((int)this.state & 2) != 0)
					return this.value;
				return this.IsKnownColor ? (long)KnownColorTable.KnownColorToArgb((KnownColor)this.knownColor) : 0L;
			}
		}

		private static void CheckByte(int value, string name)
		{
			if ((uint)value <= (uint)byte.MaxValue)
				return;
			ThrowOutOfByteRange(value, name);

			void ThrowOutOfByteRange(int v, string n) => throw new ArgumentException(SR.Format(SR.InvalidEx2BoundArgument, (object)n, (object)v, (object)(byte)0, (object)byte.MaxValue));
		}

		private static Color FromArgb(uint argb) => new Color((long)argb, (short)2, (string)null, (KnownColor)0);

		public static Color FromArgb(int argb) => Color.FromArgb((uint)argb);

		public static Color FromArgb(int alpha, int red, int green, int blue)
		{
			Color.CheckByte(alpha, nameof(alpha));
			Color.CheckByte(red, nameof(red));
			Color.CheckByte(green, nameof(green));
			Color.CheckByte(blue, nameof(blue));
			return Color.FromArgb((uint)(alpha << 24 | red << 16 | green << 8 | blue));
		}

		public static Color FromArgb(int alpha, Color baseColor)
		{
			Color.CheckByte(alpha, nameof(alpha));
			return Color.FromArgb((uint)(alpha << 24 | (int)(uint)baseColor.Value & 16777215));
		}

		public static Color FromArgb(int red, int green, int blue) => Color.FromArgb((int)byte.MaxValue, red, green, blue);

		public static Color FromKnownColor(KnownColor color) => color > (KnownColor)0 && color <= KnownColor.MenuHighlight ? new Color(color) : Color.FromName(color.ToString());

		public static Color FromName(string name)
		{
			Color result;
			return ColorTable.TryGetNamedColor(name, out result) ? result : new Color(0L, (short)8, name, (KnownColor)0);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void GetRgbValues(out int r, out int g, out int b)
		{
			uint num = (uint)this.Value;
			r = ((int)num & 16711680) >> 16;
			g = ((int)num & 65280) >> 8;
			b = (int)num & (int)byte.MaxValue;
		}

		public float GetBrightness()
		{
			int r;
			int g;
			int b;
			this.GetRgbValues(out r, out g, out b);
			int num = Math.Min(Math.Min(r, g), b);
			return (float)(Math.Max(Math.Max(r, g), b) + num) / 510f;
		}

		public float GetHue()
		{
			int r;
			int g;
			int b;
			this.GetRgbValues(out r, out g, out b);
			if (r == g && g == b)
				return 0.0f;
			int num1 = Math.Min(Math.Min(r, g), b);
			int num2 = Math.Max(Math.Max(r, g), b);
			float num3 = (float)(num2 - num1);
			float num4 = (r != num2 ? (g != num2 ? (float)((double)(r - g) / (double)num3 + 4.0) : (float)((double)(b - r) / (double)num3 + 2.0)) : (float)(g - b) / num3) * 60f;
			if ((double)num4 < 0.0)
				num4 += 360f;
			return num4;
		}

		public float GetSaturation()
		{
			int r;
			int g;
			int b;
			this.GetRgbValues(out r, out g, out b);
			if (r == g && g == b)
				return 0.0f;
			int num1 = Math.Min(Math.Min(r, g), b);
			int num2 = Math.Max(Math.Max(r, g), b);
			int num3 = num2 + num1;
			if (num3 > (int)byte.MaxValue)
				num3 = 510 - num2 - num1;
			return (float)(num2 - num1) / (float)num3;
		}

		public int ToArgb() => (int)this.Value;

		public KnownColor ToKnownColor() => (KnownColor)this.knownColor;

		public override string ToString()
		{
			if (this.IsNamedColor)
				return "Color [" + this.Name + "]";
			if (((int)this.state & 2) == 0)
				return "Color [Empty]";
			return "Color [A=" + this.A.ToString() + ", R=" + this.R.ToString() + ", G=" + this.G.ToString() + ", B=" + this.B.ToString() + "]";
		}

		public static bool operator ==(Color left, Color right) => left.value == right.value && (int)left.state == (int)right.state && (int)left.knownColor == (int)right.knownColor && left.name == right.name;

		public static bool operator !=(Color left, Color right) => !(left == right);

		public override bool Equals(object obj) => obj is Color other && this.Equals(other);

		public bool Equals(Color other) => this == other;

		public override int GetHashCode() => this.name != null & !this.IsKnownColor ? this.name.GetHashCode() : HashHelpers.Combine(HashHelpers.Combine(this.value.GetHashCode(), this.state.GetHashCode()), this.knownColor.GetHashCode());
	}

	public enum KnownColor
	{
		ActiveBorder = 1,
		ActiveCaption = 2,
		ActiveCaptionText = 3,
		AppWorkspace = 4,
		Control = 5,
		ControlDark = 6,
		ControlDarkDark = 7,
		ControlLight = 8,
		ControlLightLight = 9,
		ControlText = 10, // 0x0000000A
		Desktop = 11, // 0x0000000B
		GrayText = 12, // 0x0000000C
		Highlight = 13, // 0x0000000D
		HighlightText = 14, // 0x0000000E
		HotTrack = 15, // 0x0000000F
		InactiveBorder = 16, // 0x00000010
		InactiveCaption = 17, // 0x00000011
		InactiveCaptionText = 18, // 0x00000012
		Info = 19, // 0x00000013
		InfoText = 20, // 0x00000014
		Menu = 21, // 0x00000015
		MenuText = 22, // 0x00000016
		ScrollBar = 23, // 0x00000017
		Window = 24, // 0x00000018
		WindowFrame = 25, // 0x00000019
		WindowText = 26, // 0x0000001A
		Transparent = 27, // 0x0000001B
		AliceBlue = 28, // 0x0000001C
		AntiqueWhite = 29, // 0x0000001D
		Aqua = 30, // 0x0000001E
		Aquamarine = 31, // 0x0000001F
		Azure = 32, // 0x00000020
		Beige = 33, // 0x00000021
		Bisque = 34, // 0x00000022
		Black = 35, // 0x00000023
		BlanchedAlmond = 36, // 0x00000024
		Blue = 37, // 0x00000025
		BlueViolet = 38, // 0x00000026
		Brown = 39, // 0x00000027
		BurlyWood = 40, // 0x00000028
		CadetBlue = 41, // 0x00000029
		Chartreuse = 42, // 0x0000002A
		Chocolate = 43, // 0x0000002B
		Coral = 44, // 0x0000002C
		CornflowerBlue = 45, // 0x0000002D
		Cornsilk = 46, // 0x0000002E
		Crimson = 47, // 0x0000002F
		Cyan = 48, // 0x00000030
		DarkBlue = 49, // 0x00000031
		DarkCyan = 50, // 0x00000032
		DarkGoldenrod = 51, // 0x00000033
		DarkGray = 52, // 0x00000034
		DarkGreen = 53, // 0x00000035
		DarkKhaki = 54, // 0x00000036
		DarkMagenta = 55, // 0x00000037
		DarkOliveGreen = 56, // 0x00000038
		DarkOrange = 57, // 0x00000039
		DarkOrchid = 58, // 0x0000003A
		DarkRed = 59, // 0x0000003B
		DarkSalmon = 60, // 0x0000003C
		DarkSeaGreen = 61, // 0x0000003D
		DarkSlateBlue = 62, // 0x0000003E
		DarkSlateGray = 63, // 0x0000003F
		DarkTurquoise = 64, // 0x00000040
		DarkViolet = 65, // 0x00000041
		DeepPink = 66, // 0x00000042
		DeepSkyBlue = 67, // 0x00000043
		DimGray = 68, // 0x00000044
		DodgerBlue = 69, // 0x00000045
		Firebrick = 70, // 0x00000046
		FloralWhite = 71, // 0x00000047
		ForestGreen = 72, // 0x00000048
		Fuchsia = 73, // 0x00000049
		Gainsboro = 74, // 0x0000004A
		GhostWhite = 75, // 0x0000004B
		Gold = 76, // 0x0000004C
		Goldenrod = 77, // 0x0000004D
		Gray = 78, // 0x0000004E
		Green = 79, // 0x0000004F
		GreenYellow = 80, // 0x00000050
		Honeydew = 81, // 0x00000051
		HotPink = 82, // 0x00000052
		IndianRed = 83, // 0x00000053
		Indigo = 84, // 0x00000054
		Ivory = 85, // 0x00000055
		Khaki = 86, // 0x00000056
		Lavender = 87, // 0x00000057
		LavenderBlush = 88, // 0x00000058
		LawnGreen = 89, // 0x00000059
		LemonChiffon = 90, // 0x0000005A
		LightBlue = 91, // 0x0000005B
		LightCoral = 92, // 0x0000005C
		LightCyan = 93, // 0x0000005D
		LightGoldenrodYellow = 94, // 0x0000005E
		LightGray = 95, // 0x0000005F
		LightGreen = 96, // 0x00000060
		LightPink = 97, // 0x00000061
		LightSalmon = 98, // 0x00000062
		LightSeaGreen = 99, // 0x00000063
		LightSkyBlue = 100, // 0x00000064
		LightSlateGray = 101, // 0x00000065
		LightSteelBlue = 102, // 0x00000066
		LightYellow = 103, // 0x00000067
		Lime = 104, // 0x00000068
		LimeGreen = 105, // 0x00000069
		Linen = 106, // 0x0000006A
		Magenta = 107, // 0x0000006B
		Maroon = 108, // 0x0000006C
		MediumAquamarine = 109, // 0x0000006D
		MediumBlue = 110, // 0x0000006E
		MediumOrchid = 111, // 0x0000006F
		MediumPurple = 112, // 0x00000070
		MediumSeaGreen = 113, // 0x00000071
		MediumSlateBlue = 114, // 0x00000072
		MediumSpringGreen = 115, // 0x00000073
		MediumTurquoise = 116, // 0x00000074
		MediumVioletRed = 117, // 0x00000075
		MidnightBlue = 118, // 0x00000076
		MintCream = 119, // 0x00000077
		MistyRose = 120, // 0x00000078
		Moccasin = 121, // 0x00000079
		NavajoWhite = 122, // 0x0000007A
		Navy = 123, // 0x0000007B
		OldLace = 124, // 0x0000007C
		Olive = 125, // 0x0000007D
		OliveDrab = 126, // 0x0000007E
		Orange = 127, // 0x0000007F
		OrangeRed = 128, // 0x00000080
		Orchid = 129, // 0x00000081
		PaleGoldenrod = 130, // 0x00000082
		PaleGreen = 131, // 0x00000083
		PaleTurquoise = 132, // 0x00000084
		PaleVioletRed = 133, // 0x00000085
		PapayaWhip = 134, // 0x00000086
		PeachPuff = 135, // 0x00000087
		Peru = 136, // 0x00000088
		Pink = 137, // 0x00000089
		Plum = 138, // 0x0000008A
		PowderBlue = 139, // 0x0000008B
		Purple = 140, // 0x0000008C
		Red = 141, // 0x0000008D
		RosyBrown = 142, // 0x0000008E
		RoyalBlue = 143, // 0x0000008F
		SaddleBrown = 144, // 0x00000090
		Salmon = 145, // 0x00000091
		SandyBrown = 146, // 0x00000092
		SeaGreen = 147, // 0x00000093
		SeaShell = 148, // 0x00000094
		Sienna = 149, // 0x00000095
		Silver = 150, // 0x00000096
		SkyBlue = 151, // 0x00000097
		SlateBlue = 152, // 0x00000098
		SlateGray = 153, // 0x00000099
		Snow = 154, // 0x0000009A
		SpringGreen = 155, // 0x0000009B
		SteelBlue = 156, // 0x0000009C
		Tan = 157, // 0x0000009D
		Teal = 158, // 0x0000009E
		Thistle = 159, // 0x0000009F
		Tomato = 160, // 0x000000A0
		Turquoise = 161, // 0x000000A1
		Violet = 162, // 0x000000A2
		Wheat = 163, // 0x000000A3
		White = 164, // 0x000000A4
		WhiteSmoke = 165, // 0x000000A5
		Yellow = 166, // 0x000000A6
		YellowGreen = 167, // 0x000000A7
		ButtonFace = 168, // 0x000000A8
		ButtonHighlight = 169, // 0x000000A9
		ButtonShadow = 170, // 0x000000AA
		GradientActiveCaption = 171, // 0x000000AB
		GradientInactiveCaption = 172, // 0x000000AC
		MenuBar = 173, // 0x000000AD
		MenuHighlight = 174, // 0x000000AE
	}

	internal static class ColorTable
	{
		private static readonly Lazy<Dictionary<string, Color>> s_colorConstants = new Lazy<Dictionary<string, Color>>(new Func<Dictionary<string, Color>>(ColorTable.GetColors));

		private static Dictionary<string, Color> GetColors()
		{
			Dictionary<string, Color> dictionary = new Dictionary<string, Color>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
			ColorTable.FillWithProperties(dictionary, typeof(Color));
			ColorTable.FillWithProperties(dictionary, typeof(SystemColors));
			return dictionary;
		}

		private static void FillWithProperties(
		  Dictionary<string, Color> dictionary,
		  Type typeWithColors)
		{
			foreach (PropertyInfo property in typeWithColors.GetProperties(BindingFlags.Static | BindingFlags.Public))
			{
				if (property.PropertyType == typeof(Color))
					dictionary[property.Name] = (Color)property.GetValue((object)null, (object[])null);
			}
		}

		internal static Dictionary<string, Color> Colors => ColorTable.s_colorConstants.Value;

		internal static bool TryGetNamedColor(string name, out Color result) => ColorTable.Colors.TryGetValue(name, out result);
	}
	internal static class KnownColorTable
	{
		private static readonly uint[] s_colorTable = new uint[141]
		{
	  16777215U,
	  4293982463U,
	  4294634455U,
	  4278255615U,
	  4286578644U,
	  4293984255U,
	  4294309340U,
	  4294960324U,
	  4278190080U,
	  4294962125U,
	  4278190335U,
	  4287245282U,
	  4289014314U,
	  4292786311U,
	  4284456608U,
	  4286578432U,
	  4291979550U,
	  4294934352U,
	  4284782061U,
	  4294965468U,
	  4292613180U,
	  4278255615U,
	  4278190219U,
	  4278225803U,
	  4290283019U,
	  4289309097U,
	  4278215680U,
	  4290623339U,
	  4287299723U,
	  4283788079U,
	  4294937600U,
	  4288230092U,
	  4287299584U,
	  4293498490U,
	  4287609995U,
	  4282924427U,
	  4281290575U,
	  4278243025U,
	  4287889619U,
	  4294907027U,
	  4278239231U,
	  4285098345U,
	  4280193279U,
	  4289864226U,
	  4294966000U,
	  4280453922U,
	  4294902015U,
	  4292664540U,
	  4294506751U,
	  4294956800U,
	  4292519200U,
	  4286611584U,
	  4278222848U,
	  4289593135U,
	  4293984240U,
	  4294928820U,
	  4291648604U,
	  4283105410U,
	  4294967280U,
	  4293977740U,
	  4293322490U,
	  4294963445U,
	  4286381056U,
	  4294965965U,
	  4289583334U,
	  4293951616U,
	  4292935679U,
	  4294638290U,
	  4292072403U,
	  4287688336U,
	  4294948545U,
	  4294942842U,
	  4280332970U,
	  4287090426U,
	  4286023833U,
	  4289774814U,
	  4294967264U,
	  4278255360U,
	  4281519410U,
	  4294635750U,
	  4294902015U,
	  4286578688U,
	  4284927402U,
	  4278190285U,
	  4290401747U,
	  4287852763U,
	  4282168177U,
	  4286277870U,
	  4278254234U,
	  4282962380U,
	  4291237253U,
	  4279834992U,
	  4294311930U,
	  4294960353U,
	  4294960309U,
	  4294958765U,
	  4278190208U,
	  4294833638U,
	  4286611456U,
	  4285238819U,
	  4294944000U,
	  4294919424U,
	  4292505814U,
	  4293847210U,
	  4288215960U,
	  4289720046U,
	  4292571283U,
	  4294963157U,
	  4294957753U,
	  4291659071U,
	  4294951115U,
	  4292714717U,
	  4289781990U,
	  4286578816U,
	  4294901760U,
	  4290547599U,
	  4282477025U,
	  4287317267U,
	  4294606962U,
	  4294222944U,
	  4281240407U,
	  4294964718U,
	  4288696877U,
	  4290822336U,
	  4287090411U,
	  4285160141U,
	  4285563024U,
	  4294966010U,
	  4278255487U,
	  4282811060U,
	  4291998860U,
	  4278222976U,
	  4292394968U,
	  4294927175U,
	  4282441936U,
	  4293821166U,
	  4294303411U,
	  uint.MaxValue,
	  4294309365U,
	  4294967040U,
	  4288335154U
		};

		internal static Color ArgbToKnownColor(uint argb)
		{
			for (int index = 1; index < KnownColorTable.s_colorTable.Length; ++index)
			{
				if ((int)KnownColorTable.s_colorTable[index] == (int)argb)
					return Color.FromKnownColor((KnownColor)(index + 27));
			}
			return Color.FromArgb((int)argb);
		}

		public static uint KnownColorToArgb(KnownColor color) => !Color.IsKnownColorSystem(color) ? KnownColorTable.s_colorTable[(int)(color - 27)] : KnownColorTable.GetSystemColorArgb(color);

		private static unsafe ReadOnlySpan<byte> SystemColorIdTable => new ReadOnlySpan<byte>((void*)&\u003CPrivateImplementationDetails\u003E.\u003265182B1D4C6931DCFA70DED15DBD386810B80FF, 33);

		public static uint GetSystemColorArgb(KnownColor color) => ColorTranslator.COLORREFToARGB(Interop.User32.GetSysColor(KnownColorTable.GetSystemColorId(color)));

		private static int GetSystemColorId(KnownColor color) => color >= KnownColor.Transparent ? (int)KnownColorTable.SystemColorIdTable[(int)(color - 168 + 26)] : (int)KnownColorTable.SystemColorIdTable[(int)(color - 1)];
	}

	internal static class KnownColorNames
	{
		private static readonly string[] s_colorNameTable = new string[174]
		{
	  "ActiveBorder",
	  "ActiveCaption",
	  "ActiveCaptionText",
	  "AppWorkspace",
	  "Control",
	  "ControlDark",
	  "ControlDarkDark",
	  "ControlLight",
	  "ControlLightLight",
	  "ControlText",
	  "Desktop",
	  "GrayText",
	  "Highlight",
	  "HighlightText",
	  "HotTrack",
	  "InactiveBorder",
	  "InactiveCaption",
	  "InactiveCaptionText",
	  "Info",
	  "InfoText",
	  "Menu",
	  "MenuText",
	  "ScrollBar",
	  "Window",
	  "WindowFrame",
	  "WindowText",
	  "Transparent",
	  "AliceBlue",
	  "AntiqueWhite",
	  "Aqua",
	  "Aquamarine",
	  "Azure",
	  "Beige",
	  "Bisque",
	  "Black",
	  "BlanchedAlmond",
	  "Blue",
	  "BlueViolet",
	  "Brown",
	  "BurlyWood",
	  "CadetBlue",
	  "Chartreuse",
	  "Chocolate",
	  "Coral",
	  "CornflowerBlue",
	  "Cornsilk",
	  "Crimson",
	  "Cyan",
	  "DarkBlue",
	  "DarkCyan",
	  "DarkGoldenrod",
	  "DarkGray",
	  "DarkGreen",
	  "DarkKhaki",
	  "DarkMagenta",
	  "DarkOliveGreen",
	  "DarkOrange",
	  "DarkOrchid",
	  "DarkRed",
	  "DarkSalmon",
	  "DarkSeaGreen",
	  "DarkSlateBlue",
	  "DarkSlateGray",
	  "DarkTurquoise",
	  "DarkViolet",
	  "DeepPink",
	  "DeepSkyBlue",
	  "DimGray",
	  "DodgerBlue",
	  "Firebrick",
	  "FloralWhite",
	  "ForestGreen",
	  "Fuchsia",
	  "Gainsboro",
	  "GhostWhite",
	  "Gold",
	  "Goldenrod",
	  "Gray",
	  "Green",
	  "GreenYellow",
	  "Honeydew",
	  "HotPink",
	  "IndianRed",
	  "Indigo",
	  "Ivory",
	  "Khaki",
	  "Lavender",
	  "LavenderBlush",
	  "LawnGreen",
	  "LemonChiffon",
	  "LightBlue",
	  "LightCoral",
	  "LightCyan",
	  "LightGoldenrodYellow",
	  "LightGray",
	  "LightGreen",
	  "LightPink",
	  "LightSalmon",
	  "LightSeaGreen",
	  "LightSkyBlue",
	  "LightSlateGray",
	  "LightSteelBlue",
	  "LightYellow",
	  "Lime",
	  "LimeGreen",
	  "Linen",
	  "Magenta",
	  "Maroon",
	  "MediumAquamarine",
	  "MediumBlue",
	  "MediumOrchid",
	  "MediumPurple",
	  "MediumSeaGreen",
	  "MediumSlateBlue",
	  "MediumSpringGreen",
	  "MediumTurquoise",
	  "MediumVioletRed",
	  "MidnightBlue",
	  "MintCream",
	  "MistyRose",
	  "Moccasin",
	  "NavajoWhite",
	  "Navy",
	  "OldLace",
	  "Olive",
	  "OliveDrab",
	  "Orange",
	  "OrangeRed",
	  "Orchid",
	  "PaleGoldenrod",
	  "PaleGreen",
	  "PaleTurquoise",
	  "PaleVioletRed",
	  "PapayaWhip",
	  "PeachPuff",
	  "Peru",
	  "Pink",
	  "Plum",
	  "PowderBlue",
	  "Purple",
	  "Red",
	  "RosyBrown",
	  "RoyalBlue",
	  "SaddleBrown",
	  "Salmon",
	  "SandyBrown",
	  "SeaGreen",
	  "SeaShell",
	  "Sienna",
	  "Silver",
	  "SkyBlue",
	  "SlateBlue",
	  "SlateGray",
	  "Snow",
	  "SpringGreen",
	  "SteelBlue",
	  "Tan",
	  "Teal",
	  "Thistle",
	  "Tomato",
	  "Turquoise",
	  "Violet",
	  "Wheat",
	  "White",
	  "WhiteSmoke",
	  "Yellow",
	  "YellowGreen",
	  "ButtonFace",
	  "ButtonHighlight",
	  "ButtonShadow",
	  "GradientActiveCaption",
	  "GradientInactiveCaption",
	  "MenuBar",
	  "MenuHighlight"
		};

		public static string KnownColorToName(KnownColor color) => KnownColorNames.s_colorNameTable[(int)(color - 1)];
	}*/

	public enum PixelFormat
	{
		DontCare = 0,
		Undefined = 0,
		Max = 15, // 0x0000000F
		Indexed = 65536, // 0x00010000
		Gdi = 131072, // 0x00020000
		Format16bppRgb555 = 135173, // 0x00021005
		Format16bppRgb565 = 135174, // 0x00021006
		Format24bppRgb = 137224, // 0x00021808
		Format32bppRgb = 139273, // 0x00022009
		Format1bppIndexed = 196865, // 0x00030101
		Format4bppIndexed = 197634, // 0x00030402
		Format8bppIndexed = 198659, // 0x00030803
		Alpha = 262144, // 0x00040000
		Format16bppArgb1555 = 397319, // 0x00061007
		PAlpha = 524288, // 0x00080000
		Format32bppPArgb = 925707, // 0x000E200B
		Extended = 1048576, // 0x00100000
		Format16bppGrayScale = 1052676, // 0x00101004
		Format48bppRgb = 1060876, // 0x0010300C
		Format64bppPArgb = 1851406, // 0x001C400E
		Canonical = 2097152, // 0x00200000
		Format32bppArgb = 2498570, // 0x0026200A
		Format64bppArgb = 3424269, // 0x0034400D
	}



	/*    public sealed class BitmapData
        {
            private int _width;
            private int _height;
            private int _stride;
            private PixelFormat _pixelFormat;
            private IntPtr _scan0;
            private int _reserved;

            public int Width
            {
                get => this._width;
                set => this._width = value;
            }

            public int Height
            {
                get => this._height;
                set => this._height = value;
            }

            public int Stride
            {
                get => this._stride;
                set => this._stride = value;
            }

            public PixelFormat PixelFormat
            {
                get => this._pixelFormat;
                set
                {
                    switch (value)
                    {
                        case PixelFormat.Undefined:
                        case PixelFormat.Max:
                        case PixelFormat.Indexed:
                        case PixelFormat.Gdi:
                        case PixelFormat.Format16bppRgb555:
                        case PixelFormat.Format16bppRgb565:
                        case PixelFormat.Format24bppRgb:
                        case PixelFormat.Format32bppRgb:
                        case PixelFormat.Format1bppIndexed:
                        case PixelFormat.Format4bppIndexed:
                        case PixelFormat.Format8bppIndexed:
                        case PixelFormat.Alpha:
                        case PixelFormat.Format16bppArgb1555:
                        case PixelFormat.PAlpha:
                        case PixelFormat.Format32bppPArgb:
                        case PixelFormat.Extended:
                        case PixelFormat.Format16bppGrayScale:
                        case PixelFormat.Format48bppRgb:
                        case PixelFormat.Format64bppPArgb:
                        case PixelFormat.Canonical:
                        case PixelFormat.Format32bppArgb:
                        case PixelFormat.Format64bppArgb:
                            this._pixelFormat = value;
                            break;
                        default:
                            throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(PixelFormat));
                    }
                }
            }

            public IntPtr Scan0
            {
                get => this._scan0;
                set => this._scan0 = value;
            }

            public int Reserved
            {
                get => this._reserved;
                set => this._reserved = value;
            }
        }



        [Serializable]
        public abstract class Image : MarshalByRefObject, IDisposable, ICloneable
        {
            internal IntPtr nativeImage;
            private object _userData;
            private byte[] _rawData;

            [Localizable(false)]
            [DefaultValue(null)]
            public object Tag
            {
                get => this._userData;
                set => this._userData = value;
            }

            private protected Image()
            {
            }

            private protected Image(SerializationInfo info, StreamingContext context)
            {
                byte[] buffer = (byte[])info.GetValue("Data", typeof(byte[]));
                try
                {
                    this.SetNativeImage(this.InitializeFromStream((Stream)new MemoryStream(buffer)));
                }
                catch (ExternalException ex)
                {
                }
                catch (ArgumentException ex)
                {
                }
                catch (OutOfMemoryException ex)
                {
                }
                catch (InvalidOperationException ex)
                {
                }
                catch (NotImplementedException ex)
                {
                }
                catch (FileNotFoundException ex)
                {
                }
            }

            void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    this.Save(stream);
                    si.AddValue("Data", (object)stream.ToArray(), typeof(byte[]));
                }
            }

            public static Image FromFile(string filename) => Image.FromFile(filename, false);

            public static Image FromStream(Stream stream) => Image.FromStream(stream, false);

            public static Image FromStream(Stream stream, bool useEmbeddedColorManagement) => Image.FromStream(stream, useEmbeddedColorManagement, true);

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize((object)this);
            }

            ~Image() => this.Dispose(false);

            public void Save(string filename) => this.Save(filename, this.RawFormat);

            public SizeF PhysicalDimension
            {
                get
                {
                    float Width;
                    float height;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageDimension(new HandleRef((object)this, this.nativeImage), out Width, out height));
                    return new SizeF(Width, height);
                }
            }

            public Size Size => new Size(this.Width, this.Height);

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            [Browsable(false)]
            [DefaultValue(false)]
            public int Width
            {
                get
                {
                    int Width;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageWidth(new HandleRef((object)this, this.nativeImage), out Width));
                    return Width;
                }
            }

            [Browsable(false)]
            [DefaultValue(false)]
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public int Height
            {
                get
                {
                    int height;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageHeight(new HandleRef((object)this, this.nativeImage), out height));
                    return height;
                }
            }

            public float HorizontalResolution
            {
                get
                {
                    float horzRes;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageHorizontalResolution(new HandleRef((object)this, this.nativeImage), out horzRes));
                    return horzRes;
                }
            }

            public float VerticalResolution
            {
                get
                {
                    float vertRes;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageVerticalResolution(new HandleRef((object)this, this.nativeImage), out vertRes));
                    return vertRes;
                }
            }

            [Browsable(false)]
            public int Flags
            {
                get
                {
                    int flags;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageFlags(new HandleRef((object)this, this.nativeImage), out flags));
                    return flags;
                }
            }

            public ImageFormat RawFormat
            {
                get
                {
                    Guid format = new Guid();
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageRawFormat(new HandleRef((object)this, this.nativeImage), ref format));
                    return new ImageFormat(format);
                }
            }

            public PixelFormat PixelFormat
            {
                get
                {
                    PixelFormat format;
                    return SafeNativeMethods.Gdip.GdipGetImagePixelFormat(new HandleRef((object)this, this.nativeImage), out format) == 0 ? format : PixelFormat.Undefined;
                }
            }

            public int GetFrameCount(FrameDimension dimension)
            {
                Guid guid = dimension.Guid;
                int count;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageGetFrameCount(new HandleRef((object)this, this.nativeImage), ref guid, out count));
                return count;
            }

            public int SelectActiveFrame(FrameDimension dimension, int frameIndex)
            {
                Guid guid = dimension.Guid;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageSelectActiveFrame(new HandleRef((object)this, this.nativeImage), ref guid, frameIndex));
                return 0;
            }

            public void RotateFlip(RotateFlipType rotateFlipType) => SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageRotateFlip(new HandleRef((object)this, this.nativeImage), (int)rotateFlipType));

            public void RemovePropertyItem(int propid) => SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipRemovePropertyItem(new HandleRef((object)this, this.nativeImage), propid));

            public static Bitmap FromHbitmap(IntPtr hbitmap) => Image.FromHbitmap(hbitmap, IntPtr.Zero);

            public static Bitmap FromHbitmap(IntPtr hbitmap, IntPtr hpalette)
            {
                IntPtr bitmap = IntPtr.Zero;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipCreateBitmapFromHBITMAP(new HandleRef((object)null, hbitmap), new HandleRef((object)null, hpalette), out bitmap));
                return new Bitmap(bitmap);
            }

            public static bool IsExtendedPixelFormat(PixelFormat pixfmt) => (uint)(pixfmt & PixelFormat.Extended) > 0U;

            public static bool IsCanonicalPixelFormat(PixelFormat pixfmt) => (uint)(pixfmt & PixelFormat.Canonical) > 0U;

            internal void SetNativeImage(IntPtr handle) => this.nativeImage = !(handle == IntPtr.Zero) ? handle : throw new ArgumentException(SR.NativeHandle0, nameof(handle));

            [Browsable(false)]
            public unsafe Guid[] FrameDimensionsList
            {
                get
                {
                    int count;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsCount(new HandleRef((object)this, this.nativeImage), out count));
                    if (count <= 0)
                        return Array.Empty<Guid>();
                    Guid[] guidArray = new Guid[count];
                    fixed (Guid* dimensionIDs = guidArray)
                        SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsList(new HandleRef((object)this, this.nativeImage), dimensionIDs, count));
                    return guidArray;
                }
            }

            internal static unsafe void EnsureSave(Image image, string filename, Stream dataStream)
            {
                if (!image.RawFormat.Equals((object)ImageFormat.Gif))
                    return;
                bool flag = false;
                int count;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsCount(new HandleRef((object)image, image.nativeImage), out count));
                if (count <= 0)
                    return;
                Span<Guid> span1;
                if (count < 16)
                {
                    int length = count;
                    // ISSUE: untyped stack allocation
                    span1 = new Span<Guid>((void*)__untypedstackalloc(checked(unchecked((IntPtr)(uint)length) * sizeof(Guid))), length);
                }
                else
                    span1 = (Span<Guid>)new Guid[count];
                Span<Guid> span2 = span1;
                fixed (Guid* dimensionIDs = &MemoryMarshal.GetReference<Guid>(span2))
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageGetFrameDimensionsList(new HandleRef((object)image, image.nativeImage), dimensionIDs, count));
                Guid guid = FrameDimension.Time.Guid;
                for (int index = 0; index < count; ++index)
                {
                    if (guid == span2[index])
                    {
                        flag = image.GetFrameCount(FrameDimension.Time) > 1;
                        break;
                    }
                }
                if (!flag)
                    return;
                try
                {
                    Stream stream = (Stream)null;
                    long num = 0;
                    if (dataStream != null)
                    {
                        num = dataStream.Position;
                        dataStream.Position = 0L;
                    }
                    try
                    {
                        if (dataStream == null)
                            stream = dataStream = (Stream)File.OpenRead(filename);
                        image._rawData = new byte[(int)dataStream.Length];
                        dataStream.Read(image._rawData, 0, (int)dataStream.Length);
                    }
                    finally
                    {
                        if (stream != null)
                            stream.Close();
                        else
                            dataStream.Position = num;
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                }
                catch (DirectoryNotFoundException ex)
                {
                }
                catch (IOException ex)
                {
                }
                catch (NotSupportedException ex)
                {
                }
                catch (ObjectDisposedException ex)
                {
                }
                catch (ArgumentException ex)
                {
                }
            }

            public static Image FromFile(string filename, bool useEmbeddedColorManagement)
            {
                if (!File.Exists(filename))
                {
                    filename = Path.GetFullPath(filename);
                    throw new FileNotFoundException(filename);
                }
                filename = Path.GetFullPath(filename);
                IntPtr image = IntPtr.Zero;
                if (useEmbeddedColorManagement)
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipLoadImageFromFileICM(filename, out image));
                else
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipLoadImageFromFile(filename, out image));
                Image.ValidateImage(image);
                Image imageObject = Image.CreateImageObject(image);
                Image.EnsureSave(imageObject, filename, (Stream)null);
                return imageObject;
            }

            public static Image FromStream(
              Stream stream,
              bool useEmbeddedColorManagement,
              bool validateImageData)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                IntPtr image = IntPtr.Zero;
                if (useEmbeddedColorManagement)
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipLoadImageFromStreamICM((Interop.Ole32.IStream)new GPStream(stream), out image));
                else
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipLoadImageFromStream((Interop.Ole32.IStream)new GPStream(stream), out image));
                if (validateImageData)
                    Image.ValidateImage(image);
                Image imageObject = Image.CreateImageObject(image);
                Image.EnsureSave(imageObject, (string)null, stream);
                return imageObject;
            }

            private IntPtr InitializeFromStream(Stream stream)
            {
                IntPtr image = IntPtr.Zero;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipLoadImageFromStream((Interop.Ole32.IStream)new GPStream(stream), out image));
                Image.ValidateImage(image);
                this.nativeImage = image;
                int type = -1;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageType(new HandleRef((object)this, this.nativeImage), out type));
                Image.EnsureSave(this, (string)null, stream);
                return image;
            }

            internal Image(IntPtr nativeImage) => this.SetNativeImage(nativeImage);

            public object Clone()
            {
                IntPtr cloneimage = IntPtr.Zero;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipCloneImage(new HandleRef((object)this, this.nativeImage), out cloneimage));
                Image.ValidateImage(cloneimage);
                return (object)Image.CreateImageObject(cloneimage);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (this.nativeImage == IntPtr.Zero)
                    return;
                try
                {
                    SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef((object)this, this.nativeImage));
                }
                catch (Exception ex)
                {
                    if (!ClientUtils.IsSecurityOrCriticalException(ex))
                        return;
                    throw;
                }
                finally
                {
                    this.nativeImage = IntPtr.Zero;
                }
            }

            internal static Image CreateImageObject(IntPtr nativeImage)
            {
                int type;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageType(new HandleRef((object)null, nativeImage), out type));
                switch ((ImageType)type)
                {
                    case ImageType.Bitmap:
                        return (Image)new Bitmap(nativeImage);
                    case ImageType.Metafile:
                        return (Image)Metafile.FromGDIplus(nativeImage);
                    default:
                        throw new ArgumentException(SR.InvalidImage);
                }
            }

            public EncoderParameters GetEncoderParameterList(Guid encoder)
            {
                int size;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetEncoderParameterListSize(new HandleRef((object)this, this.nativeImage), ref encoder, out size));
                if (size <= 0)
                    return (EncoderParameters)null;
                IntPtr num = Marshal.AllocHGlobal(size);
                try
                {
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetEncoderParameterList(new HandleRef((object)this, this.nativeImage), ref encoder, size, num));
                    return EncoderParameters.ConvertFromMemory(num);
                }
                finally
                {
                    Marshal.FreeHGlobal(num);
                }
            }

            public void Save(string filename, ImageFormat format)
            {
                if (format == null)
                    throw new ArgumentNullException(nameof(format));
                ImageCodecInfo encoder = format.FindEncoder() ?? ImageFormat.Png.FindEncoder();
                this.Save(filename, encoder, (EncoderParameters)null);
            }

            public void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
            {
                if (filename == null)
                    throw new ArgumentNullException(nameof(filename));
                if (encoder == null)
                    throw new ArgumentNullException(nameof(encoder));
                IntPtr num = IntPtr.Zero;
                if (encoderParams != null)
                {
                    this._rawData = (byte[])null;
                    num = encoderParams.ConvertToMemory();
                }
                try
                {
                    Guid clsid = encoder.Clsid;
                    bool flag = false;
                    if (this._rawData != null)
                    {
                        ImageCodecInfo encoder1 = this.RawFormat.FindEncoder();
                        if (encoder1 != null && encoder1.Clsid == clsid)
                        {
                            using (FileStream fileStream = File.OpenWrite(filename))
                            {
                                fileStream.Write(this._rawData, 0, this._rawData.Length);
                                flag = true;
                            }
                        }
                    }
                    if (flag)
                        return;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipSaveImageToFile(new HandleRef((object)this, this.nativeImage), filename, ref clsid, new HandleRef((object)encoderParams, num)));
                }
                finally
                {
                    if (num != IntPtr.Zero)
                        Marshal.FreeHGlobal(num);
                }
            }

            private void Save(MemoryStream stream)
            {
                ImageFormat imageFormat = this.RawFormat;
                if (imageFormat.Guid == ImageFormat.Jpeg.Guid)
                    imageFormat = ImageFormat.Png;
                ImageCodecInfo encoder = imageFormat.FindEncoder() ?? ImageFormat.Png.FindEncoder();
                this.Save((Stream)stream, encoder, (EncoderParameters)null);
            }

            public void Save(Stream stream, ImageFormat format)
            {
                ImageCodecInfo encoder = format != null ? format.FindEncoder() : throw new ArgumentNullException(nameof(format));
                this.Save(stream, encoder, (EncoderParameters)null);
            }

            public void Save(Stream stream, ImageCodecInfo encoder, EncoderParameters encoderParams)
            {
                if (stream == null)
                    throw new ArgumentNullException(nameof(stream));
                if (encoder == null)
                    throw new ArgumentNullException(nameof(encoder));
                IntPtr num = IntPtr.Zero;
                if (encoderParams != null)
                {
                    this._rawData = (byte[])null;
                    num = encoderParams.ConvertToMemory();
                }
                try
                {
                    Guid clsid = encoder.Clsid;
                    bool flag = false;
                    if (this._rawData != null)
                    {
                        ImageCodecInfo encoder1 = this.RawFormat.FindEncoder();
                        if (encoder1 != null && encoder1.Clsid == clsid)
                        {
                            stream.Write(this._rawData, 0, this._rawData.Length);
                            flag = true;
                        }
                    }
                    if (flag)
                        return;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipSaveImageToStream(new HandleRef((object)this, this.nativeImage), (Interop.Ole32.IStream)new GPStream(stream), ref clsid, new HandleRef((object)encoderParams, num)));
                }
                finally
                {
                    if (num != IntPtr.Zero)
                        Marshal.FreeHGlobal(num);
                }
            }

            public void SaveAdd(EncoderParameters encoderParams)
            {
                IntPtr num = IntPtr.Zero;
                if (encoderParams != null)
                    num = encoderParams.ConvertToMemory();
                this._rawData = (byte[])null;
                try
                {
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipSaveAdd(new HandleRef((object)this, this.nativeImage), new HandleRef((object)encoderParams, num)));
                }
                finally
                {
                    if (num != IntPtr.Zero)
                        Marshal.FreeHGlobal(num);
                }
            }

            public void SaveAdd(Image image, EncoderParameters encoderParams)
            {
                IntPtr num = IntPtr.Zero;
                if (image == null)
                    throw new ArgumentNullException(nameof(image));
                if (encoderParams != null)
                    num = encoderParams.ConvertToMemory();
                this._rawData = (byte[])null;
                try
                {
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipSaveAddImage(new HandleRef((object)this, this.nativeImage), new HandleRef((object)image, image.nativeImage), new HandleRef((object)encoderParams, num)));
                }
                finally
                {
                    if (num != IntPtr.Zero)
                        Marshal.FreeHGlobal(num);
                }
            }

            public RectangleF GetBounds(ref GraphicsUnit pageUnit)
            {
                RectangleF gprectf;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageBounds(new HandleRef((object)this, this.nativeImage), out gprectf, out pageUnit));
                return gprectf;
            }

            [Browsable(false)]
            public ColorPalette Palette
            {
                get
                {
                    int size;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImagePaletteSize(new HandleRef((object)this, this.nativeImage), out size));
                    ColorPalette colorPalette = new ColorPalette(size);
                    IntPtr num = Marshal.AllocHGlobal(size);
                    try
                    {
                        SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImagePalette(new HandleRef((object)this, this.nativeImage), num, size));
                        colorPalette.ConvertFromMemory(num);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(num);
                    }
                    return colorPalette;
                }
                set
                {
                    IntPtr memory = value.ConvertToMemory();
                    try
                    {
                        SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipSetImagePalette(new HandleRef((object)this, this.nativeImage), memory));
                    }
                    finally
                    {
                        if (memory != IntPtr.Zero)
                            Marshal.FreeHGlobal(memory);
                    }
                }
            }

            public Image GetThumbnailImage(
              int thumbWidth,
              int thumbHeight,
              Image.GetThumbnailImageAbort callback,
              IntPtr callbackData)
            {
                IntPtr thumbImage = IntPtr.Zero;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetImageThumbnail(new HandleRef((object)this, this.nativeImage), thumbWidth, thumbHeight, out thumbImage, callback, callbackData));
                return Image.CreateImageObject(thumbImage);
            }

            [Browsable(false)]
            public int[] PropertyIdList
            {
                get
                {
                    int count;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetPropertyCount(new HandleRef((object)this, this.nativeImage), out count));
                    int[] list = new int[count];
                    if (count == 0)
                        return list;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetPropertyIdList(new HandleRef((object)this, this.nativeImage), count, list));
                    return list;
                }
            }

            public PropertyItem GetPropertyItem(int propid)
            {
                int size;
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetPropertyItemSize(new HandleRef((object)this, this.nativeImage), propid, out size));
                if (size == 0)
                    return (PropertyItem)null;
                IntPtr num = Marshal.AllocHGlobal(size);
                if (num == IntPtr.Zero)
                    throw SafeNativeMethods.Gdip.StatusException(3);
                try
                {
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetPropertyItem(new HandleRef((object)this, this.nativeImage), propid, size, num));
                    return PropertyItemInternal.ConvertFromMemory(num, 1)[0];
                }
                finally
                {
                    Marshal.FreeHGlobal(num);
                }
            }

            public void SetPropertyItem(PropertyItem propitem)
            {
                PropertyItemInternal propitem1 = PropertyItemInternal.ConvertFromPropertyItem(propitem);
                using (propitem1)
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipSetPropertyItem(new HandleRef((object)this, this.nativeImage), propitem1));
            }

            [Browsable(false)]
            public PropertyItem[] PropertyItems
            {
                get
                {
                    int count;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetPropertyCount(new HandleRef((object)this, this.nativeImage), out count));
                    int totalSize;
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetPropertySize(new HandleRef((object)this, this.nativeImage), out totalSize, ref count));
                    if (totalSize == 0 || count == 0)
                        return Array.Empty<PropertyItem>();
                    IntPtr num = Marshal.AllocHGlobal(totalSize);
                    try
                    {
                        SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipGetAllPropertyItems(new HandleRef((object)this, this.nativeImage), totalSize, count, num));
                        return PropertyItemInternal.ConvertFromMemory(num, count);
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(num);
                    }
                }
            }

            public static int GetPixelFormatSize(PixelFormat pixfmt) => (int)pixfmt >> 8 & (int)byte.MaxValue;

            public static bool IsAlphaPixelFormat(PixelFormat pixfmt) => (uint)(pixfmt & PixelFormat.Alpha) > 0U;

            internal static void ValidateImage(IntPtr image)
            {
                try
                {
                    SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdipImageForceValidation(new HandleRef((object)null, image)));
                }
                catch
                {
                    SafeNativeMethods.Gdip.GdipDisposeImage(new HandleRef((object)null, image));
                    throw;
                }
            }

            public delegate bool GetThumbnailImageAbort();
        }

        public sealed class ImageFormat
        {
            // Format IDs
            // private static ImageFormat undefined = new ImageFormat(new Guid("{b96b3ca9-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_memoryBMP = new ImageFormat(new Guid("{b96b3caa-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_bmp = new ImageFormat(new Guid("{b96b3cab-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_emf = new ImageFormat(new Guid("{b96b3cac-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_wmf = new ImageFormat(new Guid("{b96b3cad-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_jpeg = new ImageFormat(new Guid("{b96b3cae-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_png = new ImageFormat(new Guid("{b96b3caf-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_gif = new ImageFormat(new Guid("{b96b3cb0-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_tiff = new ImageFormat(new Guid("{b96b3cb1-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_exif = new ImageFormat(new Guid("{b96b3cb2-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_photoCD = new ImageFormat(new Guid("{b96b3cb3-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_flashPIX = new ImageFormat(new Guid("{b96b3cb4-0728-11d3-9d7b-0000f81ef32e}"));
            private static ImageFormat s_icon = new ImageFormat(new Guid("{b96b3cb5-0728-11d3-9d7b-0000f81ef32e}"));

            private Guid _guid;

            /// <summary>
            /// Initializes a new instance of the <see cref='ImageFormat'/> class with the specified GUID.
            /// </summary>
            public ImageFormat(Guid guid)
            {
                _guid = guid;
            }

            /// <summary>
            /// Specifies a global unique identifier (GUID) that represents this <see cref='ImageFormat'/>.
            /// </summary>
            public Guid Guid
            {
                get { return _guid; }
            }

            /// <summary>
            /// Specifies a memory bitmap image format.
            /// </summary>
            public static ImageFormat MemoryBmp
            {
                get { return s_memoryBMP; }
            }

            /// <summary>
            /// Specifies the bitmap image format.
            /// </summary>
            public static ImageFormat Bmp
            {
                get { return s_bmp; }
            }

            /// <summary>
            /// Specifies the enhanced Windows metafile image format.
            /// </summary>
            public static ImageFormat Emf
            {
                get { return s_emf; }
            }

            /// <summary>
            /// Specifies the Windows metafile image format.
            /// </summary>
            public static ImageFormat Wmf
            {
                get { return s_wmf; }
            }

            /// <summary>
            /// Specifies the GIF image format.
            /// </summary>
            public static ImageFormat Gif
            {
                get { return s_gif; }
            }

            /// <summary>
            /// Specifies the JPEG image format.
            /// </summary>
            public static ImageFormat Jpeg
            {
                get { return s_jpeg; }
            }

            /// <summary>
            /// Specifies the W3C PNG image format.
            /// </summary>
            public static ImageFormat Png
            {
                get { return s_png; }
            }

            /// <summary>
            /// Specifies the Tag Image File Format (TIFF) image format.
            /// </summary>
            public static ImageFormat Tiff
            {
                get { return s_tiff; }
            }

            /// <summary>
            /// Specifies the Exchangeable Image Format (EXIF).
            /// </summary>
            public static ImageFormat Exif
            {
                get { return s_exif; }
            }

            /// <summary>
            /// Specifies the Windows icon image format.
            /// </summary>
            public static ImageFormat Icon
            {
                get { return s_icon; }
            }

            /// <summary>
            /// Returns a value indicating whether the specified object is an <see cref='ImageFormat'/> equivalent to this
            /// <see cref='ImageFormat'/>.
            /// </summary>
            public override bool Equals(object o)
            {
                ImageFormat format = o as ImageFormat;
                if (format == null)
                    return false;
                return _guid == format._guid;
            }

            /// <summary>
            /// Returns a hash code.
            /// </summary>
            public override int GetHashCode()
            {
                return _guid.GetHashCode();
            }

    #if !FEATURE_PAL        
            // Find any random encoder which supports this format
            internal ImageCodecInfo FindEncoder()
            {
                ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.FormatID.Equals(_guid))
                        return codec;
                }
                return null;
            }
    #endif

            /// <summary>
            /// Converts this <see cref='MapleLib.WzLib.Imaging.ImageFormat'/> to a human-readable string.
            /// </summary>
            public override string ToString()
            {
                if (this.Guid == s_memoryBMP.Guid) return "MemoryBMP";
                if (this.Guid == s_bmp.Guid) return "Bmp";
                if (this.Guid == s_emf.Guid) return "Emf";
                if (this.Guid == s_wmf.Guid) return "Wmf";
                if (this.Guid == s_gif.Guid) return "Gif";
                if (this.Guid == s_jpeg.Guid) return "Jpeg";
                if (this.Guid == s_png.Guid) return "Png";
                if (this.Guid == s_tiff.Guid) return "Tiff";
                if (this.Guid == s_exif.Guid) return "Exif";
                if (this.Guid == s_icon.Guid) return "Icon";
                return "[ImageFormat: " + _guid + "]";
            }
        }

        public sealed class ImageCodecInfo
        {
            private Guid _clsid;
            private Guid _formatID;
            private string _codecName;
            private string _dllName;
            private string _formatDescription;
            private string _filenameExtension;
            private string _mimeType;
            private ImageCodecFlags _flags;
            private int _version;
            private byte[][] _signaturePatterns;
            private byte[][] _signatureMasks;

            internal ImageCodecInfo()
            {
            }

            public Guid Clsid
            {
                get { return _clsid; }
                set { _clsid = value; }
            }

            public Guid FormatID
            {
                get { return _formatID; }
                set { _formatID = value; }
            }

            public string CodecName
            {
                get { return _codecName; }
                set { _codecName = value; }
            }

            public string DllName
            {
                get
                {
                    return _dllName;
                }
                set
                {
                    _dllName = value;
                }
            }

            public string FormatDescription
            {
                get { return _formatDescription; }
                set { _formatDescription = value; }
            }

            public string FilenameExtension
            {
                get { return _filenameExtension; }
                set { _filenameExtension = value; }
            }

            public string MimeType
            {
                get { return _mimeType; }
                set { _mimeType = value; }
            }

            public ImageCodecFlags Flags
            {
                get { return _flags; }
                set { _flags = value; }
            }

            public int Version
            {
                get { return _version; }
                set { _version = value; }
            }

            [CLSCompliant(false)]
            public byte[][] SignaturePatterns
            {
                get { return _signaturePatterns; }
                set { _signaturePatterns = value; }
            }

            [CLSCompliant(false)]
            public byte[][] SignatureMasks
            {
                get { return _signatureMasks; }
                set { _signatureMasks = value; }
            }

            // Encoder/Decoder selection APIs

            public static ImageCodecInfo[] GetImageDecoders()
            {
                ImageCodecInfo[] imageCodecs;
                int numDecoders;
                int size;

                int status = Gdip.GdipGetImageDecodersSize(out numDecoders, out size);

                if (status != Gdip.Ok)
                {
                    throw Gdip.StatusException(status);
                }

                IntPtr memory = Marshal.AllocHGlobal(size);

                try
                {
                    status = Gdip.GdipGetImageDecoders(numDecoders, size, memory);

                    if (status != Gdip.Ok)
                    {
                        throw Gdip.StatusException(status);
                    }

                    imageCodecs = ImageCodecInfo.ConvertFromMemory(memory, numDecoders);
                }
                finally
                {
                    Marshal.FreeHGlobal(memory);
                }

                return imageCodecs;
            }

            public static ImageCodecInfo[] GetImageEncoders()
            {
                ImageCodecInfo[] imageCodecs;
                int numEncoders;
                int size;

                int status = Gdip.GdipGetImageEncodersSize(out numEncoders, out size);

                if (status != Gdip.Ok)
                {
                    throw Gdip.StatusException(status);
                }

                IntPtr memory = Marshal.AllocHGlobal(size);

                try
                {
                    status = Gdip.GdipGetImageEncoders(numEncoders, size, memory);

                    if (status != Gdip.Ok)
                    {
                        throw Gdip.StatusException(status);
                    }

                    imageCodecs = ImageCodecInfo.ConvertFromMemory(memory, numEncoders);
                }
                finally
                {
                    Marshal.FreeHGlobal(memory);
                }

                return imageCodecs;
            }

            private static ImageCodecInfo[] ConvertFromMemory(IntPtr memoryStart, int numCodecs)
            {
                ImageCodecInfo[] codecs = new ImageCodecInfo[numCodecs];

                int index;

                for (index = 0; index < numCodecs; index++)
                {
                    IntPtr curcodec = (IntPtr)((long)memoryStart + (int)Marshal.SizeOf(typeof(ImageCodecInfoPrivate)) * index);
                    ImageCodecInfoPrivate codecp = new ImageCodecInfoPrivate();
                    Marshal.PtrToStructure(curcodec, codecp);

                    codecs[index] = new ImageCodecInfo();
                    codecs[index].Clsid = codecp.Clsid;
                    codecs[index].FormatID = codecp.FormatID;
                    codecs[index].CodecName = Marshal.PtrToStringUni(codecp.CodecName);
                    codecs[index].DllName = Marshal.PtrToStringUni(codecp.DllName);
                    codecs[index].FormatDescription = Marshal.PtrToStringUni(codecp.FormatDescription);
                    codecs[index].FilenameExtension = Marshal.PtrToStringUni(codecp.FilenameExtension);
                    codecs[index].MimeType = Marshal.PtrToStringUni(codecp.MimeType);

                    codecs[index].Flags = (ImageCodecFlags)codecp.Flags;
                    codecs[index].Version = (int)codecp.Version;

                    codecs[index].SignaturePatterns = new byte[codecp.SigCount][];
                    codecs[index].SignatureMasks = new byte[codecp.SigCount][];

                    for (int j = 0; j < codecp.SigCount; j++)
                    {
                        codecs[index].SignaturePatterns[j] = new byte[codecp.SigSize];
                        codecs[index].SignatureMasks[j] = new byte[codecp.SigSize];

                        Marshal.Copy((IntPtr)((long)codecp.SigMask + j * codecp.SigSize), codecs[index].SignatureMasks[j], 0, codecp.SigSize);
                        Marshal.Copy((IntPtr)((long)codecp.SigPattern + j * codecp.SigSize), codecs[index].SignaturePatterns[j], 0, codecp.SigSize);
                    }
                }

                return codecs;
            }
        }

        [Flags]
        public enum ImageCodecFlags
        {
            Encoder = 0x00000001,
            Decoder = 0x00000002,
            SupportBitmap = 0x00000004,
            SupportVector = 0x00000008,
            SeekableEncode = 0x00000010,
            BlockingDecode = 0x00000020,
            Builtin = 0x00010000,
            System = 0x00020000,
            User = 0x00040000
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        internal class ImageCodecInfoPrivate
        {
    #pragma warning disable CS0618 // Legacy code: We don't care about using obsolete API's.
            [MarshalAs(UnmanagedType.Struct)]
    #pragma warning restore CS0618
            public Guid Clsid;
    #pragma warning disable CS0618 // Legacy code: We don't care about using obsolete API's.
            [MarshalAs(UnmanagedType.Struct)]
    #pragma warning restore CS0618
            public Guid FormatID;

            public IntPtr CodecName = IntPtr.Zero;
            public IntPtr DllName = IntPtr.Zero;
            public IntPtr FormatDescription = IntPtr.Zero;
            public IntPtr FilenameExtension = IntPtr.Zero;
            public IntPtr MimeType = IntPtr.Zero;

            public int Flags;
            public int Version;
            public int SigCount;
            public int SigSize;

            public IntPtr SigPattern = IntPtr.Zero;
            public IntPtr SigMask = IntPtr.Zero;
        }

        internal static class Gdip
        {
            private static readonly TraceSwitch s_gdiPlusInitialization = new TraceSwitch("GdiPlusInitialization", "Tracks GDI+ initialization and teardown");
            private static IntPtr s_initToken;
            private const string ThreadDataSlotName = "MapleLib.WzLib.threaddata";
            internal const int Ok = 0;
            internal const int GenericError = 1;
            internal const int InvalidParameter = 2;
            internal const int OutOfMemory = 3;
            internal const int ObjectBusy = 4;
            internal const int InsufficientBuffer = 5;
            internal const int NotImplemented = 6;
            internal const int Win32Error = 7;
            internal const int WrongState = 8;
            internal const int Aborted = 9;
            internal const int FileNotFound = 10;
            internal const int ValueOverflow = 11;
            internal const int AccessDenied = 12;
            internal const int UnknownImageFormat = 13;
            internal const int FontFamilyNotFound = 14;
            internal const int FontStyleNotFound = 15;
            internal const int NotTrueTypeFont = 16;
            internal const int UnsupportedGdiplusVersion = 17;
            internal const int GdiplusNotInitialized = 18;
            internal const int PropertyNotFound = 19;
            internal const int PropertyNotSupported = 20;
            private const string LibraryName = "gdiplus.dll";

            internal static extern int GdipBeginContainer(
              HandleRef graphics,
              ref RectangleF dstRect,
              ref RectangleF srcRect,
              GraphicsUnit unit,
              out int state);

            internal static extern int GdipBeginContainer2(HandleRef graphics, out int state);

            internal static extern int GdipBeginContainerI(
              HandleRef graphics,
              ref Rectangle dstRect,
              ref Rectangle srcRect,
              GraphicsUnit unit,
              out int state);

            internal static extern int GdipEndContainer(HandleRef graphics, int state);

            internal static extern int GdipCreateAdjustableArrowCap(
              float height,
              float Width,
              bool isFilled,
              out IntPtr adjustableArrowCap);

            internal static extern int GdipGetAdjustableArrowCapHeight(
              HandleRef adjustableArrowCap,
              out float height);

            internal static extern int GdipSetAdjustableArrowCapHeight(
              HandleRef adjustableArrowCap,
              float height);

            internal static extern int GdipSetAdjustableArrowCapWidth(
              HandleRef adjustableArrowCap,
              float Width);

            internal static extern int GdipGetAdjustableArrowCapWidth(
              HandleRef adjustableArrowCap,
              out float Width);

            internal static extern int GdipSetAdjustableArrowCapMiddleInset(
              HandleRef adjustableArrowCap,
              float middleInset);

            internal static extern int GdipGetAdjustableArrowCapMiddleInset(
              HandleRef adjustableArrowCap,
              out float middleInset);

            internal static extern int GdipSetAdjustableArrowCapFillState(
              HandleRef adjustableArrowCap,
              bool fillState);

            internal static extern int GdipGetAdjustableArrowCapFillState(
              HandleRef adjustableArrowCap,
              out bool fillState);

            internal static extern int GdipGetCustomLineCapType(
              HandleRef customCap,
              out CustomLineCapType capType);

            internal static extern int GdipCreateCustomLineCap(
              HandleRef fillpath,
              HandleRef strokepath,
              LineCap baseCap,
              float baseInset,
              out IntPtr customCap);

            internal static extern int GdipDeleteCustomLineCap(HandleRef customCap);

            internal static extern int GdipCloneCustomLineCap(HandleRef customCap, out IntPtr clonedCap);

            internal static extern int GdipSetCustomLineCapStrokeCaps(
              HandleRef customCap,
              LineCap startCap,
              LineCap endCap);

            internal static extern int GdipGetCustomLineCapStrokeCaps(
              HandleRef customCap,
              out LineCap startCap,
              out LineCap endCap);

            internal static extern int GdipSetCustomLineCapStrokeJoin(
              HandleRef customCap,
              LineJoin lineJoin);

            internal static extern int GdipGetCustomLineCapStrokeJoin(
              HandleRef customCap,
              out LineJoin lineJoin);

            internal static extern int GdipSetCustomLineCapBaseCap(HandleRef customCap, LineCap baseCap);

            internal static extern int GdipGetCustomLineCapBaseCap(
              HandleRef customCap,
              out LineCap baseCap);

            internal static extern int GdipSetCustomLineCapBaseInset(HandleRef customCap, float inset);

            internal static extern int GdipGetCustomLineCapBaseInset(HandleRef customCap, out float inset);

            internal static extern int GdipSetCustomLineCapWidthScale(
              HandleRef customCap,
              float widthScale);

            internal static extern int GdipGetCustomLineCapWidthScale(
              HandleRef customCap,
              out float widthScale);

            internal static extern int GdipCreatePathIter(out IntPtr pathIter, HandleRef path);

            internal static extern int GdipDeletePathIter(HandleRef pathIter);

            internal static extern int GdipPathIterNextSubpath(
              HandleRef pathIter,
              out int resultCount,
              out int startIndex,
              out int endIndex,
              out bool isClosed);

            internal static extern int GdipPathIterNextSubpathPath(
              HandleRef pathIter,
              out int resultCount,
              HandleRef path,
              out bool isClosed);

            internal static extern int GdipPathIterNextPathType(
              HandleRef pathIter,
              out int resultCount,
              out byte pathType,
              out int startIndex,
              out int endIndex);

            internal static extern int GdipPathIterNextMarker(
              HandleRef pathIter,
              out int resultCount,
              out int startIndex,
              out int endIndex);

            internal static extern int GdipPathIterNextMarkerPath(
              HandleRef pathIter,
              out int resultCount,
              HandleRef path);

            internal static extern int GdipPathIterGetCount(HandleRef pathIter, out int count);

            internal static extern int GdipPathIterGetSubpathCount(HandleRef pathIter, out int count);

            internal static extern int GdipPathIterHasCurve(HandleRef pathIter, out bool hasCurve);

            internal static extern int GdipPathIterRewind(HandleRef pathIter);

            internal static extern unsafe int GdipPathIterEnumerate(
              HandleRef pathIter,
              out int resultCount,
              PointF* points,
              byte* types,
              int count);

            internal static extern unsafe int GdipPathIterCopyData(
              HandleRef pathIter,
              out int resultCount,
              PointF* points,
              byte* types,
              int startIndex,
              int endIndex);

            internal static extern int GdipCreateHatchBrush(
              int hatchstyle,
              int forecol,
              int backcol,
              out IntPtr brush);

            internal static extern int GdipGetHatchStyle(HandleRef brush, out int hatchstyle);

            internal static extern int GdipGetHatchForegroundColor(HandleRef brush, out int forecol);

            internal static extern int GdipGetHatchBackgroundColor(HandleRef brush, out int backcol);

            internal static extern int GdipCloneBrush(HandleRef brush, out IntPtr clonebrush);

            internal static extern int GdipCreateLineBrush(
              ref PointF point1,
              ref PointF point2,
              int color1,
              int color2,
              WrapMode wrapMode,
              out IntPtr lineGradient);

            internal static extern int GdipCreateLineBrushI(
              ref Point point1,
              ref Point point2,
              int color1,
              int color2,
              WrapMode wrapMode,
              out IntPtr lineGradient);

            internal static extern int GdipCreateLineBrushFromRect(
              ref RectangleF rect,
              int color1,
              int color2,
              LinearGradientMode lineGradientMode,
              WrapMode wrapMode,
              out IntPtr lineGradient);

            internal static extern int GdipCreateLineBrushFromRectI(
              ref Rectangle rect,
              int color1,
              int color2,
              LinearGradientMode lineGradientMode,
              WrapMode wrapMode,
              out IntPtr lineGradient);

            internal static extern int GdipCreateLineBrushFromRectWithAngle(
              ref RectangleF rect,
              int color1,
              int color2,
              float angle,
              bool isAngleScaleable,
              WrapMode wrapMode,
              out IntPtr lineGradient);

            internal static extern int GdipCreateLineBrushFromRectWithAngleI(
              ref Rectangle rect,
              int color1,
              int color2,
              float angle,
              bool isAngleScaleable,
              WrapMode wrapMode,
              out IntPtr lineGradient);

            internal static extern int GdipSetLineColors(HandleRef brush, int color1, int color2);

            internal static extern int GdipGetLineColors(HandleRef brush, int[] colors);

            internal static extern int GdipGetLineRect(HandleRef brush, out RectangleF gprectf);

            internal static extern int GdipGetLineGammaCorrection(
              HandleRef brush,
              out bool useGammaCorrection);

            internal static extern int GdipSetLineGammaCorrection(
              HandleRef brush,
              bool useGammaCorrection);

            internal static extern int GdipSetLineSigmaBlend(HandleRef brush, float focus, float scale);

            internal static extern int GdipSetLineLinearBlend(HandleRef brush, float focus, float scale);

            internal static extern int GdipGetLineBlendCount(HandleRef brush, out int count);

            internal static extern int GdipGetLineBlend(
              HandleRef brush,
              IntPtr blend,
              IntPtr positions,
              int count);

            internal static extern int GdipSetLineBlend(
              HandleRef brush,
              HandleRef blend,
              HandleRef positions,
              int count);

            internal static extern int GdipGetLinePresetBlendCount(HandleRef brush, out int count);

            internal static extern int GdipGetLinePresetBlend(
              HandleRef brush,
              IntPtr blend,
              IntPtr positions,
              int count);

            internal static extern int GdipSetLinePresetBlend(
              HandleRef brush,
              HandleRef blend,
              HandleRef positions,
              int count);

            internal static extern int GdipSetLineWrapMode(HandleRef brush, int wrapMode);

            internal static extern int GdipGetLineWrapMode(HandleRef brush, out int wrapMode);

            internal static extern int GdipResetLineTransform(HandleRef brush);

            internal static extern int GdipMultiplyLineTransform(
              HandleRef brush,
              HandleRef matrix,
              MatrixOrder order);

            internal static extern int GdipGetLineTransform(HandleRef brush, HandleRef matrix);

            internal static extern int GdipSetLineTransform(HandleRef brush, HandleRef matrix);

            internal static extern int GdipTranslateLineTransform(
              HandleRef brush,
              float dx,
              float dy,
              MatrixOrder order);

            internal static extern int GdipScaleLineTransform(
              HandleRef brush,
              float sx,
              float sy,
              MatrixOrder order);

            internal static extern int GdipRotateLineTransform(
              HandleRef brush,
              float angle,
              MatrixOrder order);

            internal static extern unsafe int GdipCreatePathGradient(
              PointF* points,
              int count,
              WrapMode wrapMode,
              out IntPtr brush);

            internal static extern unsafe int GdipCreatePathGradientI(
              Point* points,
              int count,
              WrapMode wrapMode,
              out IntPtr brush);

            internal static extern int GdipCreatePathGradientFromPath(HandleRef path, out IntPtr brush);

            internal static extern int GdipGetPathGradientCenterColor(HandleRef brush, out int color);

            internal static extern int GdipSetPathGradientCenterColor(HandleRef brush, int color);

            internal static extern int GdipGetPathGradientSurroundColorsWithCount(
              HandleRef brush,
              int[] color,
              ref int count);

            internal static extern int GdipSetPathGradientSurroundColorsWithCount(
              HandleRef brush,
              int[] argb,
              ref int count);

            internal static extern int GdipGetPathGradientCenterPoint(HandleRef brush, out PointF point);

            internal static extern int GdipSetPathGradientCenterPoint(HandleRef brush, ref PointF point);

            internal static extern int GdipGetPathGradientRect(HandleRef brush, out RectangleF gprectf);

            internal static extern int GdipGetPathGradientPointCount(HandleRef brush, out int count);

            internal static extern int GdipGetPathGradientSurroundColorCount(
              HandleRef brush,
              out int count);

            internal static extern int GdipGetPathGradientBlendCount(HandleRef brush, out int count);

            internal static extern int GdipGetPathGradientBlend(
              HandleRef brush,
              float[] blend,
              float[] positions,
              int count);

            internal static extern int GdipSetPathGradientBlend(
              HandleRef brush,
              HandleRef blend,
              HandleRef positions,
              int count);

            internal static extern int GdipGetPathGradientPresetBlendCount(HandleRef brush, out int count);

            internal static extern int GdipGetPathGradientPresetBlend(
              HandleRef brush,
              int[] blend,
              float[] positions,
              int count);

            internal static extern int GdipSetPathGradientPresetBlend(
              HandleRef brush,
              int[] blend,
              float[] positions,
              int count);

            internal static extern int GdipSetPathGradientSigmaBlend(
              HandleRef brush,
              float focus,
              float scale);

            internal static extern int GdipSetPathGradientLinearBlend(
              HandleRef brush,
              float focus,
              float scale);

            internal static extern int GdipSetPathGradientWrapMode(HandleRef brush, int wrapmode);

            internal static extern int GdipGetPathGradientWrapMode(HandleRef brush, out int wrapmode);

            internal static extern int GdipSetPathGradientTransform(HandleRef brush, HandleRef matrix);

            internal static extern int GdipGetPathGradientTransform(HandleRef brush, HandleRef matrix);

            internal static extern int GdipResetPathGradientTransform(HandleRef brush);

            internal static extern int GdipMultiplyPathGradientTransform(
              HandleRef brush,
              HandleRef matrix,
              MatrixOrder order);

            internal static extern int GdipTranslatePathGradientTransform(
              HandleRef brush,
              float dx,
              float dy,
              MatrixOrder order);

            internal static extern int GdipScalePathGradientTransform(
              HandleRef brush,
              float sx,
              float sy,
              MatrixOrder order);

            internal static extern int GdipRotatePathGradientTransform(
              HandleRef brush,
              float angle,
              MatrixOrder order);

            internal static extern int GdipGetPathGradientFocusScales(
              HandleRef brush,
              float[] xScale,
              float[] yScale);

            internal static extern int GdipSetPathGradientFocusScales(
              HandleRef brush,
              float xScale,
              float yScale);

            internal static extern int GdipCreateImageAttributes(out IntPtr imageattr);

            internal static extern int GdipCloneImageAttributes(
              HandleRef imageattr,
              out IntPtr cloneImageattr);

            internal static extern int GdipDisposeImageAttributes(HandleRef imageattr);

            internal static extern int GdipSetImageAttributesColorMatrix(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag,
              ColorMatrix colorMatrix,
              ColorMatrix grayMatrix,
              ColorMatrixFlag flags);

            internal static extern int GdipSetImageAttributesThreshold(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag,
              float threshold);

            internal static extern int GdipSetImageAttributesGamma(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag,
              float gamma);

            internal static extern int GdipSetImageAttributesNoOp(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag);

            internal static extern int GdipSetImageAttributesColorKeys(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag,
              int colorLow,
              int colorHigh);

            internal static extern int GdipSetImageAttributesOutputChannel(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag,
              ColorChannelFlag flags);

            internal static extern int GdipSetImageAttributesOutputChannelColorProfile(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag,
              string colorProfileFilename);

            internal static extern int GdipSetImageAttributesRemapTable(
              HandleRef imageattr,
              ColorAdjustType type,
              bool enableFlag,
              int mapSize,
              HandleRef map);

            internal static extern int GdipSetImageAttributesWrapMode(
              HandleRef imageattr,
              int wrapmode,
              int argb,
              bool clamp);

            internal static extern int GdipGetImageAttributesAdjustedPalette(
              HandleRef imageattr,
              HandleRef palette,
              ColorAdjustType type);

            internal static extern int GdipGetImageDecodersSize(out int numDecoders, out int size);

            internal static extern int GdipGetImageDecoders(int numDecoders, int size, IntPtr decoders);

            internal static extern int GdipGetImageEncodersSize(out int numEncoders, out int size);

            internal static extern int GdipGetImageEncoders(int numEncoders, int size, IntPtr encoders);

            internal static extern int GdipCreateSolidFill(int color, out IntPtr brush);

            internal static extern int GdipSetSolidFillColor(HandleRef brush, int color);

            internal static extern int GdipGetSolidFillColor(HandleRef brush, out int color);

            internal static extern int GdipCreateTexture(
              HandleRef bitmap,
              int wrapmode,
              out IntPtr texture);

            internal static extern int GdipCreateTexture2(
              HandleRef bitmap,
              int wrapmode,
              float x,
              float y,
              float Width,
              float height,
              out IntPtr texture);

            internal static extern int GdipCreateTextureIA(
              HandleRef bitmap,
              HandleRef imageAttrib,
              float x,
              float y,
              float Width,
              float height,
              out IntPtr texture);

            internal static extern int GdipCreateTexture2I(
              HandleRef bitmap,
              int wrapmode,
              int x,
              int y,
              int Width,
              int height,
              out IntPtr texture);

            internal static extern int GdipCreateTextureIAI(
              HandleRef bitmap,
              HandleRef imageAttrib,
              int x,
              int y,
              int Width,
              int height,
              out IntPtr texture);

            internal static extern int GdipSetTextureTransform(HandleRef brush, HandleRef matrix);

            internal static extern int GdipGetTextureTransform(HandleRef brush, HandleRef matrix);

            internal static extern int GdipResetTextureTransform(HandleRef brush);

            internal static extern int GdipMultiplyTextureTransform(
              HandleRef brush,
              HandleRef matrix,
              MatrixOrder order);

            internal static extern int GdipTranslateTextureTransform(
              HandleRef brush,
              float dx,
              float dy,
              MatrixOrder order);

            internal static extern int GdipScaleTextureTransform(
              HandleRef brush,
              float sx,
              float sy,
              MatrixOrder order);

            internal static extern int GdipRotateTextureTransform(
              HandleRef brush,
              float angle,
              MatrixOrder order);

            internal static extern int GdipSetTextureWrapMode(HandleRef brush, int wrapMode);

            internal static extern int GdipGetTextureWrapMode(HandleRef brush, out int wrapMode);

            internal static extern int GdipGetTextureImage(HandleRef brush, out IntPtr image);

            internal static extern int GdipGetFontCollectionFamilyCount(
              HandleRef fontCollection,
              out int numFound);

            internal static extern int GdipGetFontCollectionFamilyList(
              HandleRef fontCollection,
              int numSought,
              IntPtr[] gpfamilies,
              out int numFound);

            internal static extern int GdipCloneFontFamily(
              HandleRef fontfamily,
              out IntPtr clonefontfamily);

            internal static extern int GdipCreateFontFamilyFromName(
              string name,
              HandleRef fontCollection,
              out IntPtr FontFamily);

            internal static extern int GdipGetGenericFontFamilySansSerif(out IntPtr fontfamily);

            internal static extern int GdipGetGenericFontFamilySerif(out IntPtr fontfamily);

            internal static extern int GdipGetGenericFontFamilyMonospace(out IntPtr fontfamily);

            internal static extern int GdipDeleteFontFamily(HandleRef fontFamily);

            internal static extern unsafe int GdipGetFamilyName(
              HandleRef family,
              char* name,
              int language);

            internal static extern int GdipIsStyleAvailable(
              HandleRef family,
              FontStyle style,
              out int isStyleAvailable);

            internal static extern int GdipGetEmHeight(
              HandleRef family,
              FontStyle style,
              out int EmHeight);

            internal static extern int GdipGetCellAscent(
              HandleRef family,
              FontStyle style,
              out int CellAscent);

            internal static extern int GdipGetCellDescent(
              HandleRef family,
              FontStyle style,
              out int CellDescent);

            internal static extern int GdipGetLineSpacing(
              HandleRef family,
              FontStyle style,
              out int LineSpaceing);

            internal static extern int GdipNewInstalledFontCollection(out IntPtr fontCollection);

            internal static extern int GdipNewPrivateFontCollection(out IntPtr fontCollection);

            internal static extern int GdipDeletePrivateFontCollection(ref IntPtr fontCollection);

            internal static extern int GdipPrivateAddFontFile(HandleRef fontCollection, string filename);

            internal static extern int GdipPrivateAddMemoryFont(
              HandleRef fontCollection,
              HandleRef memory,
              int length);

            internal static extern int GdipCreateFont(
              HandleRef fontFamily,
              float emSize,
              FontStyle style,
              GraphicsUnit unit,
              out IntPtr font);

            internal static extern int GdipCloneFont(HandleRef font, out IntPtr cloneFont);

            internal static extern int GdipDeleteFont(HandleRef font);

            internal static extern int GdipGetFamily(HandleRef font, out IntPtr family);

            internal static extern int GdipGetFontStyle(HandleRef font, out FontStyle style);

            internal static extern int GdipGetFontSize(HandleRef font, out float size);

            internal static extern int GdipGetFontHeight(
              HandleRef font,
              HandleRef graphics,
              out float size);

            internal static extern int GdipGetFontHeightGivenDPI(
              HandleRef font,
              float dpi,
              out float size);

            internal static extern int GdipGetFontUnit(HandleRef font, out GraphicsUnit unit);

            internal static extern int GdipGetLogFontW(
              HandleRef font,
              HandleRef graphics,
              ref SafeNativeMethods.LOGFONT lf);

            internal static extern int GdipCreatePen1(int argb, float Width, int unit, out IntPtr pen);

            internal static extern int GdipCreatePen2(
              HandleRef brush,
              float Width,
              int unit,
              out IntPtr pen);

            internal static extern int GdipClonePen(HandleRef pen, out IntPtr clonepen);

            internal static extern int GdipDeletePen(HandleRef Pen);

            internal static extern int GdipSetPenMode(HandleRef pen, PenAlignment penAlign);

            internal static extern int GdipGetPenMode(HandleRef pen, out PenAlignment penAlign);

            internal static extern int GdipSetPenWidth(HandleRef pen, float Width);

            internal static extern int GdipGetPenWidth(HandleRef pen, float[] Width);

            internal static extern int GdipSetPenLineCap197819(
              HandleRef pen,
              int startCap,
              int endCap,
              int dashCap);

            internal static extern int GdipSetPenStartCap(HandleRef pen, int startCap);

            internal static extern int GdipSetPenEndCap(HandleRef pen, int endCap);

            internal static extern int GdipGetPenStartCap(HandleRef pen, out int startCap);

            internal static extern int GdipGetPenEndCap(HandleRef pen, out int endCap);

            internal static extern int GdipGetPenDashCap197819(HandleRef pen, out int dashCap);

            internal static extern int GdipSetPenDashCap197819(HandleRef pen, int dashCap);

            internal static extern int GdipSetPenLineJoin(HandleRef pen, int lineJoin);

            internal static extern int GdipGetPenLineJoin(HandleRef pen, out int lineJoin);

            internal static extern int GdipSetPenCustomStartCap(HandleRef pen, HandleRef customCap);

            internal static extern int GdipGetPenCustomStartCap(HandleRef pen, out IntPtr customCap);

            internal static extern int GdipSetPenCustomEndCap(HandleRef pen, HandleRef customCap);

            internal static extern int GdipGetPenCustomEndCap(HandleRef pen, out IntPtr customCap);

            internal static extern int GdipSetPenMiterLimit(HandleRef pen, float miterLimit);

            internal static extern int GdipGetPenMiterLimit(HandleRef pen, float[] miterLimit);

            internal static extern int GdipSetPenTransform(HandleRef pen, HandleRef matrix);

            internal static extern int GdipGetPenTransform(HandleRef pen, HandleRef matrix);

            internal static extern int GdipResetPenTransform(HandleRef brush);

            internal static extern int GdipMultiplyPenTransform(
              HandleRef brush,
              HandleRef matrix,
              MatrixOrder order);

            internal static extern int GdipTranslatePenTransform(
              HandleRef brush,
              float dx,
              float dy,
              MatrixOrder order);

            internal static extern int GdipScalePenTransform(
              HandleRef brush,
              float sx,
              float sy,
              MatrixOrder order);

            internal static extern int GdipRotatePenTransform(
              HandleRef brush,
              float angle,
              MatrixOrder order);

            internal static extern int GdipSetPenColor(HandleRef pen, int argb);

            internal static extern int GdipGetPenColor(HandleRef pen, out int argb);

            internal static extern int GdipSetPenBrushFill(HandleRef pen, HandleRef brush);

            internal static extern int GdipGetPenBrushFill(HandleRef pen, out IntPtr brush);

            internal static extern int GdipGetPenFillType(HandleRef pen, out int pentype);

            internal static extern int GdipGetPenDashStyle(HandleRef pen, out int dashstyle);

            internal static extern int GdipSetPenDashStyle(HandleRef pen, int dashstyle);

            internal static extern int GdipSetPenDashArray(
              HandleRef pen,
              HandleRef memorydash,
              int count);

            internal static extern int GdipGetPenDashOffset(HandleRef pen, float[] dashoffset);

            internal static extern int GdipSetPenDashOffset(HandleRef pen, float dashoffset);

            internal static extern int GdipGetPenDashCount(HandleRef pen, out int dashcount);

            internal static extern int GdipGetPenDashArray(HandleRef pen, float[] memorydash, int count);

            internal static extern int GdipGetPenCompoundCount(HandleRef pen, out int count);

            internal static extern int GdipSetPenCompoundArray(HandleRef pen, float[] array, int count);

            internal static extern int GdipGetPenCompoundArray(HandleRef pen, float[] array, int count);

            internal static extern int GdipSetWorldTransform(HandleRef graphics, HandleRef matrix);

            internal static extern int GdipResetWorldTransform(HandleRef graphics);

            internal static extern int GdipMultiplyWorldTransform(
              HandleRef graphics,
              HandleRef matrix,
              MatrixOrder order);

            internal static extern int GdipTranslateWorldTransform(
              HandleRef graphics,
              float dx,
              float dy,
              MatrixOrder order);

            internal static extern int GdipScaleWorldTransform(
              HandleRef graphics,
              float sx,
              float sy,
              MatrixOrder order);

            internal static extern int GdipRotateWorldTransform(
              HandleRef graphics,
              float angle,
              MatrixOrder order);

            internal static extern int GdipGetWorldTransform(HandleRef graphics, HandleRef matrix);

            internal static extern int GdipSetCompositingMode(
              HandleRef graphics,
              CompositingMode compositingMode);

            internal static extern int GdipSetTextRenderingHint(
              HandleRef graphics,
              TextRenderingHint textRenderingHint);

            internal static extern int GdipSetTextContrast(HandleRef graphics, int textContrast);

            internal static extern int GdipSetInterpolationMode(
              HandleRef graphics,
              InterpolationMode interpolationMode);

            internal static extern int GdipGetCompositingMode(
              HandleRef graphics,
              out CompositingMode compositingMode);

            internal static extern int GdipSetRenderingOrigin(HandleRef graphics, int x, int y);

            internal static extern int GdipGetRenderingOrigin(HandleRef graphics, out int x, out int y);

            internal static extern int GdipSetCompositingQuality(
              HandleRef graphics,
              CompositingQuality quality);

            internal static extern int GdipGetCompositingQuality(
              HandleRef graphics,
              out CompositingQuality quality);

            internal static extern int GdipSetSmoothingMode(
              HandleRef graphics,
              SmoothingMode smoothingMode);

            internal static extern int GdipGetSmoothingMode(
              HandleRef graphics,
              out SmoothingMode smoothingMode);

            internal static extern int GdipSetPixelOffsetMode(
              HandleRef graphics,
              PixelOffsetMode pixelOffsetMode);

            internal static extern int GdipGetPixelOffsetMode(
              HandleRef graphics,
              out PixelOffsetMode pixelOffsetMode);

            internal static extern int GdipGetTextRenderingHint(
              HandleRef graphics,
              out TextRenderingHint textRenderingHint);

            internal static extern int GdipGetTextContrast(HandleRef graphics, out int textContrast);

            internal static extern int GdipGetInterpolationMode(
              HandleRef graphics,
              out InterpolationMode interpolationMode);

            internal static extern int GdipGetPageUnit(HandleRef graphics, out GraphicsUnit unit);

            internal static extern int GdipGetPageScale(HandleRef graphics, out float scale);

            internal static extern int GdipSetPageUnit(HandleRef graphics, GraphicsUnit unit);

            internal static extern int GdipSetPageScale(HandleRef graphics, float scale);

            internal static extern int GdipGetDpiX(HandleRef graphics, out float dpi);

            internal static extern int GdipGetDpiY(HandleRef graphics, out float dpi);

            internal static extern int GdipCreateMatrix(out IntPtr matrix);

            internal static extern int GdipCreateMatrix2(
              float m11,
              float m12,
              float m21,
              float m22,
              float dx,
              float dy,
              out IntPtr matrix);

            internal static extern unsafe int GdipCreateMatrix3(
              ref RectangleF rect,
              PointF* dstplg,
              out IntPtr matrix);

            internal static extern unsafe int GdipCreateMatrix3I(
              ref Rectangle rect,
              Point* dstplg,
              out IntPtr matrix);

            internal static extern int GdipCloneMatrix(HandleRef matrix, out IntPtr cloneMatrix);

            internal static extern int GdipDeleteMatrix(HandleRef matrix);

            internal static extern int GdipSetMatrixElements(
              HandleRef matrix,
              float m11,
              float m12,
              float m21,
              float m22,
              float dx,
              float dy);

            internal static extern int GdipMultiplyMatrix(
              HandleRef matrix,
              HandleRef matrix2,
              MatrixOrder order);

            internal static extern int GdipTranslateMatrix(
              HandleRef matrix,
              float offsetX,
              float offsetY,
              MatrixOrder order);

            internal static extern int GdipScaleMatrix(
              HandleRef matrix,
              float scaleX,
              float scaleY,
              MatrixOrder order);

            internal static extern int GdipRotateMatrix(HandleRef matrix, float angle, MatrixOrder order);

            internal static extern int GdipShearMatrix(
              HandleRef matrix,
              float shearX,
              float shearY,
              MatrixOrder order);

            internal static extern int GdipInvertMatrix(HandleRef matrix);

            internal static extern unsafe int GdipTransformMatrixPoints(
              HandleRef matrix,
              PointF* pts,
              int count);

            internal static extern unsafe int GdipTransformMatrixPointsI(
              HandleRef matrix,
              Point* pts,
              int count);

            internal static extern unsafe int GdipVectorTransformMatrixPoints(
              HandleRef matrix,
              PointF* pts,
              int count);

            internal static extern unsafe int GdipVectorTransformMatrixPointsI(
              HandleRef matrix,
              Point* pts,
              int count);

            internal static extern int GdipGetMatrixElements(HandleRef matrix, IntPtr m);

            internal static extern int GdipIsMatrixInvertible(HandleRef matrix, out int boolean);

            internal static extern int GdipIsMatrixIdentity(HandleRef matrix, out int boolean);

            internal static extern int GdipIsMatrixEqual(
              HandleRef matrix,
              HandleRef matrix2,
              out int boolean);

            internal static extern int GdipCreateRegion(out IntPtr region);

            internal static extern int GdipCreateRegionRect(ref RectangleF gprectf, out IntPtr region);

            internal static extern int GdipCreateRegionRectI(ref Rectangle gprect, out IntPtr region);

            internal static extern int GdipCreateRegionPath(HandleRef path, out IntPtr region);

            internal static extern int GdipCreateRegionRgnData(
              byte[] rgndata,
              int size,
              out IntPtr region);

            internal static extern int GdipCreateRegionHrgn(HandleRef hRgn, out IntPtr region);

            internal static extern int GdipCloneRegion(HandleRef region, out IntPtr cloneregion);

            internal static extern int GdipDeleteRegion(HandleRef region);

            internal static extern int GdipFillRegion(
              HandleRef graphics,
              HandleRef brush,
              HandleRef region);

            internal static extern int GdipSetInfinite(HandleRef region);

            internal static extern int GdipSetEmpty(HandleRef region);

            internal static extern int GdipCombineRegionRect(
              HandleRef region,
              ref RectangleF gprectf,
              CombineMode mode);

            internal static extern int GdipCombineRegionRectI(
              HandleRef region,
              ref Rectangle gprect,
              CombineMode mode);

            internal static extern int GdipCombineRegionPath(
              HandleRef region,
              HandleRef path,
              CombineMode mode);

            internal static extern int GdipCombineRegionRegion(
              HandleRef region,
              HandleRef region2,
              CombineMode mode);

            internal static extern int GdipTranslateRegion(HandleRef region, float dx, float dy);

            internal static extern int GdipTranslateRegionI(HandleRef region, int dx, int dy);

            internal static extern int GdipTransformRegion(HandleRef region, HandleRef matrix);

            internal static extern int GdipGetRegionBounds(
              HandleRef region,
              HandleRef graphics,
              out RectangleF gprectf);

            internal static extern int GdipGetRegionHRgn(
              HandleRef region,
              HandleRef graphics,
              out IntPtr hrgn);

            internal static extern int GdipIsEmptyRegion(
              HandleRef region,
              HandleRef graphics,
              out int boolean);

            internal static extern int GdipIsInfiniteRegion(
              HandleRef region,
              HandleRef graphics,
              out int boolean);

            internal static extern int GdipIsEqualRegion(
              HandleRef region,
              HandleRef region2,
              HandleRef graphics,
              out int boolean);

            internal static extern int GdipGetRegionDataSize(HandleRef region, out int bufferSize);

            internal static extern int GdipGetRegionData(
              HandleRef region,
              byte[] regionData,
              int bufferSize,
              out int sizeFilled);

            internal static extern int GdipIsVisibleRegionPoint(
              HandleRef region,
              float X,
              float Y,
              HandleRef graphics,
              out int boolean);

            internal static extern int GdipIsVisibleRegionPointI(
              HandleRef region,
              int X,
              int Y,
              HandleRef graphics,
              out int boolean);

            internal static extern int GdipIsVisibleRegionRect(
              HandleRef region,
              float X,
              float Y,
              float Width,
              float height,
              HandleRef graphics,
              out int boolean);

            internal static extern int GdipIsVisibleRegionRectI(
              HandleRef region,
              int X,
              int Y,
              int Width,
              int height,
              HandleRef graphics,
              out int boolean);

            internal static extern int GdipGetRegionScansCount(
              HandleRef region,
              out int count,
              HandleRef matrix);

            internal static extern unsafe int GdipGetRegionScans(
              HandleRef region,
              RectangleF* rects,
              out int count,
              HandleRef matrix);

            internal static extern int GdipSetClipGraphics(
              HandleRef graphics,
              HandleRef srcgraphics,
              CombineMode mode);

            internal static extern int GdipSetClipRect(
              HandleRef graphics,
              float x,
              float y,
              float Width,
              float height,
              CombineMode mode);

            internal static extern int GdipSetClipRectI(
              HandleRef graphics,
              int x,
              int y,
              int Width,
              int height,
              CombineMode mode);

            internal static extern int GdipSetClipPath(
              HandleRef graphics,
              HandleRef path,
              CombineMode mode);

            internal static extern int GdipSetClipRegion(
              HandleRef graphics,
              HandleRef region,
              CombineMode mode);

            internal static extern int GdipResetClip(HandleRef graphics);

            internal static extern int GdipTranslateClip(HandleRef graphics, float dx, float dy);

            internal static extern int GdipGetClip(HandleRef graphics, HandleRef region);

            internal static extern int GdipGetClipBounds(HandleRef graphics, out RectangleF rect);

            internal static extern int GdipIsClipEmpty(HandleRef graphics, out bool result);

            internal static extern int GdipGetVisibleClipBounds(HandleRef graphics, out RectangleF rect);

            internal static extern int GdipIsVisibleClipEmpty(HandleRef graphics, out bool result);

            internal static extern int GdipIsVisiblePoint(
              HandleRef graphics,
              float x,
              float y,
              out bool result);

            internal static extern int GdipIsVisiblePointI(
              HandleRef graphics,
              int x,
              int y,
              out bool result);

            internal static extern int GdipIsVisibleRect(
              HandleRef graphics,
              float x,
              float y,
              float Width,
              float height,
              out bool result);

            internal static extern int GdipIsVisibleRectI(
              HandleRef graphics,
              int x,
              int y,
              int Width,
              int height,
              out bool result);

            internal static extern int GdipFlush(HandleRef graphics, FlushIntention intention);

            internal static extern int GdipGetDC(HandleRef graphics, out IntPtr hdc);

            internal static extern int GdipSetStringFormatMeasurableCharacterRanges(
              HandleRef format,
              int rangeCount,
              [In, Out] CharacterRange[] range);

            internal static extern int GdipCreateStringFormat(
              StringFormatFlags options,
              int language,
              out IntPtr format);

            internal static extern int GdipStringFormatGetGenericDefault(out IntPtr format);

            internal static extern int GdipStringFormatGetGenericTypographic(out IntPtr format);

            internal static extern int GdipDeleteStringFormat(HandleRef format);

            internal static extern int GdipCloneStringFormat(HandleRef format, out IntPtr newFormat);

            internal static extern int GdipSetStringFormatFlags(
              HandleRef format,
              StringFormatFlags options);

            internal static extern int GdipGetStringFormatFlags(
              HandleRef format,
              out StringFormatFlags result);

            internal static extern int GdipSetStringFormatAlign(HandleRef format, StringAlignment align);

            internal static extern int GdipGetStringFormatAlign(
              HandleRef format,
              out StringAlignment align);

            internal static extern int GdipSetStringFormatLineAlign(
              HandleRef format,
              StringAlignment align);

            internal static extern int GdipGetStringFormatLineAlign(
              HandleRef format,
              out StringAlignment align);

            internal static extern int GdipSetStringFormatHotkeyPrefix(
              HandleRef format,
              HotkeyPrefix hotkeyPrefix);

            internal static extern int GdipGetStringFormatHotkeyPrefix(
              HandleRef format,
              out HotkeyPrefix hotkeyPrefix);

            internal static extern int GdipSetStringFormatTabStops(
              HandleRef format,
              float firstTabOffset,
              int count,
              float[] tabStops);

            internal static extern int GdipGetStringFormatTabStops(
              HandleRef format,
              int count,
              out float firstTabOffset,
              [In, Out] float[] tabStops);

            internal static extern int GdipGetStringFormatTabStopCount(HandleRef format, out int count);

            internal static extern int GdipGetStringFormatMeasurableCharacterRangeCount(
              HandleRef format,
              out int count);

            internal static extern int GdipSetStringFormatTrimming(
              HandleRef format,
              StringTrimming trimming);

            internal static extern int GdipGetStringFormatTrimming(
              HandleRef format,
              out StringTrimming trimming);

            internal static extern int GdipSetStringFormatDigitSubstitution(
              HandleRef format,
              int langID,
              StringDigitSubstitute sds);

            internal static extern int GdipGetStringFormatDigitSubstitution(
              HandleRef format,
              out int langID,
              out StringDigitSubstitute sds);

            internal static extern int GdipGetImageDimension(
              HandleRef image,
              out float Width,
              out float height);

            internal static extern int GdipGetImageWidth(HandleRef image, out int Width);

            internal static extern int GdipGetImageHeight(HandleRef image, out int height);

            internal static extern int GdipGetImageHorizontalResolution(
              HandleRef image,
              out float horzRes);

            internal static extern int GdipGetImageVerticalResolution(HandleRef image, out float vertRes);

            internal static extern int GdipGetImageFlags(HandleRef image, out int flags);

            internal static extern int GdipGetImageRawFormat(HandleRef image, ref Guid format);

            internal static extern int GdipGetImagePixelFormat(HandleRef image, out PixelFormat format);

            internal static extern int GdipImageGetFrameCount(
              HandleRef image,
              ref Guid dimensionID,
              out int count);

            internal static extern int GdipImageSelectActiveFrame(
              HandleRef image,
              ref Guid dimensionID,
              int frameIndex);

            internal static extern int GdipImageRotateFlip(HandleRef image, int rotateFlipType);

            internal static extern int GdipRemovePropertyItem(HandleRef image, int propid);

            internal static extern int GdipCreateBitmapFromFile(string filename, out IntPtr bitmap);

            internal static extern int GdipCreateBitmapFromFileICM(string filename, out IntPtr bitmap);

            internal static extern int GdipCreateBitmapFromScan0(
              int Width,
              int height,
              int stride,
              int format,
              HandleRef scan0,
              out IntPtr bitmap);

            internal static extern int GdipCreateBitmapFromGraphics(
              int Width,
              int height,
              HandleRef graphics,
              out IntPtr bitmap);

            internal static extern int GdipCreateBitmapFromHBITMAP(
              HandleRef hbitmap,
              HandleRef hpalette,
              out IntPtr bitmap);

            internal static extern int GdipCreateBitmapFromHICON(HandleRef hicon, out IntPtr bitmap);

            internal static extern int GdipCreateBitmapFromResource(
              HandleRef hresource,
              HandleRef name,
              out IntPtr bitmap);

            internal static extern int GdipCreateHBITMAPFromBitmap(
              HandleRef nativeBitmap,
              out IntPtr hbitmap,
              int argbBackground);

            internal static extern int GdipCreateHICONFromBitmap(HandleRef nativeBitmap, out IntPtr hicon);

            internal static extern int GdipCloneBitmapArea(
              float x,
              float y,
              float Width,
              float height,
              int format,
              HandleRef srcbitmap,
              out IntPtr dstbitmap);

            internal static extern int GdipCloneBitmapAreaI(
              int x,
              int y,
              int Width,
              int height,
              int format,
              HandleRef srcbitmap,
              out IntPtr dstbitmap);

            internal static extern int GdipBitmapLockBits(
              HandleRef bitmap,
              ref Rectangle rect,
              ImageLockMode flags,
              PixelFormat format,
              [In, Out] BitmapData lockedBitmapData);

            internal static extern int GdipBitmapUnlockBits(HandleRef bitmap, BitmapData lockedBitmapData);

            internal static extern int GdipBitmapGetPixel(HandleRef bitmap, int x, int y, out int argb);

            internal static extern int GdipBitmapSetPixel(HandleRef bitmap, int x, int y, int argb);

            internal static extern int GdipBitmapSetResolution(HandleRef bitmap, float dpix, float dpiy);

            internal static extern int GdipImageGetFrameDimensionsCount(HandleRef image, out int count);

            internal static extern unsafe int GdipImageGetFrameDimensionsList(
              HandleRef image,
              Guid* dimensionIDs,
              int count);

            static Gdip()
            {
                SafeNativeMethods.Gdip.PlatformInitialize();
                SafeNativeMethods.StartupInput input = SafeNativeMethods.StartupInput.GetDefault();
                SafeNativeMethods.Gdip.CheckStatus(SafeNativeMethods.Gdip.GdiplusStartup(out SafeNativeMethods.Gdip.s_initToken, ref input, out SafeNativeMethods.StartupOutput _));
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(SafeNativeMethods.Gdip.OnProcessExit);
            }

            internal static bool Initialized => SafeNativeMethods.Gdip.s_initToken != IntPtr.Zero;

            internal static IDictionary ThreadData
            {
                get
                {
                    LocalDataStoreSlot namedDataSlot = Thread.GetNamedDataSlot("MapleLib.WzLib.threaddata");
                    IDictionary dictionary = (IDictionary)Thread.GetData(namedDataSlot);
                    if (dictionary == null)
                    {
                        dictionary = (IDictionary)new Hashtable();
                        Thread.SetData(namedDataSlot, (object)dictionary);
                    }
                    return dictionary;
                }
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            private static void ClearThreadData() => Thread.SetData(Thread.GetNamedDataSlot("MapleLib.WzLib.threaddata"), (object)null);

            private static void Shutdown()
            {
                if (!SafeNativeMethods.Gdip.Initialized)
                    return;
                SafeNativeMethods.Gdip.ClearThreadData();
                AppDomain currentDomain = AppDomain.CurrentDomain;
                currentDomain.ProcessExit -= new EventHandler(SafeNativeMethods.Gdip.OnProcessExit);
                if (currentDomain.IsDefaultAppDomain())
                    return;
                currentDomain.DomainUnload -= new EventHandler(SafeNativeMethods.Gdip.OnProcessExit);
            }

            [PrePrepareMethod]
            private static void OnProcessExit(object sender, EventArgs e) => SafeNativeMethods.Gdip.Shutdown();

            internal static void DummyFunction()
            {
            }

            internal static void CheckStatus(int status)
            {
                if (status != 0)
                    throw SafeNativeMethods.Gdip.StatusException(status);
            }

            internal static Exception StatusException(int status)
            {
                switch (status)
                {
                    case 1:
                        return (Exception)new ExternalException(SR.GdiplusGenericError, -2147467259);
                    case 2:
                        return (Exception)new ArgumentException(SR.GdiplusInvalidParameter);
                    case 3:
                        return (Exception)new OutOfMemoryException(SR.GdiplusOutOfMemory);
                    case 4:
                        return (Exception)new InvalidOperationException(SR.GdiplusObjectBusy);
                    case 5:
                        return (Exception)new OutOfMemoryException(SR.GdiplusInsufficientBuffer);
                    case 6:
                        return (Exception)new NotImplementedException(SR.GdiplusNotImplemented);
                    case 7:
                        return (Exception)new ExternalException(SR.GdiplusGenericError, -2147467259);
                    case 8:
                        return (Exception)new InvalidOperationException(SR.GdiplusWrongState);
                    case 9:
                        return (Exception)new ExternalException(SR.GdiplusAborted, -2147467260);
                    case 10:
                        return (Exception)new FileNotFoundException(SR.GdiplusFileNotFound);
                    case 11:
                        return (Exception)new OverflowException(SR.GdiplusOverflow);
                    case 12:
                        return (Exception)new ExternalException(SR.GdiplusAccessDenied, -2147024891);
                    case 13:
                        return (Exception)new ArgumentException(SR.GdiplusUnknownImageFormat);
                    case 14:
                        return (Exception)new ArgumentException(SR.Format(SR.GdiplusFontFamilyNotFound, (object)"?"));
                    case 15:
                        return (Exception)new ArgumentException(SR.Format(SR.GdiplusFontStyleNotFound, (object)"?", (object)"?"));
                    case 16:
                        return (Exception)new ArgumentException(SR.GdiplusNotTrueTypeFont_NoName);
                    case 17:
                        return (Exception)new ExternalException(SR.GdiplusUnsupportedGdiplusVersion, -2147467259);
                    case 18:
                        return (Exception)new ExternalException(SR.GdiplusNotInitialized, -2147467259);
                    case 19:
                        return (Exception)new ArgumentException(SR.GdiplusPropertyNotFoundError);
                    case 20:
                        return (Exception)new ArgumentException(SR.GdiplusPropertyNotSupportedError);
                    default:
                        return (Exception)new ExternalException(string.Format("{0} [{1}]", (object)SR.GdiplusUnknown, (object)status), -2147418113);
                }
            }

            private static void PlatformInitialize()
            {
            }

            private static extern int GdiplusStartup(
              out IntPtr token,
              ref SafeNativeMethods.StartupInput input,
              out SafeNativeMethods.StartupOutput output);

            internal static extern int GdipCreatePath(int brushMode, out IntPtr path);

            internal static extern unsafe int GdipCreatePath2(
              PointF* points,
              byte* types,
              int count,
              int brushMode,
              out IntPtr path);

            internal static extern unsafe int GdipCreatePath2I(
              Point* points,
              byte* types,
              int count,
              int brushMode,
              out IntPtr path);

            internal static extern int GdipClonePath(HandleRef path, out IntPtr clonepath);

            internal static extern int GdipDeletePath(HandleRef path);

            internal static extern int GdipResetPath(HandleRef path);

            internal static extern int GdipGetPointCount(HandleRef path, out int count);

            internal static extern int GdipGetPathTypes(HandleRef path, byte[] types, int count);

            internal static extern unsafe int GdipGetPathPoints(
              HandleRef path,
              PointF* points,
              int count);

            internal static extern int GdipGetPathFillMode(HandleRef path, out FillMode fillmode);

            internal static extern int GdipSetPathFillMode(HandleRef path, FillMode fillmode);

            internal static extern unsafe int GdipGetPathData(HandleRef path, GpPathData* pathData);

            internal static extern int GdipStartPathFigure(HandleRef path);

            internal static extern int GdipClosePathFigure(HandleRef path);

            internal static extern int GdipClosePathFigures(HandleRef path);

            internal static extern int GdipSetPathMarker(HandleRef path);

            internal static extern int GdipClearPathMarkers(HandleRef path);

            internal static extern int GdipReversePath(HandleRef path);

            internal static extern int GdipGetPathLastPoint(HandleRef path, out PointF lastPoint);

            internal static extern int GdipAddPathLine(
              HandleRef path,
              float x1,
              float y1,
              float x2,
              float y2);

            internal static extern unsafe int GdipAddPathLine2(HandleRef path, PointF* points, int count);

            internal static extern int GdipAddPathArc(
              HandleRef path,
              float x,
              float y,
              float Width,
              float height,
              float startAngle,
              float sweepAngle);

            internal static extern int GdipAddPathBezier(
              HandleRef path,
              float x1,
              float y1,
              float x2,
              float y2,
              float x3,
              float y3,
              float x4,
              float y4);

            internal static extern unsafe int GdipAddPathBeziers(
              HandleRef path,
              PointF* points,
              int count);

            internal static extern unsafe int GdipAddPathCurve(HandleRef path, PointF* points, int count);

            internal static extern unsafe int GdipAddPathCurve2(
              HandleRef path,
              PointF* points,
              int count,
              float tension);

            internal static extern unsafe int GdipAddPathCurve3(
              HandleRef path,
              PointF* points,
              int count,
              int offset,
              int numberOfSegments,
              float tension);

            internal static extern unsafe int GdipAddPathClosedCurve(
              HandleRef path,
              PointF* points,
              int count);

            internal static extern unsafe int GdipAddPathClosedCurve2(
              HandleRef path,
              PointF* points,
              int count,
              float tension);

            internal static extern int GdipAddPathRectangle(
              HandleRef path,
              float x,
              float y,
              float Width,
              float height);

            internal static extern unsafe int GdipAddPathRectangles(
              HandleRef path,
              RectangleF* rects,
              int count);

            internal static extern int GdipAddPathEllipse(
              HandleRef path,
              float x,
              float y,
              float Width,
              float height);

            internal static extern int GdipAddPathPie(
              HandleRef path,
              float x,
              float y,
              float Width,
              float height,
              float startAngle,
              float sweepAngle);

            internal static extern unsafe int GdipAddPathPolygon(
              HandleRef path,
              PointF* points,
              int count);

            internal static extern int GdipAddPathPath(
              HandleRef path,
              HandleRef addingPath,
              bool connect);

            internal static extern int GdipAddPathString(
              HandleRef path,
              string s,
              int length,
              HandleRef fontFamily,
              int style,
              float emSize,
              ref RectangleF layoutRect,
              HandleRef format);

            internal static extern int GdipAddPathStringI(
              HandleRef path,
              string s,
              int length,
              HandleRef fontFamily,
              int style,
              float emSize,
              ref Rectangle layoutRect,
              HandleRef format);

            internal static extern int GdipAddPathLineI(HandleRef path, int x1, int y1, int x2, int y2);

            internal static extern unsafe int GdipAddPathLine2I(HandleRef path, Point* points, int count);

            internal static extern int GdipAddPathArcI(
              HandleRef path,
              int x,
              int y,
              int Width,
              int height,
              float startAngle,
              float sweepAngle);

            internal static extern int GdipAddPathBezierI(
              HandleRef path,
              int x1,
              int y1,
              int x2,
              int y2,
              int x3,
              int y3,
              int x4,
              int y4);

            internal static extern unsafe int GdipAddPathBeziersI(
              HandleRef path,
              Point* points,
              int count);

            internal static extern unsafe int GdipAddPathCurveI(HandleRef path, Point* points, int count);

            internal static extern unsafe int GdipAddPathCurve2I(
              HandleRef path,
              Point* points,
              int count,
              float tension);

            internal static extern unsafe int GdipAddPathCurve3I(
              HandleRef path,
              Point* points,
              int count,
              int offset,
              int numberOfSegments,
              float tension);

            internal static extern unsafe int GdipAddPathClosedCurveI(
              HandleRef path,
              Point* points,
              int count);

            internal static extern unsafe int GdipAddPathClosedCurve2I(
              HandleRef path,
              Point* points,
              int count,
              float tension);

            internal static extern int GdipAddPathRectangleI(
              HandleRef path,
              int x,
              int y,
              int Width,
              int height);

            internal static extern unsafe int GdipAddPathRectanglesI(
              HandleRef path,
              Rectangle* rects,
              int count);

            internal static extern int GdipAddPathEllipseI(
              HandleRef path,
              int x,
              int y,
              int Width,
              int height);

            internal static extern int GdipAddPathPieI(
              HandleRef path,
              int x,
              int y,
              int Width,
              int height,
              float startAngle,
              float sweepAngle);

            internal static extern unsafe int GdipAddPathPolygonI(
              HandleRef path,
              Point* points,
              int count);

            internal static extern int GdipFlattenPath(
              HandleRef path,
              HandleRef matrixfloat,
              float flatness);

            internal static extern int GdipWidenPath(
              HandleRef path,
              HandleRef pen,
              HandleRef matrix,
              float flatness);

            internal static extern unsafe int GdipWarpPath(
              HandleRef path,
              HandleRef matrix,
              PointF* points,
              int count,
              float srcX,
              float srcY,
              float srcWidth,
              float srcHeight,
              WarpMode warpMode,
              float flatness);

            internal static extern int GdipTransformPath(HandleRef path, HandleRef matrix);

            internal static extern int GdipGetPathWorldBounds(
              HandleRef path,
              out RectangleF gprectf,
              HandleRef matrix,
              HandleRef pen);

            internal static extern int GdipIsVisiblePathPoint(
              HandleRef path,
              float x,
              float y,
              HandleRef graphics,
              out bool result);

            internal static extern int GdipIsVisiblePathPointI(
              HandleRef path,
              int x,
              int y,
              HandleRef graphics,
              out bool result);

            internal static extern int GdipIsOutlineVisiblePathPoint(
              HandleRef path,
              float x,
              float y,
              HandleRef pen,
              HandleRef graphics,
              out bool result);

            internal static extern int GdipIsOutlineVisiblePathPointI(
              HandleRef path,
              int x,
              int y,
              HandleRef pen,
              HandleRef graphics,
              out bool result);

            internal static extern int GdipDeleteBrush(HandleRef brush);

            internal static extern int GdipLoadImageFromStream(
              Interop.Ole32.IStream stream,
              out IntPtr image);

            internal static extern int GdipLoadImageFromFile(string filename, out IntPtr image);

            internal static extern int GdipLoadImageFromStreamICM(
              Interop.Ole32.IStream stream,
              out IntPtr image);

            internal static extern int GdipLoadImageFromFileICM(string filename, out IntPtr image);

            internal static extern int GdipCloneImage(HandleRef image, out IntPtr cloneimage);

            internal static extern int GdipDisposeImage(HandleRef image);

            internal static extern int GdipSaveImageToFile(
              HandleRef image,
              string filename,
              ref Guid classId,
              HandleRef encoderParams);

            internal static extern int GdipSaveImageToStream(
              HandleRef image,
              Interop.Ole32.IStream stream,
              ref Guid classId,
              HandleRef encoderParams);

            internal static extern int GdipSaveAdd(HandleRef image, HandleRef encoderParams);

            internal static extern int GdipSaveAddImage(
              HandleRef image,
              HandleRef newImage,
              HandleRef encoderParams);

            internal static extern int GdipGetImageGraphicsContext(HandleRef image, out IntPtr graphics);

            internal static extern int GdipGetImageBounds(
              HandleRef image,
              out RectangleF gprectf,
              out GraphicsUnit unit);

            internal static extern int GdipGetImageType(HandleRef image, out int type);

            internal static extern int GdipGetImageThumbnail(
              HandleRef image,
              int thumbWidth,
              int thumbHeight,
              out IntPtr thumbImage,
              Image.GetThumbnailImageAbort callback,
              IntPtr callbackdata);

            internal static extern int GdipGetEncoderParameterListSize(
              HandleRef image,
              ref Guid clsid,
              out int size);

            internal static extern int GdipGetEncoderParameterList(
              HandleRef image,
              ref Guid clsid,
              int size,
              IntPtr buffer);

            internal static extern int GdipGetImagePalette(HandleRef image, IntPtr palette, int size);

            internal static extern int GdipSetImagePalette(HandleRef image, IntPtr palette);

            internal static extern int GdipGetImagePaletteSize(HandleRef image, out int size);

            internal static extern int GdipGetPropertyCount(HandleRef image, out int count);

            internal static extern int GdipGetPropertyIdList(HandleRef image, int count, int[] list);

            internal static extern int GdipGetPropertyItemSize(HandleRef image, int propid, out int size);

            internal static extern int GdipGetPropertyItem(
              HandleRef image,
              int propid,
              int size,
              IntPtr buffer);

            internal static extern int GdipGetPropertySize(
              HandleRef image,
              out int totalSize,
              ref int count);

            internal static extern int GdipGetAllPropertyItems(
              HandleRef image,
              int totalSize,
              int count,
              IntPtr buffer);

            internal static extern int GdipSetPropertyItem(HandleRef image, PropertyItemInternal propitem);

            internal static extern int GdipImageForceValidation(HandleRef image);

            internal static extern int GdipCreateFromHDC(HandleRef hdc, out IntPtr graphics);

            internal static extern int GdipCreateFromHDC2(
              HandleRef hdc,
              HandleRef hdevice,
              out IntPtr graphics);

            internal static extern int GdipCreateFromHWND(HandleRef hwnd, out IntPtr graphics);

            internal static extern int GdipDeleteGraphics(HandleRef graphics);

            internal static extern int GdipReleaseDC(HandleRef graphics, HandleRef hdc);

            internal static extern unsafe int GdipTransformPoints(
              HandleRef graphics,
              int destSpace,
              int srcSpace,
              PointF* points,
              int count);

            internal static extern unsafe int GdipTransformPointsI(
              HandleRef graphics,
              int destSpace,
              int srcSpace,
              Point* points,
              int count);

            internal static extern int GdipGetNearestColor(HandleRef graphics, ref int color);

            internal static extern IntPtr GdipCreateHalftonePalette();

            internal static extern int GdipDrawLine(
              HandleRef graphics,
              HandleRef pen,
              float x1,
              float y1,
              float x2,
              float y2);

            internal static extern int GdipDrawLineI(
              HandleRef graphics,
              HandleRef pen,
              int x1,
              int y1,
              int x2,
              int y2);

            internal static extern unsafe int GdipDrawLines(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count);

            internal static extern unsafe int GdipDrawLinesI(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count);

            internal static extern int GdipDrawArc(
              HandleRef graphics,
              HandleRef pen,
              float x,
              float y,
              float Width,
              float height,
              float startAngle,
              float sweepAngle);

            internal static extern int GdipDrawArcI(
              HandleRef graphics,
              HandleRef pen,
              int x,
              int y,
              int Width,
              int height,
              float startAngle,
              float sweepAngle);

            internal static extern int GdipDrawBezier(
              HandleRef graphics,
              HandleRef pen,
              float x1,
              float y1,
              float x2,
              float y2,
              float x3,
              float y3,
              float x4,
              float y4);

            internal static extern unsafe int GdipDrawBeziers(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count);

            internal static extern unsafe int GdipDrawBeziersI(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count);

            internal static extern int GdipDrawRectangle(
              HandleRef graphics,
              HandleRef pen,
              float x,
              float y,
              float Width,
              float height);

            internal static extern int GdipDrawRectangleI(
              HandleRef graphics,
              HandleRef pen,
              int x,
              int y,
              int Width,
              int height);

            internal static extern unsafe int GdipDrawRectangles(
              HandleRef graphics,
              HandleRef pen,
              RectangleF* rects,
              int count);

            internal static extern unsafe int GdipDrawRectanglesI(
              HandleRef graphics,
              HandleRef pen,
              Rectangle* rects,
              int count);

            internal static extern int GdipDrawEllipse(
              HandleRef graphics,
              HandleRef pen,
              float x,
              float y,
              float Width,
              float height);

            internal static extern int GdipDrawEllipseI(
              HandleRef graphics,
              HandleRef pen,
              int x,
              int y,
              int Width,
              int height);

            internal static extern int GdipDrawPie(
              HandleRef graphics,
              HandleRef pen,
              float x,
              float y,
              float Width,
              float height,
              float startAngle,
              float sweepAngle);

            internal static extern int GdipDrawPieI(
              HandleRef graphics,
              HandleRef pen,
              int x,
              int y,
              int Width,
              int height,
              float startAngle,
              float sweepAngle);

            internal static extern unsafe int GdipDrawPolygon(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count);

            internal static extern unsafe int GdipDrawPolygonI(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count);

            internal static extern int GdipDrawPath(HandleRef graphics, HandleRef pen, HandleRef path);

            internal static extern unsafe int GdipDrawCurve(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count);

            internal static extern unsafe int GdipDrawCurveI(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count);

            internal static extern unsafe int GdipDrawCurve2(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count,
              float tension);

            internal static extern unsafe int GdipDrawCurve2I(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count,
              float tension);

            internal static extern unsafe int GdipDrawCurve3(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count,
              int offset,
              int numberOfSegments,
              float tension);

            internal static extern unsafe int GdipDrawCurve3I(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count,
              int offset,
              int numberOfSegments,
              float tension);

            internal static extern unsafe int GdipDrawClosedCurve(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count);

            internal static extern unsafe int GdipDrawClosedCurveI(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count);

            internal static extern unsafe int GdipDrawClosedCurve2(
              HandleRef graphics,
              HandleRef pen,
              PointF* points,
              int count,
              float tension);

            internal static extern unsafe int GdipDrawClosedCurve2I(
              HandleRef graphics,
              HandleRef pen,
              Point* points,
              int count,
              float tension);

            internal static extern int GdipGraphicsClear(HandleRef graphics, int argb);

            internal static extern int GdipFillRectangle(
              HandleRef graphics,
              HandleRef brush,
              float x,
              float y,
              float Width,
              float height);

            internal static extern int GdipFillRectangleI(
              HandleRef graphics,
              HandleRef brush,
              int x,
              int y,
              int Width,
              int height);

            internal static extern unsafe int GdipFillRectangles(
              HandleRef graphics,
              HandleRef brush,
              RectangleF* rects,
              int count);

            internal static extern unsafe int GdipFillRectanglesI(
              HandleRef graphics,
              HandleRef brush,
              Rectangle* rects,
              int count);

            internal static extern unsafe int GdipFillPolygon(
              HandleRef graphics,
              HandleRef brush,
              PointF* points,
              int count,
              FillMode brushMode);

            internal static extern unsafe int GdipFillPolygonI(
              HandleRef graphics,
              HandleRef brush,
              Point* points,
              int count,
              FillMode brushMode);

            internal static extern int GdipFillEllipse(
              HandleRef graphics,
              HandleRef brush,
              float x,
              float y,
              float Width,
              float height);

            internal static extern int GdipFillEllipseI(
              HandleRef graphics,
              HandleRef brush,
              int x,
              int y,
              int Width,
              int height);

            internal static extern int GdipFillPie(
              HandleRef graphics,
              HandleRef brush,
              float x,
              float y,
              float Width,
              float height,
              float startAngle,
              float sweepAngle);

            internal static extern int GdipFillPieI(
              HandleRef graphics,
              HandleRef brush,
              int x,
              int y,
              int Width,
              int height,
              float startAngle,
              float sweepAngle);

            internal static extern int GdipFillPath(HandleRef graphics, HandleRef brush, HandleRef path);

            internal static extern unsafe int GdipFillClosedCurve(
              HandleRef graphics,
              HandleRef brush,
              PointF* points,
              int count);

            internal static extern unsafe int GdipFillClosedCurveI(
              HandleRef graphics,
              HandleRef brush,
              Point* points,
              int count);

            internal static extern unsafe int GdipFillClosedCurve2(
              HandleRef graphics,
              HandleRef brush,
              PointF* points,
              int count,
              float tension,
              FillMode mode);

            internal static extern unsafe int GdipFillClosedCurve2I(
              HandleRef graphics,
              HandleRef brush,
              Point* points,
              int count,
              float tension,
              FillMode mode);

            internal static extern int GdipDrawImage(
              HandleRef graphics,
              HandleRef image,
              float x,
              float y);

            internal static extern int GdipDrawImageI(HandleRef graphics, HandleRef image, int x, int y);

            internal static extern int GdipDrawImageRect(
              HandleRef graphics,
              HandleRef image,
              float x,
              float y,
              float Width,
              float height);

            internal static extern int GdipDrawImageRectI(
              HandleRef graphics,
              HandleRef image,
              int x,
              int y,
              int Width,
              int height);

            internal static extern unsafe int GdipDrawImagePoints(
              HandleRef graphics,
              HandleRef image,
              PointF* points,
              int count);

            internal static extern unsafe int GdipDrawImagePointsI(
              HandleRef graphics,
              HandleRef image,
              Point* points,
              int count);

            internal static extern int GdipDrawImagePointRect(
              HandleRef graphics,
              HandleRef image,
              float x,
              float y,
              float srcx,
              float srcy,
              float srcwidth,
              float srcheight,
              int srcunit);

            internal static extern int GdipDrawImagePointRectI(
              HandleRef graphics,
              HandleRef image,
              int x,
              int y,
              int srcx,
              int srcy,
              int srcwidth,
              int srcheight,
              int srcunit);

            internal static extern int GdipDrawImageRectRect(
              HandleRef graphics,
              HandleRef image,
              float dstx,
              float dsty,
              float dstwidth,
              float dstheight,
              float srcx,
              float srcy,
              float srcwidth,
              float srcheight,
              GraphicsUnit srcunit,
              HandleRef imageAttributes,
              Graphics.DrawImageAbort callback,
              HandleRef callbackdata);

            internal static extern int GdipDrawImageRectRectI(
              HandleRef graphics,
              HandleRef image,
              int dstx,
              int dsty,
              int dstwidth,
              int dstheight,
              int srcx,
              int srcy,
              int srcwidth,
              int srcheight,
              GraphicsUnit srcunit,
              HandleRef imageAttributes,
              Graphics.DrawImageAbort callback,
              HandleRef callbackdata);

            internal static extern unsafe int GdipDrawImagePointsRect(
              HandleRef graphics,
              HandleRef image,
              PointF* points,
              int count,
              float srcx,
              float srcy,
              float srcwidth,
              float srcheight,
              GraphicsUnit srcunit,
              HandleRef imageAttributes,
              Graphics.DrawImageAbort callback,
              HandleRef callbackdata);

            internal static extern unsafe int GdipDrawImagePointsRectI(
              HandleRef graphics,
              HandleRef image,
              Point* points,
              int count,
              int srcx,
              int srcy,
              int srcwidth,
              int srcheight,
              GraphicsUnit srcunit,
              HandleRef imageAttributes,
              Graphics.DrawImageAbort callback,
              HandleRef callbackdata);

            internal static extern int GdipEnumerateMetafileDestPoint(
              HandleRef graphics,
              HandleRef metafile,
              ref PointF destPoint,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipEnumerateMetafileDestPointI(
              HandleRef graphics,
              HandleRef metafile,
              ref Point destPoint,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipEnumerateMetafileDestRect(
              HandleRef graphics,
              HandleRef metafile,
              ref RectangleF destRect,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipEnumerateMetafileDestRectI(
              HandleRef graphics,
              HandleRef metafile,
              ref Rectangle destRect,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern unsafe int GdipEnumerateMetafileDestPoints(
              HandleRef graphics,
              HandleRef metafile,
              PointF* destPoints,
              int count,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern unsafe int GdipEnumerateMetafileDestPointsI(
              HandleRef graphics,
              HandleRef metafile,
              Point* destPoints,
              int count,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipEnumerateMetafileSrcRectDestPoint(
              HandleRef graphics,
              HandleRef metafile,
              ref PointF destPoint,
              ref RectangleF srcRect,
              GraphicsUnit pageUnit,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipEnumerateMetafileSrcRectDestPointI(
              HandleRef graphics,
              HandleRef metafile,
              ref Point destPoint,
              ref Rectangle srcRect,
              GraphicsUnit pageUnit,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipEnumerateMetafileSrcRectDestRect(
              HandleRef graphics,
              HandleRef metafile,
              ref RectangleF destRect,
              ref RectangleF srcRect,
              GraphicsUnit pageUnit,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipEnumerateMetafileSrcRectDestRectI(
              HandleRef graphics,
              HandleRef metafile,
              ref Rectangle destRect,
              ref Rectangle srcRect,
              GraphicsUnit pageUnit,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern unsafe int GdipEnumerateMetafileSrcRectDestPoints(
              HandleRef graphics,
              HandleRef metafile,
              PointF* destPoints,
              int count,
              ref RectangleF srcRect,
              GraphicsUnit pageUnit,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern unsafe int GdipEnumerateMetafileSrcRectDestPointsI(
              HandleRef graphics,
              HandleRef metafile,
              Point* destPoints,
              int count,
              ref Rectangle srcRect,
              GraphicsUnit pageUnit,
              Graphics.EnumerateMetafileProc callback,
              HandleRef callbackdata,
              HandleRef imageattributes);

            internal static extern int GdipPlayMetafileRecord(
              HandleRef graphics,
              EmfPlusRecordType recordType,
              int flags,
              int dataSize,
              byte[] data);

            internal static extern int GdipSaveGraphics(HandleRef graphics, out int state);

            internal static extern int GdipRestoreGraphics(HandleRef graphics, int state);

            internal static extern int GdipGetMetafileHeaderFromWmf(
              HandleRef hMetafile,
              WmfPlaceableFileHeader wmfplaceable,
              [In, Out] MetafileHeaderWmf metafileHeaderWmf);

            internal static extern int GdipGetMetafileHeaderFromEmf(
              HandleRef hEnhMetafile,
              [In, Out] MetafileHeaderEmf metafileHeaderEmf);

            internal static extern int GdipGetMetafileHeaderFromFile(string filename, IntPtr header);

            internal static extern int GdipGetMetafileHeaderFromStream(
              Interop.Ole32.IStream stream,
              IntPtr header);

            internal static extern int GdipGetMetafileHeaderFromMetafile(
              HandleRef metafile,
              IntPtr header);

            internal static extern int GdipGetHemfFromMetafile(
              HandleRef metafile,
              out IntPtr hEnhMetafile);

            internal static extern int GdipCreateMetafileFromWmf(
              HandleRef hMetafile,
              bool deleteWmf,
              WmfPlaceableFileHeader wmfplacealbeHeader,
              out IntPtr metafile);

            internal static extern int GdipCreateMetafileFromEmf(
              HandleRef hEnhMetafile,
              bool deleteEmf,
              out IntPtr metafile);

            internal static extern int GdipCreateMetafileFromFile(string file, out IntPtr metafile);

            internal static extern int GdipCreateMetafileFromStream(
              Interop.Ole32.IStream stream,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafile(
              HandleRef referenceHdc,
              EmfType emfType,
              ref RectangleF frameRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafile(
              HandleRef referenceHdc,
              EmfType emfType,
              HandleRef pframeRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafileI(
              HandleRef referenceHdc,
              EmfType emfType,
              ref Rectangle frameRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafileFileName(
              string fileName,
              HandleRef referenceHdc,
              EmfType emfType,
              ref RectangleF frameRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafileFileName(
              string fileName,
              HandleRef referenceHdc,
              EmfType emfType,
              HandleRef pframeRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafileFileNameI(
              string fileName,
              HandleRef referenceHdc,
              EmfType emfType,
              ref Rectangle frameRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafileStream(
              Interop.Ole32.IStream stream,
              HandleRef referenceHdc,
              EmfType emfType,
              ref RectangleF frameRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafileStream(
              Interop.Ole32.IStream stream,
              HandleRef referenceHdc,
              EmfType emfType,
              HandleRef pframeRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipRecordMetafileStreamI(
              Interop.Ole32.IStream stream,
              HandleRef referenceHdc,
              EmfType emfType,
              ref Rectangle frameRect,
              MetafileFrameUnit frameUnit,
              string description,
              out IntPtr metafile);

            internal static extern int GdipComment(HandleRef graphics, int sizeData, byte[] data);

            internal static extern int GdipCreateFontFromDC(HandleRef hdc, ref IntPtr font);

            internal static extern int GdipCreateFontFromLogfontW(
              HandleRef hdc,
              ref SafeNativeMethods.LOGFONT lf,
              out IntPtr font);

            internal static extern int GdipDrawString(
              HandleRef graphics,
              string textString,
              int length,
              HandleRef font,
              ref RectangleF layoutRect,
              HandleRef stringFormat,
              HandleRef brush);

            internal static extern int GdipMeasureString(
              HandleRef graphics,
              string textString,
              int length,
              HandleRef font,
              ref RectangleF layoutRect,
              HandleRef stringFormat,
              ref RectangleF boundingBox,
              out int codepointsFitted,
              out int linesFilled);

            internal static extern int GdipMeasureCharacterRanges(
              HandleRef graphics,
              string textString,
              int length,
              HandleRef font,
              ref RectangleF layoutRect,
              HandleRef stringFormat,
              int characterCount,
              [In, Out] IntPtr[] region);

            internal static extern int GdipCreateBitmapFromStream(
              Interop.Ole32.IStream stream,
              out IntPtr bitmap);

            internal static extern int GdipCreateBitmapFromStreamICM(
              Interop.Ole32.IStream stream,
              out IntPtr bitmap);
        }*/

}