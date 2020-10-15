#define USE_NX

using System.Collections.Generic;
using System.Linq;
using System.Text;
using ms.Helper;
using MapleLib.WzLib;
using UnityEngine;

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
	// A frame of animation for a skill or similar 'meta-stance' 
	// This simply redirects to a different stance and frame to use
	public class BodyAction
	{
		public BodyAction (WzObject Characterwz00002000img_airstrike_3)
		{
			stance = Stance.by_string (Characterwz00002000img_airstrike_3["action"].ToString ());
			frame = Characterwz00002000img_airstrike_3["frame"]?.GetInt ().ToByte ()??0;
			move = Characterwz00002000img_airstrike_3["move"]?.GetPoint ().ToMSPoint ();

			short sgndelay = Characterwz00002000img_airstrike_3["delay"]?.GetShort ()??0;

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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isattackframe() const
		public bool isattackframe ()
		{
			return attackframe;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte get_frame() const
		public byte get_frame ()
		{
			return frame;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_delay() const
		public ushort get_delay ()
		{
			return delay;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_move() const
		public Point<short> get_move ()
		{
			return move;
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Stance::Id get_stance() const
		public Stance.Id get_stance ()
		{
			return stance;
		}

		private Stance.Id stance;
		private byte frame;
		private ushort delay;
		private Point<short> move = new Point<short> ();
		private bool attackframe;
	}

	public class BodyDrawInfo
	{
		public void init ()
		{
			init_Dict ();
			var node_Characterwz_00002000img = nl.nx.wzFile_character["00002000.img"];
			var node_Characterwz_00012000img = nl.nx.wzFile_character["00012000.img"];

			if (node_Characterwz_00002000img is WzImage property_Characterwz_00002000img)
			{
				foreach (var property_Characterwz_00002000img_airstrike in property_Characterwz_00002000img.WzProperties)
				{
					string ststr = property_Characterwz_00002000img_airstrike.Name;

					ushort attackdelay = 0;

					foreach (var property_Characterwz_00002000img_fly_0 in property_Characterwz_00002000img_airstrike.WzProperties)
					{
						/*for (byte frame = 0; nl.node framenode = property_Characterwz_00002000img_airstrike[frame];
					++frame)*/
						{
							byte.TryParse (property_Characterwz_00002000img_fly_0.Name, out var frame);
							bool isaction = property_Characterwz_00002000img_fly_0["action"]?.PropertyType == WzPropertyType.String;

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
								short delay = property_Characterwz_00002000img_fly_0["delay"]?.GetShort () ?? 0;

								if (delay <= 0)
								{
									delay = 100;
								}
								//if (stance == Stance.Id.STAND1)
								{
								stance_delays[(int)stance][frame] = (ushort)delay;
								Dictionary<Body.Layer, Dictionary<string, Point<short>>> bodyshiftmap = new Dictionary<Body.Layer, Dictionary<string, Point<short>>> ();
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

								if (property_Characterwz_00002000img_fly_0?.WzProperties != null)//todo WzProperties == null?
								{
									foreach (var property_Characterwz_00002000img_fly_0_arm in property_Characterwz_00002000img_fly_0.WzProperties)
									{
										string part = property_Characterwz_00002000img_fly_0_arm.Name;

										if (part != "delay" && part != "face")
										{
											string zstr = property_Characterwz_00002000img_fly_0_arm["z"]?.ToString ();
											if (string.IsNullOrEmpty (zstr)) continue;
											Body.Layer z = Body.layer_by_name (zstr);

											foreach (var property_Characterwz_00002000img_fly_0_arm_hand in property_Characterwz_00002000img_fly_0_arm["map"].WzProperties)
											{
												//bodyshiftmap.TryAdd (z, new Dictionary<string, Point<short>> ());

												bodyshiftmap[z].TryAdd (property_Characterwz_00002000img_fly_0_arm_hand.Name, property_Characterwz_00002000img_fly_0_arm_hand.GetPoint ().ToMSPoint (),true);
											}
										}
									}
								}

								//var node_Characterwz_00012000img_front_head_map = node_Characterwz_00012000img[ststr][frame.ToString()]["head"]["map"];//todo has frame?
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
										//Debug.Log ($"{property_Characterwz_00012000img_front_head_map_brow.FullPath}");
										//bodyshiftmap.TryAdd (Body.Layer.HEAD);
										bodyshiftmap[Body.Layer.HEAD].TryAdd (property_Characterwz_00012000img_front_head_map_brow.Name, property_Characterwz_00012000img_front_head_map_brow.GetPoint ().ToMSPoint (),true);
									}
								}

								#region Debug

								StringBuilder sb = new StringBuilder ();

								foreach (var key in bodyshiftmap.Keys)
								{
									sb.Append (key + "|");
								}

								//Debug.Log ($"bodyshiftmap.Keys:  \t {sb}");

								sb.Clear ();
								foreach (var key in bodyshiftmap[Body.Layer.BODY].Keys)
								{
									sb.Append (key + "|");
								}

								//Debug.Log ($"bodyshiftmap[Body.Layer.BODY].Keys: \t {sb}");

								sb.Clear ();
								foreach (var key in bodyshiftmap[Body.Layer.HAND_BELOW_WEAPON].Keys)
								{
									sb.Append (key + "|");
								}

								//Debug.Log ($"bodyshiftmap[Body.Layer.HAND_BELOW_WEAPON].Keys: \t  {sb}");

								sb.Clear ();
								foreach (var key in bodyshiftmap[Body.Layer.HEAD].Keys)
								{
									sb.Append (key + "|");
								}

								//Debug.Log ($"bodyshiftmap[Body.Layer.HEAD].Keys:  \t {sb}");

								sb.Clear ();
								foreach (var key in bodyshiftmap[Body.Layer.ARM].Keys)
								{
									sb.Append (key + "|");
								}

								#endregion


								
								//Debug.Log ($"bodyshiftmap[Body.Layer.ARM].Keys:  \t {sb}");
								//Debug.Log ($"{stance} {bodyshiftmap.Keys} {bodyshiftmap[Body.Layer.BODY].Keys}");
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

								
									var result1 = bodyshiftmap[Body.Layer.BODY]["neck"] -bodyshiftmap[Body.Layer.HEAD]["neck"] ;
									var result2 = result1 + bodyshiftmap[Body.Layer.HEAD]["brow"];
									//Debug.Log ($"Body.Layer.BODY[neck]: {bodyshiftmap[Body.Layer.BODY]["neck"]}\t Body.Layer.HEAD[neck]: {bodyshiftmap[Body.Layer.HEAD]["neck"]}\t Body.Layer.HEAD[brow]: {bodyshiftmap[Body.Layer.HEAD]["brow"]}\t result1: {result1}\t result2: {result2}");
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

					int teff;
				}
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_body_position(Stance::Id stance, byte frame) const
		public Point<short> get_body_position (Stance.Id stance, byte frame)
		{
			var pos = new Point<short> ();
			body_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;
			/*var iter = body_positions[(int)stance].find (frame);

			if (iter == body_positions[(int)stance].end ())
			{
				return
				{
				}
				;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_arm_position(Stance::Id stance, byte frame) const
		public Point<short> get_arm_position (Stance.Id stance, byte frame)
		{
			var pos = new Point<short> ();
			arm_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;
			/*var iter = arm_positions[(int)stance].find (frame);

			if (iter == arm_positions[(int)stance].end ())
			{
				return
				{
				}
				;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_hand_position(Stance::Id stance, byte frame) const
		public Point<short> get_hand_position (Stance.Id stance, byte frame)
		{
			var pos = new Point<short> ();
			hand_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;
			/*var iter = hand_positions[(int)stance].find (frame);

			if (iter == hand_positions[(int)stance].end ())
			{
				return
				{
				}
				;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> get_head_position(Stance::Id stance, byte frame) const
		public Point<short> get_head_position (Stance.Id stance, byte frame)
		{
			var pos = new Point<short> ();
			head_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;

			/*var iter = head_positions[(int)stance].find (frame);

			if (iter == head_positions[(int)stance].end ())
			{
				return
				{
				}
				;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> gethairpos(Stance::Id stance, byte frame) const
		public Point<short> gethairpos (Stance.Id stance, byte frame)
		{
			var pos = new Point<short> ();
			hair_positions[(int)stance].TryGetValue (frame, out pos);
			return pos;
			/*var iter = hair_positions[(int)stance].find (frame);

			if (iter == hair_positions[(int)stance].end ())
			{
				return
				{
				}
				;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Point<short> getfacepos(Stance::Id stance, byte frame) const
		public Point<short> getfacepos (Stance.Id stance, byte frame,bool flip=false)
		{
			var pos = new Point<short> ();
			face_positions[(int)stance].TryGetValue (frame, out pos);
			//Debug.Log ($"stance: {stance} \t getfacepos: {pos} frame: {frame}");
			if(flip)pos = new Point<short> ((short)-pos.x (),pos.y ());
			return pos;
			/*var iter = face_positions[(int)stance].find (frame);

			if (iter == face_positions[(int)stance].end ())
			{
				return
				{
				}
				;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte nextframe(Stance::Id stance, byte frame) const
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

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_delay(Stance::Id stance, byte frame) const
		public ushort get_delay (Stance.Id stance, byte frame)
		{
			ushort delay = 100;
			stance_delays[(int)stance].TryGetValue (frame, out delay);
			return delay;
			/*var iter = stance_delays[(int)stance].find (frame);

			if (iter == stance_delays[(int)stance].end ())
			{
				return 100;
			}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			return iter.second;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ushort get_attackdelay(string action, uint no) const
		public ushort get_attackdelay (string action, uint no)
		{
			ushort attackdelay = 0;
			attack_delays.TryGetValue (action, out var delays);
			if (no < delays.Count)
			{
				attackdelay = delays[(int)no];
			}

			return attackdelay;
			/*var action_iter = attack_delays.find (action);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			if (action_iter != attack_delays.end ())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				if (no < action_iter.second.size ())
				{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
					return action_iter.second[no];
				}
			}

			return 0;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: byte next_actionframe(string action, byte frame) const
		public byte next_actionframe (string action, byte frame)
		{
			byte attackdelay = 0;
			body_actions.TryGetValue (action, out var actions);
			if (actions.Any (x => x.Key == frame + 1))
			{
				attackdelay = (byte)(frame + 1);
			}

			return attackdelay;


			/*var action_iter = body_actions.find (action);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			if (action_iter != body_actions.end ())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				if (action_iter.second.count (frame + 1))
				{
					return frame + 1;
				}
			}

			return 0;*/
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const BodyAction* get_action(string action, byte frame) const
		public BodyAction get_action (string action, byte frame)
		{
			BodyAction bodyAction = null;
			body_actions.TryGetValue (action, out var actions);
			actions?.TryGetValue (frame, out bodyAction);
			return bodyAction;

			/*var action_iter = body_actions.find (action);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
			if (action_iter != body_actions.end ())
			{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				var frame_iter = action_iter.second.find (frame);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Iterators are only converted within the context of 'while' and 'for' loops:
				if (frame_iter != action_iter.second.end ())
				{
					return (frame_iter.second);
				}
			}

			return null;*/
		}

		private Dictionary<byte, Point<short>>[] body_positions = new Dictionary<byte, Point<short>>[(int)Stance.Id.LENGTH]; //todo dict is null
		private Dictionary<byte, Point<short>>[] arm_positions = new Dictionary<byte, Point<short>>[(int)Stance.Id.LENGTH];
		private Dictionary<byte, Point<short>>[] hand_positions = new Dictionary<byte, Point<short>>[(int)Stance.Id.LENGTH];
		private Dictionary<byte, Point<short>>[] head_positions = new Dictionary<byte, Point<short>>[(int)Stance.Id.LENGTH];
		private Dictionary<byte, Point<short>>[] hair_positions = new Dictionary<byte, Point<short>>[(int)Stance.Id.LENGTH];
		private Dictionary<byte, Point<short>>[] face_positions = new Dictionary<byte, Point<short>>[(int)Stance.Id.LENGTH];
		private Dictionary<byte, ushort>[] stance_delays = new Dictionary<byte, ushort>[(int)Stance.Id.LENGTH];

		private Dictionary<string, Dictionary<byte, BodyAction>> body_actions = new Dictionary<string, Dictionary<byte, BodyAction>> ();
		private Dictionary<string, List<ushort>> attack_delays = new Dictionary<string, List<ushort>> ();

		private void init_Dict ()
		{
			for (int i = 0; i < (int)Stance.Id.LENGTH; i++)
			{
				body_positions[i] = new Dictionary<byte, Point<short>> ();
				arm_positions[i] = new Dictionary<byte, Point<short>> ();
				hand_positions[i] = new Dictionary<byte, Point<short>> ();
				head_positions[i] = new Dictionary<byte, Point<short>> ();
				hair_positions[i] = new Dictionary<byte, Point<short>> ();
				face_positions[i] = new Dictionary<byte, Point<short>> ();
				stance_delays[i] = new Dictionary<byte, ushort> ();
			}
		}
	}
}


#if USE_NX
#endif