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
	// Interface for tooltips
	// Window with helpful information that appears on mouse hover at a specific location
	public abstract class Tooltip : System.IDisposable
	{
		// Possible parent UIs for Tooltips
		public enum Parent
		{
			NONE,
			EQUIPINVENTORY,
			ITEMINVENTORY,
			SKILLBOOK,
			SHOP,
			EVENT,
			TEXT,
			KEYCONFIG,
			WORLDMAP,
			MINIMAP
		}

		public virtual void Dispose()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual void draw(Point<short> cursorpos) const = 0;
		public abstract void draw(Point<short> cursorpos);
	}
}