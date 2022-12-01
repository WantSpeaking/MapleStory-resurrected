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
namespace provider.wz
{

	public class WZEntry : MapleDataEntry
	{
		private string name;
		private int size;
		private int checksum;
		private int offset;
		private MapleDataEntity parent;

		public WZEntry(string name, int size, int checksum, MapleDataEntity parent) : base()
		{
			this.name = name;
			this.size = size;
			this.checksum = checksum;
			this.parent = parent;
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public  int Size
		{
			get
			{
				return size;
			}
		}

		public  int Checksum
		{
			get
			{
				return checksum;
			}
		}

		public  int Offset
		{
			get
			{
				return offset;
			}
		}

		public  MapleDataEntity Parent
		{
			get
			{
				return parent;
			}
		}

		/*public string getName => name;

		public MapleDataEntity getParent => parent;

		int MapleDataEntry.getSize => throw new System.NotImplementedException ();

		int MapleDataEntry.getChecksum => throw new System.NotImplementedException ();

		int MapleDataEntry.getOffset => throw new System.NotImplementedException ();

		public int getChecksum ()
		{
			return size;
		}

		public int getOffset ()
		{
			return offset;
		}

		public int getSize ()
		{
			return size;
		}*/
	}

}