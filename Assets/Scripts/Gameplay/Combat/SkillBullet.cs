﻿using System.Collections.Generic;
using MapleLib.WzLib;

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
	public abstract class SkillBullet : System.IDisposable
	{
		public virtual void Dispose ()
		{
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: virtual Animation get(const Char& user, int bulletid) const = 0;
		public abstract Animation get (Char user, int bulletid);

		protected class Ball
		{
			public Animation animation = new Animation ();

			public Ball (WzObject src)
			{
				animation = src;
			}

			public Ball ()
			{
			}
		}
	}

	public class RegularBullet : SkillBullet
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Animation get(const Char& user, int bulletid) const override
		public override Animation get (Char user, int bulletid)
		{
			return BulletData.get (bulletid).get_animation ();
		}
	}

	public class SingleBullet : SkillBullet
	{
		public SingleBullet (WzObject src)
		{
			ball = new Ball (src["ball"]);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Animation get(const Char& user, int bulletid) const override
		public override Animation get (Char user, int bulletid)
		{
			return ball.animation;
		}

		private Ball ball = new Ball ();
	}

	public class BySkillLevelBullet : SkillBullet
	{
		public BySkillLevelBullet (WzObject src, int id)
		{
			skillid = id;

			foreach (var sub in src["level"])
			{
				var level = string_conversion.or_zero<int> (sub.Name);
				bullets[level] = new Ball (sub["ball"]);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Animation get(const Char& user, int bulletid) const override
		public override Animation get (Char user, int bulletid)
		{
			int level = user.get_skilllevel (skillid);
			Animation anim;
			if (bullets.TryGetValue (level, out var ball))
			{
				anim = ball.animation;
			}
			else
			{
				anim = new Animation ();
			}

			return anim;

			/*var iter = bullets.find(level);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			if (iter != bullets.end())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				return iter.second.animation;
			}
			else
			{
				return
				{
				};
			}*/
		}

		private Dictionary<int, Ball> bullets = new Dictionary<int, Ball> ();
		private int skillid;
	}
}