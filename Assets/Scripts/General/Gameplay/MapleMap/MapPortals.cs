#define USE_NX

using System;
using System.Collections.Generic;
using GameFramework.Resource;
using MapleLib.WzLib;
using ms_Unity;
using UnityEngine;

namespace ms
{
	// Collection of portals on a map
	// Draws and updates portals
	// Also contains methods for using portals and obtaining spawn points
	public class MapPortals:IDisposable
	{
        GameObject gobj_Portal;
        string p_assetPath = "";

        public static void init ()
		{
            /*var src = ms.wz.wzFile_map["MapHelper.img"]["portal"]["game"];

			animations[Portal.Type.HIDDEN] = new Animation (src["ph"]["default"]["portalContinue"]);
			animations[Portal.Type.REGULAR] = new Animation (src["pv"]);

			animations[Portal.Type.SPAWN] = new Animation ();
			animations[Portal.Type.INVISIBLE] = new Animation ();

			animations[Portal.Type.TOUCH] = new Animation ();
			animations[Portal.Type.TYPE5] = new Animation ();
			animations[Portal.Type.WARP] = new Animation ();
			animations[Portal.Type.SPAWN] = new Animation ();
			animations[Portal.Type.SCRIPTED] = new Animation ();
			animations[Portal.Type.SCRIPTED_INVISIBLE] = new Animation ();
			animations[Portal.Type.SCRIPTED_TOUCH] = new Animation ();

			animations[Portal.Type.SCRIPTED_HIDDEN] = new Animation ();
			animations[Portal.Type.SPRING1] = new Animation ();
			animations[Portal.Type.SPRING2] = new Animation ();
			animations[Portal.Type.TYPE14] = new Animation ();*/
        }

        public MapPortals (WzObject node_100000000img_portal, int mapid)
		{
			if (node_100000000img_portal is WzImageProperty property_100000000img_portal)
			{
				foreach (var property_100000000img_portal_0 in property_100000000img_portal.WzProperties)
				{
					sbyte portal_id = string_conversion.or_default (property_100000000img_portal_0.Name, (sbyte)-1);

					if (portal_id < 0)
					{
						continue;
					}


					Portal.Type type = Portal.typebyid (property_100000000img_portal_0["pt"]);
					string name = property_100000000img_portal_0["pn"].ToString ();

					//if (name.Contains ("west00"))
					{
						string target_name = property_100000000img_portal_0["tn"].ToString ();
						int target_id = property_100000000img_portal_0["tm"];
						var script = property_100000000img_portal_0["script"]?.ToString();
                        Point_short position = new Point_short (property_100000000img_portal_0["x"], property_100000000img_portal_0["y"]);
						//AppDebug.Log($"portal target_id:{target_id}\t target_name:{target_name}");
						//Animation animation = Get_AnimationByType (type);
						bool intramap = target_id == mapid;
						portals_by_id.Add ((byte)portal_id, new Portal (null, type, name, intramap, position, target_id, target_name, script));
						//portals_by_id.emplace(piecewise_construct, forward_as_tuple(portal_id), forward_as_tuple(animation, type, name, intramap, position, target_id, target_name));

						portal_ids_by_name.TryAdd (name, (byte)portal_id); //todo 2 add repeatedly
					}
				}
			}


			cooldown = WARPCD;

            //gobj_Portal = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>($"Prefabs/Portal/Map_{string_format.extend_id(mapid, 9)}_Portal"));

            //GameEntry.Resource.LoadAsset($"Assets/GameMain/Prefabs/Portal/Map_{string_format.extend_id(mapid, 9)}_Portal.prefab", typeof(UnityEngine.GameObject), new LoadAssetCallbacks((assetName, asset, duration, userData) => { gobj_Portal = UnityEngine.Object.Instantiate<GameObject>((GameObject)asset); }));

            string strid = string_format.extend_id(mapid, 9);
            string prefix = Convert.ToString(mapid / 100000000);
			p_assetPath = $"Prefabs/Portal/Map{prefix}_Portal";
            var p_asset = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(p_assetPath, $"Map{prefix}_Portal.{strid}");
            gobj_Portal = UnityEngine.Object.Instantiate<GameObject>(p_asset);
        }

		public MapPortals ()
		{
			cooldown = WARPCD;
		}

		public void update (Point_short playerpos)
		{
			//animations[Portal.Type.REGULAR].update ((ushort)Constants.TIMESTEP);
			//animations[Portal.Type.HIDDEN].update ((ushort)Constants.TIMESTEP);

			foreach (var iter in portals_by_id)
			{
				Portal portal = iter.Value;
				switch (portal.get_type ())
				{
					case Portal.Type.HIDDEN:
					case Portal.Type.TOUCH:
						portal.update (new Point_short(playerpos));
						break;
				}
			}

			if (cooldown > 0)
			{
				cooldown--;
			}
		}

		public void draw (Point_short viewpos, float inter)
		{
			if (gobj_Portal == null) return;
            gobj_Portal.transform.position = new Vector3(viewpos.x(), -viewpos.y(), 0);
            /*foreach (var ptit in portals_by_id)
			{
				ptit.Value.draw (viewpos, inter);
			}*/
		}

		public Portal.WarpInfo find_warp_at (Point_short playerpos)
		{
			if (cooldown == 0)
			{
				cooldown = WARPCD;
				Portal.WarpInfo warpInfo;

				foreach (var iter in portals_by_id)
				{
					Portal portal = iter.Value;

					if (portal.bounds ().contains (playerpos))
					{
						warpInfo = portal.getwarpinfo ();
						if (warpInfo.name == "sp" || warpInfo.targetMapid == -1)//跳过出生点 跳过925020100.img/portal/6，6和8 重合，6的tm = -1
						{
							continue;
						}
						return warpInfo;
					}
				}
			}

			return default;
			/*return
			{
			};*/
		}
		private readonly Point_short ABOVE = new Point_short (0, 30);

		public Point_short get_portalPos_by_id (byte portal_id)
		{
			if (portals_by_id.TryGetValue (portal_id, out var portalPos))
			{
				return portalPos.get_position () - ABOVE;
			}

			return Point_short.zero;
		}

		public Portal get_portal_by_id (byte portal_id)
		{
			if (!portals_by_id.TryGetValue (portal_id, out var portalPos))
			{
				AppDebug.LogError ($"can't find portal id:{portal_id}");
			}

			return portalPos;
		}
		public Point_short get_portal_by_name (string portal_name)
		{
			if (portal_ids_by_name.TryGetValue (portal_name, out var portalId))
			{
				return get_portalPos_by_id (portalId);
			}

			return Point_short.zero;
		}

		private Animation Get_AnimationByType (Portal.Type portalType)
		{
			Animation ani;
			var src = ms.wz.wzFile_map["MapHelper.img"]["portal"]["game"];
            //ani = new Animation(src["pv"]);
            /*if (portalType == Portal.Type.HIDDEN)
			{
				ani = new Animation (src["ph"]["default"]["portalContinue"]);
			}
			else if (portalType == Portal.Type.REGULAR)
			{
				ani = new Animation (src["pv"]);
			}
			else
			{
				ani = new Animation ();
			}*/

            /*if (portalType == Portal.Type.HIDDEN)
            {
                ani = new Animation(src["ph"]["default"]["portalContinue"]);
            }
            else if (portalType == Portal.Type.SPAWN)
            {
                ani = new Animation();
            }
            else
            {
                ani = new Animation(src["pv"]);
            }*/
            
            if (portalType == Portal.Type.HIDDEN)
			{
				ani = new Animation (src["ph"]["default"]["portalContinue"]);
			}
			else if (portalType == Portal.Type.REGULAR || portalType == Portal.Type.SCRIPTED)
			{
				ani = new Animation (src["pv"]);
			}
			else
			{
				ani = new Animation ();
			}
            return ani;
		}

        public void Dispose()
        {
			/*foreach (var p in portals_by_id)
			{
				p.Value?.Dispose();
			}*/

            UnityEngine.Object.Destroy(gobj_Portal);

            AppDebug.Log("Dispose MapPortals");
            //AssetBundleLoaderMgr.Instance.UnloadAssetBundle(p_assetPath);
            //GameEntry.Resource.UnloadUnusedAssets(true);
        }

        //private static Dictionary<Portal.Type, Animation> animations = new Dictionary<Portal.Type, Animation> ();

		private Dictionary<byte, Portal> portals_by_id = new Dictionary<byte, Portal> ();
		private Dictionary<string, byte> portal_ids_by_name = new Dictionary<string, byte> ();

		private const short WARPCD = 48;
		private short cooldown;

		public IReadOnlyDictionary<byte, Portal> get_portals() => portals_by_id;
    }
}


#if USE_NX
#endif