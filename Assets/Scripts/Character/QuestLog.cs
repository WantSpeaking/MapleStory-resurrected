using System;
using System.Collections.Generic;
using System.Linq;

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
	// Class that stores information on the quest log of an individual character
	public class QuestLog
	{
		public void add_started(short qid, string qdata)
		{
			started[qid] = qdata;
		}
		public void add_in_progress(short qid, short qidl, string qdata)
		{
			in_progress[qid] = new Tuple<short, string> (qidl, qdata);
		}
		public void add_completed(short qid, long time)
		{
			completed[qid] = time;
		}
		public bool is_started(short qid)
		{
			return started.Any(pair=>pair.Key==qid);
		}
		public short get_last_started()
		{
			return started.Last ().Key;
			/*var qend = started.end();
			qend--;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return qend.first;*/
		}

		private SortedDictionary<short, string> started = new SortedDictionary<short, string>();
		private SortedDictionary<short, System.Tuple<short, string>> in_progress = new SortedDictionary<short, System.Tuple<short, string>>();
		private SortedDictionary<short, long> completed = new SortedDictionary<short, long>();
	}
}


