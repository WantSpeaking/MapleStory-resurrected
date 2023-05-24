#define USE_NX

using System;
using System.Collections.Generic;
using ms.Helper;
using MapleLib.WzLib;





namespace ms
{
    public class Body:IDisposable
    {
        public enum Layer
        {
            NONE,
            BODY,
            ARM,
            ARM_BELOW_HEAD,
            ARM_BELOW_HEAD_OVER_MAIL,
            ARM_OVER_HAIR,
            ARM_OVER_HAIR_BELOW_WEAPON,
            HAND_BELOW_WEAPON,
            HAND_OVER_HAIR,
            HAND_OVER_WEAPON,
            HEAD,
            NUM_LAYERS
        }

        public Body(int skin, BodyDrawInfo drawinfo)
        {
            init_Dict();

            string strid = string_format.extend_id(skin, 2);
            var bodynode = ms.wz.wzFile_character["000020" + strid + ".img"];
            var headnode = ms.wz.wzFile_character["000120" + strid + ".img"];

            foreach (var iter in Stance.names)
            {
                Stance.Id stance = iter.Key;
                string stancename = iter.Value;

                var node_00002000img_alert = bodynode?[stancename];

                if (node_00002000img_alert == null)
                {
                    continue;
                }

                if (node_00002000img_alert is WzImageProperty property_00002000img_alert)
                {
                    foreach (var property_00002000img_alert_0 in property_00002000img_alert.WzProperties)
                    {
                        var frame = byte.Parse(property_00002000img_alert_0.Name);
                        foreach (var property_00002000img_alert_0_arm in property_00002000img_alert_0.WzProperties)
                        {
                            string part = property_00002000img_alert_0_arm.Name;

                            if (part != "delay" && part != "face")
                            {
                                string z = property_00002000img_alert_0_arm["z"]?.ToString();
                                Body.Layer layer = layer_by_name(z);

                                if (layer == Body.Layer.NONE)
                                {
                                    continue;
                                }

                                Point_short shift = new Point_short();

                                switch (layer)
                                {
                                    case Body.Layer.HAND_BELOW_WEAPON:
                                        shift = new Point_short(drawinfo.get_hand_position(stance, frame)) ?? new Point_short();
                                        var tempPoint = property_00002000img_alert_0_arm["map"]?["handMove"]?.GetPoint().ToMSPoint() ?? new Point_short();
                                        shift = shift - tempPoint;
                                        break;
                                    default:
                                        shift = new Point_short(drawinfo.get_body_position(stance, frame)) ?? new Point_short();
                                        var tempPoint1 = property_00002000img_alert_0_arm["map"]["navel"]?.GetPoint().ToMSPoint() ?? new Point_short();
                                        //AppDebug.Log ($"shift:{shift} tempPoint:{tempPoint1}");
                                        shift = shift - tempPoint1;
                                        break;
                                }

                                var texture = new Texture(property_00002000img_alert_0_arm, "Player");
                                texture.shift(shift);
                                //AppDebug.Log (texture.get_origin ());
                                stances[(int)stance, (int)layer][frame] = texture; //todo 2 might repeat add
                                /*stances[stance][layer]
								.emplace(frame, partnode)
								.first->second.shift(shift);
								*/
                            }
                        }

                        if (headnode[stancename]?[frame.ToString()]?["head"] is WzObject headsfnode)
                        {
                            Point_short shift = drawinfo.get_head_position(stance, frame) ?? new Point_short();

                            var texture = new Texture(headsfnode, "Player");
                            texture.shift(shift);
                            stances[(int)stance, (int)Body.Layer.HEAD]?.Add(frame, texture);
                        }
                    }
                }
            }

            const uint NUM_SKINTYPES = 12;

            string[] skintypes = new[] { "Light", "Tan", "Dark", "Pale", "Blue", "Green", "", "", "", "Grey", "Pink", "Red" };

            uint index = (uint)skin;
            name = (index < NUM_SKINTYPES) ? skintypes[index] : "";
        }


        public void draw(Stance.Id stance, Layer layer, byte frame, DrawArgument args, bool drawOrErase = true)
        {
            //AppDebug.Log ($"body args.FlipX: {args.FlipX}");
            Texture frameit = null;
            if (stances == null) return;
            stances[(int)stance, (int)layer]?.TryGetValue(frame, out frameit);
            if (drawOrErase)
            {
                frameit?.draw(args);
            }
            else
            {
                frameit?.erase();
            }

        }

        public string get_name()
        {
            return name;
        }

        public static Body.Layer layer_by_name(string name)
        {
            if (string.IsNullOrEmpty(name)) return Layer.NONE;
            layers_by_name.TryGetValue(name, out var layer);
            return layer;
        }

        private Dictionary<byte, Texture>[,] stances = new Dictionary<byte, Texture>[(int)Enum.GetNames(typeof(Stance.Id)).Length, (int)Layer.NUM_LAYERS];

        private void init_Dict()
        {
            for (int i = 0; i < (int)Enum.GetNames(typeof(Stance.Id)).Length; i++)
            {
                for (int j = 0; j < (int)Layer.NUM_LAYERS; j++)
                {
                    stances[i, j] = new Dictionary<byte, Texture>();
                }
            }
        }

        private string name;

        private static Dictionary<string, Layer> layers_by_name = new Dictionary<string, Layer>()
        {
            {"body", Layer.BODY},
            {"backBody", Layer.BODY},
            {"arm", Layer.ARM},
            {"armBelowHead", Layer.ARM_BELOW_HEAD},
            {"armBelowHeadOverMailChest", Layer.ARM_BELOW_HEAD_OVER_MAIL},
            {"armOverHair", Layer.ARM_OVER_HAIR},
            {"armOverHairBelowWeapon", Layer.ARM_OVER_HAIR_BELOW_WEAPON},
            {"handBelowWeapon", Layer.HAND_BELOW_WEAPON},
            {"handOverHair", Layer.HAND_OVER_HAIR},
            {"handOverWeapon", Layer.HAND_OVER_WEAPON},
            {"head", Layer.HEAD}
        };

        public void Dispose ()
        {
            if (stances == null) return;
            foreach (var pair1 in stances)
            {
                foreach (var pair2 in pair1)
                {
                    pair2.Value.Dispose ();
                }
                //pair1.Clear ();
            }

            //stances = null;
        }
    }
}