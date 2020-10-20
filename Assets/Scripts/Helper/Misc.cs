#define USE_NX

using System;
using System.Collections.Generic;

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
	public static class string_format
	{
		public static void split_number ()
		{
		}

		public static string extend_id (int id, int length)
		{
			string strid = id.ToString ();
			//if (strid.Length < length)

			for (int i = 0; i < length - strid.Length; i++)
			{
				strid = strid.Insert (0, "0");
			}

			return strid;
		}

		public static void bytecode ()
		{
		}

		/*namespace NxHelper
		{
			namespace Map
			{

		public class MapInfo
		{
			public string description;
			public string name;
			public string street_name;
			public string full_name;
		}*/
	}

	public static class string_conversion
	{
		public static T or_default<T> (string str, T defaultVaule)
		{
			T ret = (T)Convert.ChangeType (defaultVaule, typeof (T));
			try
			{
				ret = (T)Convert.ChangeType (str, typeof (T));
			}
			catch (Exception exception)
			{
			}

			return ret;
		}

		public static T or_zero<T> (string str)
		{
			return or_default<T> (str, default (T));
		}
	}
}


#if USE_NX
#endif