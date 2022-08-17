using System;
using System.Collections.Generic;






namespace ms
{
	// A collection of remote controlled characters on a map
	public class MapChars
	{
		// Draw all characters on a layer
		public void draw(Layer.Id layer, double viewx, double viewy, float alpha)
		{
			chars.draw(layer, viewx, viewy, alpha);
		}
		
		// Update all characters
		public void update(Physics physics)
		{
			for (; spawns.Count > 0; spawns.Dequeue())
			{
				CharSpawn spawn = spawns.Peek();

				int cid = spawn.get_cid();
				Optional<OtherChar> ochar = get_char(cid);

				if (ochar)
				{
					// TODO: Blank
				}
				else
				{
					chars.add(spawn.instantiate());
				}
			}

			chars.update(physics);
		}

		// Spawn a new character, if it has not been spawned yet.
		public void spawn(CharSpawn spawn)
		{
			spawns.Enqueue(spawn);
		}
		// Remove a character
		public void remove(int cid)
		{
			chars.remove(cid);
		}
		// Remove all characters
		public void clear()
		{
			chars.clear();
		}

		// Returns a reference to the MapObjects` object
		public MapObjects get_chars()
		{
			return chars;
		}

		// Update a character's movement
		public void send_movement(int cid, List<Movement> movements)
		{
			Optional<OtherChar> otherchar = get_char (cid);
			if (otherchar)
			{
				otherchar.get ().send_movement(movements);
			}
			else
			{
				AppDebug.Log($"can't get_char,cid:{cid}");
			}
		}
		// Update a character's look
		public void update_look(int cid, LookEntry look)
		{
			Optional<OtherChar> otherchar = get_char (cid);
			if (otherchar)
			{
				otherchar.get ().update_look(look);
			}
		}

		public Optional<OtherChar> findFirstRightClickedChar (Point_short mousePos)
		{
			foreach (var pair in chars)
			{
				if (pair.Value is OtherChar c)
				{
					if (c.bounds ().contains (mousePos))
					{
						return (OtherChar)pair.Value;
					}
				}
			}

			return Optional<OtherChar>.Empty;
		}
		public Optional<OtherChar> get_char(int cid)
		{
			return (OtherChar)chars.get(cid);
		}
		
		public Optional<OtherChar> get_char(string cName)
		{
			foreach (var c in chars)
			{
				if (c.Value is OtherChar oc && oc.get_name ().Equals (cName))
				{
					return oc;
				}
			}
			return new Optional<OtherChar> ();
		}
		private MapObjects chars = new MapObjects();

		private Queue<CharSpawn> spawns = new Queue<CharSpawn>();
	}
}
