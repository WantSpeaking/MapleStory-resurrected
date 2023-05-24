using UnityEngine;



namespace ms
{
	// Base for objects on a map, e.g., Mobs, NPCs, Characters, etc.
	public abstract class MapObject : System.IDisposable
	{
		public GameObject MapGameObject;
		protected MapObject(int o, Point_short p )
		{
			this.oid = o;
			set_position (new Point_short (p));
			active = true;
			MapGameObject = new GameObject ($"{GetType().Name}_{oid}");
			//MapGameObject.AddComponent(GetType ());
		}
		
		public virtual void Dispose()
		{
			UnityEngine.GameObject.Destroy( MapGameObject );
		}

		// Draws the object at the given position and with the specified interpolation.
		public abstract void draw(double viewx, double viewy, float alpha);

		// Updates the object and returns the updated layer.
		public virtual sbyte update(Physics physics)
		{
			physics.move_object(phobj);

			return (sbyte)phobj.fhlayer;
		}
		// Reactivates the object.
		public virtual void makeactive()
		{
			active = true;
		}
		// Deactivates the object.
		public virtual void deactivate()
		{
			DisposeTextureBeforeDeactivate ();
			active = false;
		}
		// Checks whether this object is active.
		public virtual bool is_active()
		{
			return active;
		}
		// Obtains the layer used to determine the drawing order on the map.
		public virtual sbyte get_layer()
		{
			return phobj.fhlayer;
		}

		// Changes the objects position.
		public void set_position(short x, short y)
		{
			phobj.set_x(x);
			phobj.set_y(y);
		}
		// Changes the objects position.
		public void set_position(Point_short position)
		{
			short x = position.x();
			short y = position.y();
			set_position(x, y);
		}

		// Returns the object id unique to every object on one map.
		public int get_oid()
		{
			return oid;
		}
		// Returns the current position.
		public Point_short get_position()
		{
			return phobj.get_position();
		}

		protected virtual void DisposeTextureBeforeDeactivate ()
		{
			
		}

		protected PhysicsObject phobj = new PhysicsObject();
		protected int oid;
		protected bool active;
	}
}
