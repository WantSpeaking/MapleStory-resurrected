using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ms;

namespace Assets.ms
{
    class Stage : Singleton<Stage>
    {
        int mapid;
        MapBackgrounds backgrounds;
        MapTilesObjs tilesobjs;
        public void load_map(int mapid)//mapid:100000000
        {
            this.mapid = mapid;

            //string strid = string_format.extend_id(mapid, 9);
            string strid = mapid.ToString();//strid:100000000
            string prefix = Convert.ToString(mapid / 100000000);//prefix:1

            //nl.node src = mapid == -1 ? nl.nx.ui["CashShopPreview.img"] : nl.nx.map["Map"]["Map" + prefix][strid + ".img"];
            //var src = nl.nx.mapWz.ResolvePath("Map").ResolvePath($"Map{prefix}").ResolvePath($"{strid}.img"); //srcNodePath:Map/Map1/100000000.img
            var node_100000000img = nl.nx.map["Map"]["Map" + prefix][strid + ".img"]; ; //srcNodePath:Map/Map1/100000000.img


            tilesobjs = new MapTilesObjs(node_100000000img);
            //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
            //ORIGINAL LINE: backgrounds = MapBackgrounds(src["back"]);
            backgrounds = new MapBackgrounds(node_100000000img["back"]);//back2NodePath:Map/Map1/100000000.img/back
                                                                        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                                                                        //ORIGINAL LINE: physics = Physics(src["foothold"]);

        }

        public void draw(float alpha)
        {
            double viewx = 0;
            double viewy = 0;
            Point<short> viewpos = new Point<short>();

            backgrounds.drawbackgrounds(1, 1, 1);

            foreach (var id in Enum.GetValues(typeof(global::ms.Layer.Id)))
            {
                tilesobjs.draw((global::ms.Layer.Id)id, viewpos, alpha);
                /* reactors.draw(id, viewx, viewy, alpha);
                 npcs.draw(id, viewx, viewy, alpha);
                 mobs.draw(id, viewx, viewy, alpha);
                 chars.draw(id, viewx, viewy, alpha);
                 player.draw(id, viewx, viewy, alpha);
                 drops.draw(id, viewx, viewy, alpha);*/
            }

            backgrounds.drawforegrounds(1, 1, 1);

        }
    }
}
