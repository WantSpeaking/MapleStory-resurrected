#define USE_NX

using System;





namespace ms
{
	// Information about a bullet type item.
	public class BulletData : Cache<BulletData>
	{
		// Returns whether the bullet was loaded correctly.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool is_valid() const
		public bool is_valid()
		{
			return itemdata.is_valid();
		}
		// Returns whether the bullet was loaded correctly.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: operator bool() const
		public static implicit operator bool(BulletData ImpliedObject)
		{
			return ImpliedObject.is_valid();
		}

		// Returns the weapon attack increase when using this bullet.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: short get_watk() const
		public short get_watk()
		{
			return watk;
		}
		// Returns the bullet animation.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Animation& get_animation() const
		public Animation get_animation()
		{
			return bullet;
		}
		// Returns the general item data.
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const ItemData& get_itemdata() const
		public ItemData get_itemdata()
		{
			return itemdata;
		}

		// Load a bullet from the game files.
		public BulletData(int itemid)
		{
			this.itemdata = ItemData.get(itemid);
			string prefix = "0" + Convert.ToString(itemid / 10000);
			string strid = "0" + Convert.ToString(itemid);
			var src  = ms.wz.wzProvider_item[$"Consume/{prefix}.img"][strid];

			bullet =new Animation(src["bullet"]); 
			watk = src["info"]["incPAD"];
		}

		private readonly ItemData itemdata;

		private Animation bullet = new Animation();
		private short watk;
	}
}

#if USE_NX
#endif
