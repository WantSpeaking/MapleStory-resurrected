/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010, 2015 Snow and haha01haha01
   
 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using MapleLib.WzLib.WzProperties;
using ms;
using ms.Helper;

namespace MapleLib.WzLib
{
	/// <summary>
	/// An abstract class for wz objects
	/// </summary>
	public abstract class WzObject : IDisposable, IEnumerable<WzImageProperty>
	{
		private object hcTag = null;
		private object hcTag_spine = null;
		private object msTag = null;
		private object msTag_spine = null;
		private object tag3 = null;

		public abstract void Dispose ();

		/// <summary>
		/// The name of the object
		/// </summary>
		public abstract string Name { get; set; }

		/// <summary>
		/// The WzObjectType of the object
		/// </summary>
		public abstract WzObjectType ObjectType { get; }

		/// <summary>
		/// Returns the parent object
		/// </summary>
		public abstract WzObject Parent { get; internal set; }

		/// <summary>
		/// Returns the parent WZ File
		/// </summary>
		public abstract WzFile WzFileParent { get; }

		public WzObject this [string name]
		{
			get
			{
				if (this is WzFile)
				{
					return ((WzFile)this)[name];
				}
				else if (this is WzDirectory)
				{
					return ((WzDirectory)this)[name];
				}
				else if (this is WzImage)
				{
					return ((WzImage)this)[name];
				}
				else if (this is WzImageProperty)
				{
					return ((WzImageProperty)this)[name];
				}
				else
				{
					throw new NotImplementedException ();
				}
			}
		}


		/// <summary>
		/// Gets the top most WZObject directory (i.e Map.wz, Skill.wz)
		/// </summary>
		/// <returns></returns>
		public WzObject GetTopMostWzDirectory ()
		{
			WzObject parent = this.Parent;
			if (parent == null)
				return this; // this

			while (parent.Parent != null)
			{
				parent = parent.Parent;
			}

			return parent;
		}

		public string FullPath
		{
			get
			{
				if (this is WzFile) return ((WzFile)this).WzDirectory.Name;
				string result = this.Name;
				WzObject currObj = this;
				while (currObj.Parent != null)
				{
					currObj = currObj.Parent;
					result = currObj.Name + @"\" + result;
				}

				return result;
			}
		}

		/// <summary>
		/// Used in HaCreator to save already parsed images
		/// </summary>
		public virtual object HCTag
		{
			get { return hcTag; }
			set { hcTag = value; }
		}


		/// <summary>
		/// Used in HaCreator to save already parsed spine images
		/// </summary>
		public virtual object HCTagSpine
		{
			get { return hcTag_spine; }
			set { hcTag_spine = value; }
		}

		/// <summary>
		/// Used in HaCreator's MapSimulator to save already parsed textures
		/// </summary>
		public virtual object MSTag
		{
			get { return msTag; }
			set { msTag = value; }
		}

		/// <summary>
		/// Used in HaCreator's MapSimulator to save already parsed spine objects
		/// </summary>
		public virtual object MSTagSpine
		{
			get { return msTag_spine; }
			set { msTag_spine = value; }
		}

		/// <summary>
		/// Used in HaRepacker to save WzNodes
		/// </summary>
		public virtual object HRTag
		{
			get { return tag3; }
			set { tag3 = value; }
		}

		public virtual object WzValue
		{
			get { return null; }
		}

		public abstract void Remove ();

		//Credits to BluePoop for the idea of using cast overriding
		//2015 - That is the worst idea ever, removed and replaced with Get* methods

		#region Cast Values

		public virtual int GetInt ()
		{
			throw new NotImplementedException ();
		}

		public virtual short GetShort ()
		{
			throw new NotImplementedException ();
		}

		public virtual long GetLong ()
		{
			throw new NotImplementedException ();
		}

		public virtual float GetFloat ()
		{
			throw new NotImplementedException ();
		}

		public virtual double GetDouble ()
		{
			throw new NotImplementedException ();
		}

		public virtual string GetString ()
		{
			throw new NotImplementedException ();
		}

		public virtual Point GetPoint ()
		{
			throw new NotImplementedException ();
		}

		public virtual Bitmap GetBitmap ()
		{
			return null;
		}
		public virtual byte[] GetPngData ()
		{
			return null;
		}
		public virtual byte[] GetBytes ()
		{
			throw new NotImplementedException ();
		}

		#endregion

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
		public static implicit operator ms.Point<short> (WzObject wzObject)
		{
			return wzObject?.GetPoint ().ToMSPoint () ?? Point<short>.zero;
		}

		public static implicit operator Bitmap (WzObject wzObject)
		{
			return wzObject?.GetBitmap ();
		}

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
			return wzObject?.GetShort () == 1;
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
			return GetBitmap () != null;
		}
		
		public IEnumerator<WzImageProperty> GetEnumerator ()
		{
			if (this is WzImage wzImage)
			{
				return wzImage.WzProperties.GetEnumerator ();
			}
			else if (this is WzImageProperty imageProperty)
			{
				return imageProperty.WzProperties.GetEnumerator ();
			}
			else
			{
				return default;
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}
	}
}