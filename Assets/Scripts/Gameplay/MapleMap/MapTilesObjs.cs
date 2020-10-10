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




using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using SD.Tools.Algorithmia.GeneralDataStructures;

namespace ms
{
    // A tile and object layer
    public class TilesObjs
    {
        public TilesObjs()
        {
        }
        public TilesObjs(WzImageProperty node_100000000img_0)//node:100000000.img/0
        {
            var tileset = node_100000000img_0["info"]["tS"] + ".img";

            foreach (var node_100000000img_0_Tile_0 in node_100000000img_0["tile"].WzProperties)
            {
                Tile tile = new Tile(node_100000000img_0_Tile_0, tileset);
                var z = tile.getz();
                tiles.Add(z, tile);
                //tiles.emplace(z, std::move(tile));
            }

            foreach (var node_100000000img_0_obj_0 in node_100000000img_0["obj"].WzProperties)
            {
                Obj obj = new Obj(node_100000000img_0_obj_0);
                var z = obj.getz();
                objs.Add(z, obj);
                //objs.emplace(z, std::move(obj));
            }
        }

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(Point<short> viewpos, float alpha) const
        public void draw(Point<short> viewpos, float alpha)
        {
            foreach (var pair in objs)
            {
                foreach (var obj in pair.Value)
                {
                    obj.draw(viewpos, alpha);
                }
            }
            foreach (var pair in tiles)
            {
                foreach (var tile in pair.Value)
                {
                    tile.draw(viewpos);
                }
            }
            /* foreach (var iter in objs)
             {
                 iter.second.draw(viewpos, alpha);
             }

             foreach (var iter in tiles)
             {
                 iter.second.draw(viewpos);
             }*/
        }
        public void update()
        {
            /* foreach (var iter in objs)
             {
                 iter.second.update();
             }*/
        }

        private MultiValueDictionary<byte, Tile> tiles = new MultiValueDictionary<byte, Tile>();
        private MultiValueDictionary<byte, Obj> objs = new MultiValueDictionary<byte, Obj>();
    }

    // The collection of tile and object layers on a map
    public class MapTilesObjs
    {
        public MapTilesObjs()
        {
        }
        public MapTilesObjs(WzObject node_100000000img)
        {

            if (node_100000000img != null)
            {
                for (int i = 0; i <= 7; i++)
                {
                    if (node_100000000img[i.ToString()] is WzImageProperty node)//node:100000000.img/0
                    {
                        //LoadLayer(node, i);
                        var tileObj = new TilesObjs(node);
                        layers[(Layer.Id)i] = tileObj;
                    }
                }
            }
        }

        /*private void LoadLayer(WzImageProperty layerNode, int level)//layerNode:0
        {
            //var layerSceneNode = (LayerNode)this.Scene.Layers.Nodes[level];

            //读取obj
            if (layerNode["obj"] is WzSubProperty objNode)//objNode:0/obj
            {
                foreach (var node in objNode.WzProperties)//node:0/obj/0
                {
                    var tileObj = new TilesObjs(node);
                    *//*var item = ObjItem.LoadFromNode(node);
					item.Name = $"obj_{level}_{node.Text}";
					item.Index = int.Parse(node.Text);*//*
                    layers[(Layer.Id)level] = tileObj;
                    //layerSceneNode.Obj.Slots.Add(item);
                }
            }


            //读取tile
            string tS = layerNode.Nodes["info"]?.Nodes["tS"].GetValueEx<string>(null);
            var tileNode = layerNode.Nodes["tile"];
            if (tS != null && tileNode != null)
            {
                foreach (var node in tileNode.Nodes)
                {
                    var item = TileItem.LoadFromNode(node);
                    item.TS = tS;
                    item.Name = $"tile_{level}_{node.Text}";
                    item.Index = int.Parse(node.Text);

                    layerSceneNode.Tile.Slots.Add(item);
                }
            }
        }*/

        //C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void draw(Layer::Id layer, Point<short> viewpos, float alpha) const
        public void draw(Layer.Id layer, Point<short> viewpos, float alpha)
        {
            layers[layer].draw(viewpos, alpha);
        }
        public void update()
        {
            /*foreach (var iter in layers)
            {
                iter.second.update();
            }*/
        }

        private EnumMap<Layer.Id, TilesObjs> layers = new EnumMap<Layer.Id, TilesObjs>();
    }
}
