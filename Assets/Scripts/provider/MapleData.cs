using System.Collections;
using System.Collections.Generic;
using provider.wz;

/*
	This file is part of the OdinMS Maple Story Server
    Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
		       Matthias Butz <matze@odinms.de>
		       Jan Christian Meyer <vimes@odinms.de>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation version 3 as published by
    the Free Software Foundation. You may not use, modify or distribute
    this program under any other version of the GNU Affero General Public
    License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace provider
{
	using MapleDataType = provider.wz.MapleDataType;

	public class MapleData : MapleDataEntity, IEnumerable<MapleData>
	{
		public virtual string Name { get; }
		public virtual MapleDataType Type { get; }
		public virtual IList<MapleData> Children { get; }

		public virtual MapleData getChildByPath (string path)
		{
			return null;
		}


		public virtual object Data { get; }

		public virtual MapleData this [string name]
		{
			get { return null; }
		}

		public virtual string FullPath { get; }

		#region implement interface

		public virtual MapleDataEntity Parent => throw new System.NotImplementedException ();

		public virtual IEnumerator<MapleData> GetEnumerator ()
		{
			throw new System.NotImplementedException ();
		}

		public override string ToString ()
		{
			return Data?.ToString () ?? string.Empty;
		}

		public bool IsTexture ()
		{
			return Type == MapleDataType.CANVAS || Type == MapleDataType.IMG_0x00;
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		#region implicit operator

		public static implicit operator int (MapleData mapleData)
		{
			return (int)(mapleData?.Data ?? 0);
		}

		public static implicit operator int? (MapleData mapleData)
		{
			return (int?)mapleData?.Data;
		}

		public static implicit operator short (MapleData mapleData)
		{
			/*	if (mapleData.Data == null) return 0;
				if (mapleData.Type ==  MapleDataType.INT)
				{
					return short.Parse (mapleData.Data.ToString());
				}*/
			return mapleData?.Data == null ? (short)0 : short.Parse (mapleData.Data.ToString ());
		}

		public static implicit operator long (MapleData mapleData)
		{
			return mapleData?.Data == null ? (long)0 : long.Parse (mapleData.Data.ToString ());
		}

		public static implicit operator float (MapleData mapleData)
		{
			return mapleData?.Data == null ? (float)0 : float.Parse (mapleData.Data.ToString ());
		}

		public static implicit operator float? (MapleData mapleData)
		{
			return (float?)mapleData?.Data;
		}

		public static implicit operator double (MapleData mapleData)
		{
			return mapleData?.Data == null ? (double)0 : double.Parse (mapleData.Data.ToString ());
		}

		public static implicit operator string (MapleData mapleData)
		{
			return (string)mapleData?.Data?.ToString ();
		}

		public static implicit operator ms.Point_short (MapleData mapleData)
		{
			return (ms.Point_short)mapleData?.Data ?? ms.Point_short.zero;
		}

		/*public static implicit operator byte[] (MapleData mapleData)
		{
			return mapleData.GetBytes ();
		}*/

		public static implicit operator ushort (MapleData mapleData)
		{
			return mapleData?.Data == null ? (ushort)0 : ushort.Parse (mapleData.Data.ToString ());
		}

		public static implicit operator byte (MapleData mapleData)
		{
			return mapleData?.Data == null ? (byte)0 : byte.Parse (mapleData.Data.ToString ());
		}

		public static implicit operator bool (MapleData mapleData)
		{
			//return mapleData?.GetShort() == 1;
			return (int?)mapleData?.Data == 1;
		}

		public static implicit operator FileStoredPngMapleCanvas (MapleData mapleData)
		{
			return (FileStoredPngMapleCanvas)mapleData.Data;
		}

		public static implicit operator ms.Animation (MapleData mapleData)
		{
			if (mapleData != null)
			{
				return new ms.Animation (mapleData);
			}
			else
			{
				return new ms.Animation ();
			}
		}

		/*
		public static implicit operator Sprite (MapleData mapleData)
		{
			if (mapleData != null)
			{
				return new Sprite (mapleData);
			}
			else
			{
				return new Sprite ();
			}
		}

		public static implicit operator Texture (MapleData mapleData)
		{
			if (mapleData != null)
			{
				return new Texture (mapleData);
			}
			else
			{
				return new Texture ();
			}
		}

		public static implicit operator Sound (MapleData mapleData)
		{
			if (mapleData != null)
			{
				return new Sound (mapleData);
			}
			else
			{
				return new Sound ();
			}
		}

		public static implicit operator List<WzImageProperty> (MapleData mapleData)
		{
			if (mapleData is WzImageProperty imageProperty)
			{
				return imageProperty.WzProperties;
			}

			return default;
		}

		public static implicit operator WzObjectType (MapleData mapleData)
		{
			return mapleData.ObjectType;
		}

		public static implicit operator WzPropertyType (MapleData mapleData)
		{
			if (mapleData is WzImageProperty wzImageProperty)
			{
				return wzImageProperty.PropertyType;
			}

			return WzPropertyType.Null;
		}*/

		#endregion
	}
}