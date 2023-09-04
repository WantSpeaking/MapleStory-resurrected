




using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using SD.Tools.Algorithmia.GeneralDataStructures;
using System;

namespace ms
{
    // A tile and object layer
    public class TilesObjs:IDisposable
    {
        public TilesObjs()
        {
        }
        public TilesObjs(WzImageProperty node_100000000img_0,int layerId)//node:100000000.img/0
        {
            this.layerId = layerId;
            var tileset = node_100000000img_0["info"]["tS"] + ".img";

            foreach (var node_100000000img_0_Tile_0 in node_100000000img_0["tile"].WzProperties)
            {
                Tile tile = new Tile(node_100000000img_0_Tile_0, tileset);
                var z = tile.getz();
                tiles.Add(z, tile);
                //tiles.emplace(z, move(tile));
            }

            /*foreach (var node_100000000img_0_obj_0 in node_100000000img_0["obj"].WzProperties)
            {
                Obj obj = new Obj(node_100000000img_0_obj_0);
                var z = obj.getz();
                objs.Add(z, obj);
                //objs.emplace(z, move(obj));
            }*/
        }

        public void draw(Point_short viewpos, float alpha)
        {
            foreach (var pair in objs)
            {
                foreach (var obj in pair.Value)
                {
                    obj.draw(viewpos, alpha,layerId);
                }
            }
            foreach (var pair in tiles)
            {
                foreach (var tile in pair.Value)
                {
                    tile.draw(viewpos,layerId);
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
             foreach (var iter in objs)
             {
                 foreach (var obj in iter.Value)
                 {
                     obj.update();
                 }
             }
        }
        public void ClearDisplayObj ()
		{
            foreach (var iter in objs)
            {
                foreach (var obj in iter.Value)
                {
                    obj.update ();
                }
            }
        }

        public void Dispose()
        {
            foreach (var pair in tiles)
            {
                foreach (var t in pair.Value)
                {
                    t?.Dispose();
                }
            }

            foreach (var pair in objs)
            {
                foreach (var t in pair.Value)
                {
                    t?.Dispose();
                }
            }
        }

        private MultiValueDictionary<byte, Tile> tiles = new MultiValueDictionary<byte, Tile>();
        private MultiValueDictionary<byte, Obj> objs = new MultiValueDictionary<byte, Obj>();
        private int layerId;

        public MultiValueDictionary<byte, Tile> get_tiles()=> tiles;
        public MultiValueDictionary<byte, Obj> get_objs() => objs;
    }

    // The collection of tile and object layers on a map
    public class MapTilesObjs:IDisposable
    {
        public MapTilesObjs(WzObject node_100000000img)
        {

            if (node_100000000img != null)
            {
                for (int i = 0; i <= 7; i++)
                {
                    if (node_100000000img[i.ToString()] is WzImageProperty node)//node:100000000.img/0
                    {
                        //LoadLayer(node, i);
                        var tileObj = new TilesObjs(node,i);
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

        public void draw(Layer.Id layer, Point_short viewpos, float alpha)
        {
            layers[layer]?.draw(viewpos, alpha);
        }
        public void update()
        {
            foreach (var iter in layers)
            {
                iter.Value?.update();
            }
        }

        public void Dispose()
        {
            foreach(var iter in layers)
            {
                iter.Value.Dispose();
            }
        }

        private EnumMap<Layer.Id, TilesObjs> layers = new EnumMap<Layer.Id, TilesObjs>();

        public EnumMap<Layer.Id, TilesObjs> get_layers() => layers;
    }
}
