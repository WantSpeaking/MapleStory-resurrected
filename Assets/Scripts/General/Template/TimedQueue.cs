using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Priority_Queue;

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
	public class TimedQueue
	{
		public TimedQueue ()
		{
			time = 0;
		}
		public TimedQueue (System.Action in_action)
		{
			this.action = in_action;
			time = 0;
		}

		public Timed register (Action in_action, long delay)
		{
			this.action = in_action;
			var handle = new Timed (time + delay);
			queue.Enqueue (handle, time + delay);
			return handle;
		}

		public void push (long delay)
		{
			queue.Enqueue (new Timed (time + delay), time + delay);
		}

		public void update (long timestep = Constants.TIMESTEP)
		{
			time += timestep;

			for (; queue.Count > 0; queue.Dequeue ())
			{
				Timed top = queue.First;

				if (top.isValid)
				{
					if (top.when > time)
					{
						break;
					}

					action?.Invoke ();
				}
			}

			time += timestep;
		}

		public class Timed
		{
			public long when;
			public bool isValid { get; set; }
			public Timed (long w)
			{
				this.when = w;
				isValid = true;
			}

			public void cancel (bool result)
			{
				isValid = false;
			}
		}

		private readonly SimplePriorityQueue<Timed, long> queue = new ();
		private System.Action action;
		private long time;
	}

	public class TimedQueue<T>
	{
		public TimedQueue ()
		{
			time = 0;
		}
		public TimedQueue (System.Action<T> in_action)
		{
			this.action = in_action;
			time = 0;
		}

		public void push (long delay, T t)
		{
			queue.Enqueue (new Timed (time + delay, t), time + delay);
		}

		/*//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++11 variadic templates:
				public void emplace<typename...Args>(long delay, Args & ...args)
				{
					queue.Enqueue(time + delay, move(args)...);
				}*/
		public void update (long timestep = Constants.TIMESTEP)
		{
			time += timestep;

			for (; queue.Count > 0; queue.Dequeue ())
			{
				Timed top = queue.First;

				if (top.when > time)
				{
					break;
				}

				action (top.value);
			}

			time += timestep;
		}

		private class Timed
		{
			public T value;
			public long when;

			public Timed (long w, T v)
			{
				this.when = w;
				this.value = v;
			}

			/*
			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: There is no equivalent in C# to C++11 variadic templates:
						public Timed<typename...Args>(long w, Args & ...args)
						{
							this.when = {w};
							this.value = {forward<Args>(args)...};
						}*/
		}

		/*private class TimedComparator<V>:IComparer<V>
		{
/#1#/C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ()(const Timed& a, const Timed& b) const
			public static bool functorMethod(Timed a, Timed b)
			{
				return a.when > b.when;
			}

			public int CompareTo (Timed other)
			{
				return a.when > b.when;
			}#1#

			public int Compare (Timed x, Timed y)
			{
				return x.when > y.when;
			}

			public int Compare (V x, V y)
			{
				throw new NotImplementedException ();
			}
		}*/

		private SimplePriorityQueue<Timed, long> queue = new SimplePriorityQueue<Timed, long> ();
		private readonly System.Action<T> action;
		private long time;
	}
}