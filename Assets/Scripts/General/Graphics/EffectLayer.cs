using System;
using System.Collections.Generic;
using System.Linq;

namespace ms
{
	// A list of animations. Animations will be removed after all frames were displayed.
	public class EffectLayer
	{
		public void drawbelow (Point_short position, float alpha)
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

		public void drawabove (Point_short position, float alpha)
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
				this.sprite = new ms.Sprite (new Animation (a), args);
				this.speed = s;
			}

			public void Dispose ()
			{
				sprite.Dispose ();
			}
			
			//private static DrawArgument effectRenderOrderArgs = new DrawArgument(Constants.get ().sortingLayer_Effect,0);
			public void draw (Point_short position, float alpha)
			{
				//sprite.draw (new Point_short (position) , alpha, effectRenderOrderArgs);
				sprite.draw (new Point_short (position) , alpha);
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