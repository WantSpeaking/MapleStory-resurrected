


using System.Diagnostics.CodeAnalysis;

namespace ms
{
	// Simple color class which stores RGBA components
	[SuppressMessage ("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.Single[]")]
	public class Color
	{
		public static uint LENGTH = 4;

		// Codes of predefined colors
		public enum Code : uint
		{
			CNONE = 0x00000000,
			CWHITE = 0xFFFFFFFF,
			CBLACK = 0x000000FF,
			CRED = 0xFF0000FF,
			CGREEN = 0x00FF00FF,
			CBLUE = 0x0000FFFF,
			CYELLOW = 0xFFFF00FF,
			CTURQUOISE = 0x00FFFFFF,
			CPURPLE = 0xFF00FFFF
		}

		// Name of predefined colors
		public enum Name : uint
		{
			BLACK,
			WHITE,
			YELLOW,
			BLUE,
			RED,
			DARKRED,
			BROWN,
			JAMBALAYA,
			LIGHTGREY,
			DARKGREY,
			ORANGE,
			MEDIUMBLUE,
			VIOLET,
			TOBACCOBROWN,
			EAGLE,
			LEMONGRASS,
			TUNA,
			GALLERY,
			DUSTYGRAY,
			EMPEROR,
			MINESHAFT,
			HALFANDHALF,
			ENDEAVOUR,
			BROWNDERBY,
			PORCELAIN,
			IRISHCOFFEE,
			BOULDER,
			GREEN,
			LIGHTGREEN,
			JAPANESELAUREL,
			GRAYOLIVE,
			ELECTRICLIME,
			SUPERNOVA,
			CHARTREUSE,
			MALIBU,
			SILVERCHALICE,
			GRAY,
			TORCHRED,
			CREAM,
			SMALT,
			PRUSSIANBLUE,
			NUM_COLORS
		}

		// Predefined colors by name
		public static float[][] colors = new float[][]
		{
			new float[] {0.00f, 0.00f, 0.00f},
			new float[] {1.00f, 1.00f, 1.00f},
			new float[] {1.00f, 1.00f, 0.00f},
			new float[] {0.00f, 0.00f, 1.00f},
			new float[] {1.00f, 0.00f, 0.00f},
			new float[] {0.80f, 0.30f, 0.30f},
			new float[] {0.50f, 0.25f, 0.00f},
			new float[] {0.34f, 0.20f, 0.07f},
			new float[] {0.50f, 0.50f, 0.50f},
			new float[] {0.25f, 0.25f, 0.25f},
			new float[] {1.00f, 0.50f, 0.00f},
			new float[] {0.00f, 0.75f, 1.00f},
			new float[] {0.50f, 0.00f, 0.50f},
			new float[] {0.47f, 0.40f, 0.27f},
			new float[] {0.74f, 0.74f, 0.67f},
			new float[] {0.60f, 0.60f, 0.54f},
			new float[] {0.20f, 0.20f, 0.27f},
			new float[] {0.94f, 0.94f, 0.94f},
			new float[] {0.60f, 0.60f, 0.60f},
			new float[] {0.34f, 0.34f, 0.34f},
			new float[] {0.20f, 0.20f, 0.20f},
			new float[] {1.00f, 1.00f, 0.87f},
			new float[] {0.00f, 0.40f, 0.67f},
			new float[] {0.30f, 0.20f, 0.10f},
			new float[] {0.94f, 0.95f, 0.95f},
			new float[] {0.34f, 0.27f, 0.14f},
			new float[] {0.47f, 0.47f, 0.47f},
			new float[] {0.00f, 0.75f, 0.00f},
			new float[] {0.00f, 1.00f, 0.00f},
			new float[] {0.00f, 0.50f, 0.00f},
			new float[] {0.67f, 0.67f, 0.60f},
			new float[] {0.80f, 1.00f, 0.00f},
			new float[] {1.00f, 0.80f, 0.00f},
			new float[] {0.47f, 1.00f, 0.00f},
			new float[] {0.47f, 0.80f, 1.00f},
			new float[] {0.67f, 0.67f, 0.67f},
			new float[] {0.54f, 0.54f, 0.54f},
			new float[] {0.94f, 0.00f, 0.20f},
			new float[] {1.00f, 1.00f, 0.80f},
			new float[] {0.00f, 0.23f, 0.56f},
			new float[] {0.01f, 0.19f, 0.28f}
		};

		// Create a color by an array of real numbers [0.0f, 1.0f]
		public Color (float[] comps)
		{
			this.rgba = comps;
		}

		// Create a color by real numbers [0.0f, 1.0f]
		public Color (float red, float green, float blue, float alpha) : this (new float[] {red, green, blue, alpha})
		{
		}

		// Create a color by an array of natural numbers [0, 255]
		public Color (byte[] comps) : this (comps[0], comps[1], comps[2], comps[3])
		{
		}

		// Create a color by natural numbers [0, 255]
		public Color (byte red, byte green, byte blue, byte alpha) : this ((float)red / 255, (float)green / 255, (float)blue / 255, (float)alpha / 255)
		{
		}

		// Create a color by code
		public Color (uint code) : this ((byte)(code >> 24), (byte)(code >> 16), (byte)(code >> 8), (byte)code)
		{
		}

		// Create a color by named code
		public Color (Code code) : this ((uint)code)
		{
		}

		public Color (Color src) : this (src.r (), src.g (), src.b (), src.a ())
		{
		}

		public Color () : this (Code.CNONE)
		{
		}

		// Check whether the color is completely invisible
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool invisible() const
		public bool invisible ()
		{
			return rgba[3] <= 0.0f;
		}

		// Return the red component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float r() const
		public float r ()
		{
			return rgba[0];
		}

		// Return the green component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float g() const
		public float g ()
		{
			return rgba[1];
		}

		// Return the blue component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float b() const
		public float b ()
		{
			return rgba[2];
		}

		// Return the alpha (opacity) component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float a() const
		public float a ()
		{
			return rgba[3];
		}

		// Return all components
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const float* data() const
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: C# has no equivalent to methods returning pointers to value types:
		public float[] data ()
		{
			return rgba;
		}

		/*// Return a begin iterator
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float[]::const_iterator begin() const
		public float[].const_iterator begin()
		{
			return rgba.begin();
		}

		// Return an end iterator
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: float[]::const_iterator end() const
		public float[].const_iterator end()
		{
			return rgba.end();
		}*/

		// Blend the second color into the first
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Color blend(const Color& other, float alpha) const
		public Color blend (Color other, float alpha)
		{
			Color blended = new Color ();

/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: transform(begin(), end(), other.begin(), blended.begin(), [alpha](float first, float second)
			transform(begin(), end(), other.begin(), blended.begin(), (float first, float second) =>
			{
					return lerp(first, second, alpha);
			});*/

			return blended;
		}

		// Combine two colors
		public static Color operator * (Color ImpliedObject, Color o)
		{
			return new Color (ImpliedObject.r () * o.r (), ImpliedObject.g () * o.g (), ImpliedObject.b () * o.b (), ImpliedObject.a () * o.a ());
		}

		// Combine two colors
		public static Color operator / (Color ImpliedObject, Color o)
		{
			return new Color (ImpliedObject.r () / o.r (), ImpliedObject.g () / o.g (), ImpliedObject.b () / o.b (), ImpliedObject.a () / o.a ());
		}

		private float[] rgba;

		public override string ToString ()
		{
			return $"{{R:{r ()} G:{g ()} B:{b ()} A:{a ()}}}";
		}
	}
}