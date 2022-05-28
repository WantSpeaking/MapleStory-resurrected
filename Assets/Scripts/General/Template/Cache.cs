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


namespace ms
{
	// Template for a cache of game objects 
	// which can be constructed from an identifier.
	// The 'get' factory method is static.
	public class Cache<T> : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

		// Return a ref to the game object with the specified id.
		// If the object is not in cache, it is created.
		public static T get (int id)
		{
			if (!cache.TryGetValue (id, out T t))
			{
				t = (T)System.Activator.CreateInstance (typeof (T), id);
				cache.Add (id, t);
			}

			return t;

		}

		private static Dictionary<int, T> cache = new Dictionary<int, T> ();
	}

}