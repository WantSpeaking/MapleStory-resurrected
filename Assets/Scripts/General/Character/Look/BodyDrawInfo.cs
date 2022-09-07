#define USE_NX

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helper;
using ms.Helper;
using MapleLib.WzLib;





namespace ms
{
	// A frame of animation for a skill or similar 'meta-stance' 
	// This simply redirects to a different stance and frame to use
	public class BodyAction
	{
		public BodyAction (WzObject Characterwz00002000img_airstrike_3)
		{
			if (Characterwz00002000img_airstrike_3.FullPath.Contains ("burster2"))
			{
				var f = 0;
			}
			stance = Stance.by_string (Characterwz00002000img_airstrike_3["action"].ToString ());
			frame = Characterwz00002000img_airstrike_3["frame"];
			move = Characterwz00002000img_airstrike_3["move"];

			short sgndelay = Characterwz00002000img_airstrike_3["delay"];

			if (sgndelay == 0)
			{
				sgndelay = 100;
			}

			if (sgndelay > 0)
			{
				delay = (ushort)sgndelay;
				attackframe = true;
			}
			else if (sgndelay < 0)
			{
				delay = (ushort)-sgndelay;
				attackframe = false;
			}
		}

		public BodyAction ()
		{
		}

		public bool isattackframe ()
		{
			return attackframe;
		}

		public byte get_frame ()
		{
			return frame;
		}

		public ushort get_delay ()
		{
			return delay;
		}

		public Point_short get_move ()
		{
			return move;
		}

		public Stance.Id get_stance ()
		{
			return stance;
		}

		private Stance.Id stance;
		private byte frame;
		private ushort delay;
		private Point_short move = new Point_short ();
		private bool attackframe;
	}

	public class BodyDrawInfo
	{
		public void init ()
		{
			init_Dict ();
			var node_Characterwz_00002000img = ms.wz.wzFile_character["00002000.img"];
			var node_Characterwz_00012000img = ms.wz.wzFile_character["00012000.img"];

			if (node_Characterwz_00002000img is WzImage property_Characterwz_00002000img)
			{
				foreach (var property_Characterwz_00002000img_airstrike in property_Characterwz_00002000img.WzProperties)
				{
					string ststr = property_Characterwz_00002000img_airstrike.Name;

					ushort attackdelay = 0;

					foreach (var property_Characterwz_00002000img_fly_0 in property_Characterwz_00002000img_airstrike.WzProperties)
					{
						/*for (byte frame = 0; WzObject framenode = property_Characterwz_00002000img_airstrike[frame];
					++frame)*/
						{
							byte.TryParse (property_Characterwz_00002000img_fly_0.Name, out var frame);
							bool isaction = property_Characterwz_00002000img_fly_0["action"] != null;

							if (isaction)
							{
								BodyAction action = new BodyAction (property_Characterwz_00002000img_fly_0);
								body_actions.TryAdd (ststr);
								body_actions[ststr].TryAdd (frame, action);

								//body_actions[ststr][frame] = action;

								if (action.isattackframe ())
								{
									if (!attack_delays.ContainsKey (ststr))
									{
										var tempList = new List<ushort> ();
										attack_delays.Add (ststr, tempList);
									}

									attack_delays[ststr].Add (attackdelay);
								}

								attackdelay += action.get_delay ();
							}
							else
							{
								Stance.Id stance = Stance.by_string (ststr);
								short delay = property_Characterwz_00002000img_fly_0["delay"];

								if (delay <= 0)
								{
									delay = 100;
								}

								//if (stance == Stance.Id.STAND1)
								{
									stance_delays[(int)stance][frame] = (ushort)delay;
									Dictionary<Body.Layer, Dictionary<string, Point_short>> bodyshiftmap = new Dictionary<Body.Layer, Dictionary<string, Point_short>> ();
									bodyshiftmap.TryAdd (Body.Layer.ARM);
									bodyshiftmap.TryAdd (Body.Layer.BODY);
									bodyshiftmap.TryAdd (Body.Layer.HEAD);
									bodyshiftmap.TryAdd (Body.Layer.NONE);
									bodyshiftmap.TryAdd (Body.Layer.NUM_LAYERS);
									bodyshiftmap.TryAdd (Body.Layer.ARM_OVER_HAIR);
									bodyshiftmap.TryAdd (Body.Layer.ARM_BELOW_HEAD);
									bodyshiftmap.TryAdd (Body.Layer.HAND_OVER_HAIR);
									bodyshiftmap.TryAdd (Body.Layer.HAND_OVER_WEAPON);
									bodyshiftmap.TryAdd (Body.Layer.HAND_BELOW_WEAPON);
									bodyshiftmap.TryAdd (Body.Layer.ARM_BELOW_HEAD_OVER_MAIL);
									bodyshiftmap.TryAdd (Body.Layer.ARM_OVER_HAIR_BELOW_WEAPON);
									bodyshiftmap[Body.Layer.ARM].TryAdd ("hand");
									bodyshiftmap[Body.Layer.ARM].TryAdd ("navel");
									bodyshiftmap[Body.Layer.ARM_OVER_HAIR].TryAdd ("hand");
									bodyshiftmap[Body.Layer.ARM_OVER_HAIR].TryAdd ("navel");
									bodyshiftmap[Body.Layer.BODY].TryAdd ("navel");
									bodyshiftmap[Body.Layer.BODY].TryAdd ("neck");
									bodyshiftmap[Body.Layer.HEAD].TryAdd ("brow");
									bodyshiftmap[Body.Layer.HEAD].TryAdd ("neck");
									bodyshiftmap[Body.Layer.HAND_BELOW_WEAPON].TryAdd ("handMove");

									if (property_Characterwz_00002000img_fly_0?.WzProperties != null) //todo 2 WzProperties == null?
									{
										foreach (var property_Characterwz_00002000img_fly_0_arm in property_Characterwz_00002000img_fly_0.WzProperties)
										{
											string part = property_Characterwz_00002000img_fly_0_arm.Name;

											if (part != "delay" && part != "face")
											{
												string zstr = property_Characterwz_00002000img_fly_0_arm["z"]?.ToString ();
												if (string.IsNullOrEmpty (zstr))
													continue;
												Body.Layer z = Body.layer_by_name (zstr);

												foreach (var property_Characterwz_00002000img_fly_0_arm_hand in property_Characterwz_00002000img_fly_0_arm["map"].WzProperties)
												{
													//bodyshiftmap.TryAdd (z, new Dictionary<string, Point_short> ());

													bodyshiftmap[z].TryAdd (property_Characterwz_00002000img_fly_0_arm_hand.Name, property_Characterwz_00002000img_fly_0_arm_hand.GetPoint ().ToMSPoint (), true);
												}
											}
										}
									}

									//var node_Characterwz_00012000img_front_head_map = node_Characterwz_00012000img[ststr][frame.ToString()]["head"]["map"];//todo 2 has frame?
									var stand1 = node_Characterwz_00012000img["stand1"];
									var frame0 = stand1["0"];
									var head = frame0["head"];
									var map = head["map"];

									var node_Characterwz_00012000img_front_head_map = map;
									//var node_Characterwz_00012000img_front_head_map = node_Characterwz_00012000img[ststr][frame.ToString ()]["head"]["map"];
									if (node_Characterwz_00012000img_front_head_map is WzImageProperty property_Characterwz_00012000img_front_head_map)
									{
										foreach (var property_Characterwz_00012000img_front_head_map_brow in property_Characterwz_00012000img_front_head_map.WzProperties)
										{
											//AppDebug.Log ($"{property_Characterwz_00012000img_front_head_map_brow.FullPath}");
											//bodyshiftmap.TryAdd (Body.Layer.HEAD);
											bodyshiftmap[Body.Layer.HEAD].TryAdd (property_Characterwz_00012000img_front_head_map_brow.Name, property_Characterwz_00012000img_front_head_map_brow.GetPoint ().ToMSPoint (), true);
										}
									}

									#region Debug

									StringBuilder sb = new StringBuilder ();

									foreach (var key in bodyshiftmap.Keys)
									{
										sb.Append (key + "|");
									}

									//AppDebug.Log ($"bodyshiftmap.Keys:  \t {sb}");

									sb.Clear ();
									foreach (var key in bodyshiftmap[Body.Layer.BODY].Keys)
									{
										sb.Append (key + "|");
									}

									//AppDebug.Log ($"bodyshiftmap[Body.Layer.BODY].Keys: \t {sb}");

									sb.Clear ();
									foreach (var key in bodyshiftmap[Body.Layer.HAND_BELOW_WEAPON].Keys)
									{
										sb.Append (key + "|");
									}

									//AppDebug.Log ($"bodyshiftmap[Body.Layer.HAND_BELOW_WEAPON].Keys: \t  {sb}");

									sb.Clear ();
									foreach (var key in bodyshiftmap[Body.Layer.HEAD].Keys)
									{
										sb.Append (key + "|");
									}

									//AppDebug.Log ($"bodyshiftmap[Body.Layer.HEAD].Keys:  \t {sb}");

									sb.Clear ();
									foreach (var key in bodyshiftmap[Body.Layer.ARM].Keys)
									{
										sb.Append (key + "|");
									}

									#endregion


									//AppDebug.Log ($"bodyshiftmap[Body.Layer.ARM].Keys:  \t {sb}");
									//AppDebug.Log ($"{stance} {bodyshiftmap.Keys} {bodyshiftmap[Body.Layer.BODY].Keys}");
									body_positions[(int)stance].TryAdd (frame, bodyshiftmap[Body.Layer.BODY]["navel"]);
									arm_positions[(int)stance].TryAdd (frame, bodyshiftmap.Any (x => x.Key == Body.Layer.ARM) ? bodyshiftmap[Body.Layer.ARM]["hand"] - bodyshiftmap[Body.Layer.ARM]["navel"] + bodyshiftmap[Body.Layer.BODY]["navel"] : bodyshiftmap[Body.Layer.ARM_OVER_HAIR]["hand"] - bodyshiftmap[Body.Layer.ARM_OVER_HAIR]["navel"] + bodyshiftmap[Body.Layer.BODY]["navel"]);
									hand_positions[(int)stance].TryAdd (frame, bodyshiftmap[Body.Layer.HAND_BELOW_WEAPON]["handMove"]);
									//var dsf = bodyshiftmap[Body.Layer.BODY];
									var bodyneck = bodyshiftmap[Body.Layer.BODY]["neck"];
									var headneck = bodyshiftmap[Body.Layer.HEAD]["neck"];
									var delta = bodyneck - headneck;
									head_positions[(int)stance].TryAdd (frame, bodyshiftmap[Body.Layer.BODY]["neck"] - bodyshiftmap[Body.Layer.HEAD]["neck"]);
									face_positions[(int)stance].TryAdd (frame, bodyshiftmap[Body.Layer.BODY]["neck"] - bodyshiftmap[Body.Layer.HEAD]["neck"] + bodyshiftmap[Body.Layer.HEAD]["brow"]);
									hair_positions[(int)stance].TryAdd (frame, bodyshiftmap[Body.Layer.HEAD]["brow"] - bodyshiftmap[Body.Layer.HEAD]["neck"] + bodyshiftmap[Body.Layer.BODY]["neck"]);


									var result1 = bodyshiftmap[Body.Layer.BODY]["neck"] - bodyshiftmap[Body.Layer.HEAD]["neck"];
									var result2 = result1 + bodyshiftmap[Body.Layer.HEAD]["brow"];
									//AppDebug.Log ($"Body.Layer.BODY[neck]: {bodyshiftmap[Body.Layer.BODY]["neck"]}\t Body.Layer.HEAD[neck]: {bodyshiftmap[Body.Layer.HEAD]["neck"]}\t Body.Layer.HEAD[brow]: {bodyshiftmap[Body.Layer.HEAD]["brow"]}\t result1: {result1}\t result2: {result2}");
								}

								/*body_positions[(int)stance][frame] = bodyshiftmap[Body.Layer.BODY]["navel"];
								arm_positions[(int)stance][frame] = bodyshiftmap.Any (x => x.Key == Body.Layer.ARM) ? bodyshiftmap[Body.Layer.ARM]["hand"] - bodyshiftmap[Body.Layer.ARM]["navel"] + bodyshiftmap[Body.Layer.BODY]["navel"] : bodyshiftmap[Body.Layer.ARM_OVER_HAIR]["hand"] - bodyshiftmap[Body.Layer.ARM_OVER_HAIR]["navel"] + bodyshiftmap[Body.Layer.BODY]["navel"];
								hand_positions[(int)stance][frame] = bodyshiftmap[Body.Layer.HAND_BELOW_WEAPON]["handMove"];
								head_positions[(int)stance][frame] = bodyshiftmap[Body.Layer.BODY]["neck"] - bodyshiftmap[Body.Layer.HEAD]["neck"];
								face_positions[(int)stance][frame] = bodyshiftmap[Body.Layer.BODY]["neck"] - bodyshiftmap[Body.Layer.HEAD]["neck"] + bodyshiftmap[Body.Layer.HEAD]["brow"];
								hair_positions[(int)stance][frame] = bodyshiftmap[Body.Layer.HEAD]["brow"] - bodyshiftmap[Body.Layer.HEAD]["neck"] + bodyshiftmap[Body.Layer.BODY]["neck"];*/
							}
						}
					}
				}
			}
		}

		public Point_short get_body_position (Stance.Id stance, byte frame)
		{
			var pos = new Point_short ();
			body_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;

		}

		public Point_short get_arm_position (Stance.Id stance, byte frame)
		{
			var pos = new Point_short ();
			arm_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;

		}

		public Point_short get_hand_position (Stance.Id stance, byte frame)
		{
			var pos = new Point_short ();
			hand_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;

		}

		public Point_short get_head_position (Stance.Id stance, byte frame)
		{
			var pos = new Point_short ();
			head_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;


		}

		public Point_short gethairpos (Stance.Id stance, byte frame)
		{
			var pos = new Point_short ();
			hair_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;

		}

		public Point_short getfacepos (Stance.Id stance, byte frame, bool flip = false)
		{
			if (!face_positions[(int)stance].TryGetValue (frame, out var pos))
			{
				pos = new Point_short ();
			}

			if (flip)
				pos = new Point_short ((short)-pos.x (), pos.y ());

			//AppDebug.Log ($"flip:{flip} stance: {stance} \t getfacepos: {pos} frame: {frame}");

			return pos;

		}

		public byte nextframe (Stance.Id stance, byte frame)
		{
			if (stance_delays[(int)stance].Any (x => x.Key == frame + 1))
			{
				return (byte)(frame + 1);
			}
			else
			{
				return 0;
			}
		}

		public ushort get_delay (Stance.Id stance, byte frame)
		{
			ushort delay = 100;
			stance_delays[(int)stance].TryGetValue (frame, out delay);
			return delay;

		}
		public ushort get_total_delay (Stance.Id stance)
		{
			return (ushort)stance_delays[(int)stance].Values.Sum (delay => delay);
		}
		public ushort get_attackdelay (string action, uint no)
		{
			ushort attackdelay = 0;
			attack_delays.TryGetValue (action, out var delays);
			if (no < delays.Count)
			{
				attackdelay = delays[(int)no];
			}

			return attackdelay;

		}
		public ushort get_total_attackdelay (string action)
		{
			ushort attackdelay = 0;

			if (attack_delays.TryGetValue (action, out var delays))
			{
				attackdelay = (ushort)delays.Sum (delay => delay);
			}
			return attackdelay;
		}
		public byte next_actionframe (string action, byte frame)
		{
			byte nextFrame = 0;
			body_actions.TryGetValue (action, out var actions);
			if (actions.Any (x => x.Key == frame + 1))
			{
				nextFrame = (byte)(frame + 1);
			}

			return nextFrame;



		}

		public BodyAction get_action (string action, byte frame)
		{
			BodyAction bodyAction = null;
			body_actions.TryGetValue (action, out var actions);
			actions?.TryGetValue (frame, out bodyAction);
			return bodyAction;


		}

		private Dictionary<byte, Point_short>[] body_positions = new Dictionary<byte, Point_short>[EnumUtil.GetEnumLength<Stance.Id> ()]; //todo 2 dict is null
		private Dictionary<byte, Point_short>[] arm_positions = new Dictionary<byte, Point_short>[EnumUtil.GetEnumLength<Stance.Id> ()];
		private Dictionary<byte, Point_short>[] hand_positions = new Dictionary<byte, Point_short>[EnumUtil.GetEnumLength<Stance.Id> ()];
		private Dictionary<byte, Point_short>[] head_positions = new Dictionary<byte, Point_short>[EnumUtil.GetEnumLength<Stance.Id> ()];
		private Dictionary<byte, Point_short>[] hair_positions = new Dictionary<byte, Point_short>[EnumUtil.GetEnumLength<Stance.Id> ()];
		private Dictionary<byte, Point_short>[] face_positions = new Dictionary<byte, Point_short>[EnumUtil.GetEnumLength<Stance.Id> ()];
		private Dictionary<byte, ushort>[] stance_delays = new Dictionary<byte, ushort>[EnumUtil.GetEnumLength<Stance.Id> ()];

		private Dictionary<string, Dictionary<byte, BodyAction>> body_actions = new Dictionary<string, Dictionary<byte, BodyAction>> ();
		private Dictionary<string, List<ushort>> attack_delays = new Dictionary<string, List<ushort>> ();

		private void init_Dict ()
		{
			for (int i = 0; i < EnumUtil.GetEnumLength<Stance.Id> (); i++)
			{
				body_positions[i] = new Dictionary<byte, Point_short> ();
				arm_positions[i] = new Dictionary<byte, Point_short> ();
				hand_positions[i] = new Dictionary<byte, Point_short> ();
				head_positions[i] = new Dictionary<byte, Point_short> ();
				hair_positions[i] = new Dictionary<byte, Point_short> ();
				face_positions[i] = new Dictionary<byte, Point_short> ();
				stance_delays[i] = new Dictionary<byte, ushort> ();
			}
		}
	}
}


#if USE_NX
#endif