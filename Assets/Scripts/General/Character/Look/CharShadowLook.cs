using System;
using System.Collections.Generic;
using MapleLib.WzLib;

namespace ms
{
	public class CharShadowLook:IDisposable
	{
		private Dictionary<Stance.Id, Animation> stance_animation_dict = new Dictionary<Stance.Id, Animation> ();
		public CharShadowLook ()
		{
			WzObject skillwz_411img_skill_4111002_special = wz.wzFile_skill["411.img"]["skill"]["4111002"]["special"];
			if (skillwz_411img_skill_4111002_special == null) return;
			foreach (var child in skillwz_411img_skill_4111002_special)
			{
				
			}

			foreach (var iter in Stance.names)
			{
				Stance.Id stance = iter.Key;
				string stancename = iter.Value;

				var skillwz_411img_skill_4111002_special_alert = skillwz_411img_skill_4111002_special[stancename];

				if (skillwz_411img_skill_4111002_special_alert == null)
				{
					continue;
				}

				stance_animation_dict.TryAdd (stance, new Animation (skillwz_411img_skill_4111002_special_alert));
			}
		}

		public void draw (float alpha,CharLook look)
		{
			if (stance_animation_dict.TryGetValue (look.get_lastDraw_stance (),out var animation))
			{
				var FlipX = look.get_lastDraw_args ().FlipX;
				DrawArgument faceargs = look.get_lastDraw_args () + new Point_short((short)((FlipX?-1:1)*40),0);
				
				animation.draw(faceargs, alpha);
			}
		}

		public void Dispose ()
		{
			foreach (var pair in stance_animation_dict)
			{
				pair.Value.Dispose ();
			}
			stance_animation_dict.Clear ();
			stance_animation_dict = null;
		}
	}
}