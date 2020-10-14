//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////



namespace ms
{
	// Simple color class which stores RGBA components
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
		public static float[,] colors = new float[,]
		{
			{0.00f, 0.00f, 0.00f},
			{1.00f, 1.00f, 1.00f},
			{1.00f, 1.00f, 0.00f},
			{0.00f, 0.00f, 1.00f},
			{1.00f, 0.00f, 0.00f},
			{0.80f, 0.30f, 0.30f},
			{0.50f, 0.25f, 0.00f},
			{0.34f, 0.20f, 0.07f},
			{0.50f, 0.50f, 0.50f},
			{0.25f, 0.25f, 0.25f},
			{1.00f, 0.50f, 0.00f},
			{0.00f, 0.75f, 1.00f},
			{0.50f, 0.00f, 0.50f},
			{0.47f, 0.40f, 0.27f},
			{0.74f, 0.74f, 0.67f},
			{0.60f, 0.60f, 0.54f},
			{0.20f, 0.20f, 0.27f},
			{0.94f, 0.94f, 0.94f},
			{0.60f, 0.60f, 0.60f},
			{0.34f, 0.34f, 0.34f},
			{0.20f, 0.20f, 0.20f},
			{1.00f, 1.00f, 0.87f},
			{0.00f, 0.40f, 0.67f},
			{0.30f, 0.20f, 0.10f},
			{0.94f, 0.95f, 0.95f},
			{0.34f, 0.27f, 0.14f},
			{0.47f, 0.47f, 0.47f},
			{0.00f, 0.75f, 0.00f},
			{0.00f, 1.00f, 0.00f},
			{0.00f, 0.50f, 0.00f},
			{0.67f, 0.67f, 0.60f},
			{0.80f, 1.00f, 0.00f},
			{1.00f, 0.80f, 0.00f},
			{0.47f, 1.00f, 0.00f},
			{0.47f, 0.80f, 1.00f},
			{0.67f, 0.67f, 0.67f},
			{0.54f, 0.54f, 0.54f},
			{0.94f, 0.00f, 0.20f},
			{1.00f, 1.00f, 0.80f},
			{0.00f, 0.23f, 0.56f},
			{0.01f, 0.19f, 0.28f}
		};

		// Create a color by an array of real numbers [0.0f, 1.0f]
		public Color(float[] comps)
		{
			this.rgba = comps;
		}
		// Create a color by real numbers [0.0f, 1.0f]
		public Color(float red, float green, float blue, float alpha) : this(new float[]{red, green, blue, alpha})
		{
		}
		// Create a color by an array of natural numbers [0, 255]
		public Color(byte[] comps) : this(comps[0], comps[1], comps[2], comps[3])
		{
		}

		// Create a color by natural numbers [0, 255]
		public Color(byte red, byte green, byte blue, byte alpha) : this((float)red / 255, (float)green / 255, (float)blue / 255, (float)alpha / 255)
		{
		}

		// Create a color by code
		public Color(uint code) : this((byte)(code >> 24), (byte)(code >> 16), (byte)(code >> 8), (byte)code)
		{
		}

		// Create a color by named code
		public Color(Code code) : this((uint)code)
		{
		}
		public Color() :this(Code.CNONE)
		{
		}

		// Check whether the color is completely invisible
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr bool invisible() const
		public bool invisible()
		{
			return rgba[3] <= 0.0f;
		}

		// Return the red component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float r() const
		public float r()
		{
			return rgba[0];
		}

		// Return the green component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float g() const
		public float g()
		{
			return rgba[1];
		}

		// Return the blue component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float b() const
		public float b()
		{
			return rgba[2];
		}

		// Return the alpha (opacity) component
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr float a() const
		public float a()
		{
			return rgba[3];
		}

		// Return all components
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const float* data() const
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: C# has no equivalent to methods returning pointers to value types:
		public float[] data()
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
		public Color blend(Color other, float alpha)
		{
			Color blended = new Color();

/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Only lambda expressions having all locals passed by reference can be converted to C#:
//ORIGINAL LINE: std::transform(begin(), end(), other.begin(), blended.begin(), [alpha](float first, float second)
			std::transform(begin(), end(), other.begin(), blended.begin(), (float first, float second) =>
			{
					return lerp(first, second, alpha);
			});*/

			return blended;
		}

		// Combine two colors
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Color operator *(const Color& o) const
		public static Color operator * (Color ImpliedObject, Color o)
		{
			return new Color(ImpliedObject.r() * o.r(), ImpliedObject.g() * o.g(), ImpliedObject.b() * o.b(), ImpliedObject.a() * o.a());
		}

		// Combine two colors
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: constexpr Color operator /(const Color& o) const
		public static Color operator / (Color ImpliedObject, Color o)
		{
			return new Color(ImpliedObject.r() / o.r(), ImpliedObject.g() / o.g(), ImpliedObject.b() / o.b(), ImpliedObject.a() / o.a());
		}

		private float[] rgba = new float[LENGTH];
	}
}

