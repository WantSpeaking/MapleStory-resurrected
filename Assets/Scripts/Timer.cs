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

using System;

namespace ms
{
	// Small class for measuring elapsed time between game loops.
	public class Timer : Singleton<Timer>
	{
		public Timer()
		{
			start();
		}

		public new void Dispose()
		{
			base.Dispose();
		}

		// Start the timer by setting the last measurement to now.
		public void start()
		{
			point = DateTime.Now;
		}

		// Return time elapsed since the last measurement.
		public int stop()
		{
			return (DateTime.Now - point).Milliseconds;
		}


		private DateTime point = DateTime.MinValue;
	}

	// Small class for measuring elapsed time given a specific start time.
	public class ContinuousTimer : Singleton<ContinuousTimer>
	{

		public ContinuousTimer()
		{
			start();
		}

		public new void Dispose()
		{
			base.Dispose();
		}

		// Return now from the clock to be used to calculate elapsed time later.
		public DateTime start()
		{
			return DateTime.Now;
		}

		// Return time elapsed since the last measurement provided.
		public long stop(DateTime last)
		{
			return (DateTime.Now - last).Milliseconds;
		}

	}
}