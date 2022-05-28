using System.Collections.Generic;




namespace ms
{
	// Collection of reactors on a map
	public class MapReactors
	{
		// Draw all reactors on a layer
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void draw(Layer::Id layer, double viewx, double viewy, float alpha) const
		public void draw (Layer.Id layer, double viewx, double viewy, float alpha)
		{
			reactors.draw (layer, viewx, viewy, alpha);
		}
		// Update all reactors

		// Spawns all reactors to map with proper footholds
		public void update (Physics physics)
		{
			for (; spawns.Count > 0; spawns.Dequeue ())
			{
				ReactorSpawn spawn = spawns.Peek ();

				int oid = spawn.get_oid ();

				var reactor = reactors.get (oid);
				if (reactor)
				{
					reactor.get ().makeactive ();
				}
				else
				{
					reactors.add (spawn.instantiate (physics));
				}
			}

			reactors.update (physics);
		}

		// Trigger a reactor
		public void trigger (int oid, sbyte state)
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			Optional<Reactor> reactor = (Reactor)reactors.get (oid);
			if (reactor)
			{
				reactor.get ().set_state (state);
			}
		}

		// Spawn a new reactor
		public void spawn (ReactorSpawn spawn)
		{
			spawns.Enqueue (spawn);
		}

		// Remove a reactor
		public void remove (int oid, sbyte state, Point_short position)
		{
			Optional<Reactor> reactor = (Reactor)reactors.get (oid);
			if (reactor)
			{
				reactor.get ().destroy (state, position);
			}
		}

		// Remove all reactors
		public void clear ()
		{
			reactors.clear ();
		}

		public MapObjects get_reactors ()
		{
			return reactors;
		}

		private MapObjects reactors = new MapObjects ();

		private Queue<ReactorSpawn> spawns = new Queue<ReactorSpawn> ();
	}
}