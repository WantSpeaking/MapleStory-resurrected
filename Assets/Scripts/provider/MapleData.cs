using MapleLib.WzLib;
using ms;
using System;
using System.Collections;
using System.Collections.Generic;

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

	public abstract class MapleData : MapleDataEntity, IEnumerable<MapleData>
	{
        public abstract MapleDataType Type {get;}
        public abstract IList<MapleData> Children {get;}
        public abstract MapleData getChildByPath(string path);

        public IEnumerator<MapleData> GetEnumerator()
        {
            return Children?.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Children?.GetEnumerator();
        }

        public abstract object Data {get;}

        public abstract string Name { get; }

        public abstract MapleDataEntity Parent { get; }

        public abstract MapleData this[string dataPath] { get;}

        public abstract bool IsTexture();

        public virtual Point GetPoint()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return MapleDataTool.getString(this);
        }

        #region implicit operator
        public static implicit operator int(MapleData wzObject)
        {
            return MapleDataTool.getInt(wzObject);
        }
        public static implicit operator short(MapleData wzObject)
        {
            return (short)MapleDataTool.getInt(wzObject);
        }
        public static implicit operator bool(MapleData wzObject)
        {
            return MapleDataTool.getInt(wzObject) == 1;
        }
        public static implicit operator MapleLib.WzLib.Point(MapleData wzObject)
        {
            return MapleDataTool.getPoint(wzObject);
        }

        public static implicit operator Point_short(MapleData data)
        {
            return new Point_short((short)MapleDataTool.getInt(data?["x"]), (short)MapleDataTool.getInt(data?["y"]));
        }
        public static implicit operator byte(MapleData wzObject)
        {
            return (byte)MapleDataTool.getInt(wzObject);
        }
        public static implicit operator ushort(MapleData wzObject)
        {
            return (ushort)MapleDataTool.getInt(wzObject);
        }
        public static implicit operator string(MapleData wzObject)
        {
            return MapleDataTool.getString(wzObject);
        }
        public static implicit operator Animation(MapleData wzObject)
        {
            return new Animation(wzObject);
        }
        public static implicit operator ms.Texture(MapleData wzObject)
        {
            return new ms.Texture(wzObject);
        }
        #endregion
    }

}