using System.Collections.Generic;
using System.Collections.ObjectModel;

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

	public class WZDirectoryEntry : WZEntry, MapleDataDirectoryEntry
	{
		private IList<MapleDataDirectoryEntry> subdirs = new List<MapleDataDirectoryEntry> ();
		private IList<MapleDataFileEntry> files = new List<MapleDataFileEntry> ();
		private IDictionary<string, MapleDataEntry> entries = new Dictionary<string, MapleDataEntry> ();

		public WZDirectoryEntry (string name, int size, int checksum, MapleDataEntity parent) : base (name, size, checksum, parent)
		{
		}

		public WZDirectoryEntry () : base (null, 0, 0, null)
		{
		}

		public virtual void addDirectory (MapleDataDirectoryEntry dir)
		{
			subdirs.Add (dir);
			entries[dir.Name] = dir;
		}

		public virtual void addFile (MapleDataFileEntry fileEntry)
		{
			files.Add (fileEntry);
			entries[fileEntry.Name] = fileEntry;
		}

		public virtual IList<MapleDataDirectoryEntry> Subdirectories
		{
			get
			{
				return subdirs;
			}
		}

		public virtual IList<MapleDataFileEntry> Files
		{
			get
			{
				return files;
			}
		}

		public virtual MapleDataEntry getEntry (string name)
		{
			return entries[name];
		}
	}

}