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
	// A list of animations. Animations will be removed after all frames were displayed.
	public class EffectLayer
	{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drawbelow(Point<short> position, float alpha) const
		public void drawbelow (Point<short> position, float alpha)
		{
			foreach (var pair in effects)
			{
				if (pair.Key < -1)
				{
					foreach (var effect in pair.Value)
					{
						effect.draw (position, alpha);
					}
				}
			}

			/*for (var iter = effects.begin(); iter != effects.upper_bound(-1); ++iter)
			for (var& effect : iter->second)
				effect.draw(position, alpha);*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void drawabove(Point<short> position, float alpha) const
		public void drawabove (Point<short> position, float alpha)
		{
			foreach (var pair in effects)
			{
				if (pair.Key >= -1)
				{
					foreach (var effect in pair.Value)
					{
						effect.draw (position, alpha);
					}
				}
			}


			/*for (var iter = effects.upper_bound(-1); iter != effects.end(); ++iter)
			{
				foreach (var effect in iter.second)
				{
					effect.draw(position, alpha);
				}
			}*/
		}

		private readonly List<Effect> _bufferEffects = new List<Effect>(); 
		public void update ()
		{
			
			foreach (var pair in effects)
			{
				_bufferEffects.Clear ();
				
				foreach (var effect in pair.Value)
				{
					if (effect.update ())
					{
						_bufferEffects.Add (effect);
					}
				}

				foreach (var _bufferEffect in _bufferEffects)
				{
					_bufferEffect.Dispose ();
					pair.Value.Remove (_bufferEffect);
				};
				
				//pair.Value.Remove (pair.Value.Where ((Effect effect) => effect.update())).remove_if ((Effect effect) => effect.update);
			}
		}

		public void add (Animation animation, DrawArgument args, sbyte z, float speed)
		{
			effects.TryAdd (z);
			effects[z].AddLast (new Effect (animation, args, speed));
		}

		public void add (Animation animation, DrawArgument args, sbyte z)
		{
			add (animation, args, z, 1.0f);
		}

		public void add (Animation animation, DrawArgument args)
		{
			add (animation, args, 0, 1.0f);
		}

		public void add (Animation animation)
		{
			add (animation, new DrawArgument (), 0, 1.0f);
		}

		private class Effect : IDisposable
		{
			public Effect (Animation a, DrawArgument args, float s)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: this.sprite = new ms.Sprite(a, args);
				this.sprite = new ms.Sprite (a, args);
				this.speed = s;
			}

			public void Dispose ()
			{
				sprite.Dispose ();
			}
			
			public void draw (Point<short> position, float alpha)
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: sprite.draw(position, alpha);
				sprite.draw (position, alpha);
			}

			public bool update ()
			{
				return sprite.update ((ushort)(Constants.TIMESTEP * speed));
			}

			private Sprite sprite = new Sprite ();
			private float speed;
		}

		private SortedDictionary<sbyte, LinkedList<Effect>> effects = new SortedDictionary<sbyte, LinkedList<Effect>> ();
	}
}