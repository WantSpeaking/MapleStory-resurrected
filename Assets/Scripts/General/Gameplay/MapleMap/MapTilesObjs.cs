




using GameFramework.Resource;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
using ms_Unity;
using provider;
using SD.Tools.Algorithmia.GeneralDataStructures;
using System;
using UnityEngine;

namespace ms
{
    // A tile and object layer
    public class TilesObjs : IDisposable
    {
        public TilesObjs()
        {
        }
        public TilesObjs(WzImageProperty node_100000000img_0, int layerId)//node:100000000.img/0
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

            foreach (var node_100000000img_0_obj_0 in node_100000000img_0["obj"].WzProperties)
            {
                Obj obj = new Obj(node_100000000img_0_obj_0);
                var z = obj.getz();
                objs.Add(z, obj);
                //objs.emplace(z, move(obj));
            }
        }
        public TilesObjs(MapleData node_100000000img_0, int layerId)//node:100000000.img/0
        {
            this.layerId = layerId;
            var tileset = node_100000000img_0["info"]["tS"] + ".img";

            foreach (var node_100000000img_0_Tile_0 in node_100000000img_0["tile"])
            {
                Tile tile = new Tile(node_100000000img_0_Tile_0, tileset);
                var z = tile.getz();
                tiles.Add(z, tile);
                //tiles.emplace(z, move(tile));
            }

            foreach (var node_100000000img_0_obj_0 in node_100000000img_0["obj"])
            {
                Obj obj = new Obj(node_100000000img_0_obj_0);
                var z = obj.getz();
                objs.Add(z, obj);
                //objs.emplace(z, move(obj));
            }
        }
        public void draw(Point_short viewpos, float alpha)
        {
            foreach (var pair in objs)
            {
                foreach (var obj in pair.Value)
                {
                    obj.draw(viewpos, alpha, layerId);
                }
            }
            foreach (var pair in tiles)
            {
                foreach (var tile in pair.Value)
                {
                    tile.draw(viewpos, layerId);
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
        public void ClearDisplayObj()
        {
            foreach (var iter in objs)
            {
                foreach (var obj in iter.Value)
                {
                    obj.update();
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

        private MultiValueDictionary<int, Tile> tiles = new MultiValueDictionary<int, Tile>();
        private MultiValueDictionary<int, Obj> objs = new MultiValueDictionary<int, Obj>();
        private int layerId;

        public MultiValueDictionary<int, Tile> get_tiles() => tiles;
        public MultiValueDictionary<int, Obj> get_objs() => objs;
    }

    // The collection of tile and object layers on a map
    public class MapTilesObjs : IDisposable
    {
        GameObject gobj_Tile;
        GameObject gobj_Obj;
        string o_assetPath = "";
        string t_assetPath = "";
        public MapTilesObjs(WzObject node_100000000img,int mapid)
        {
            if(GameUtil.Instance.Use_wz_or_ab_draw_ObjTile)
            {
                if (node_100000000img != null)
                {
                    for (int i = 0; i <= 7; i++)
                    {
                        if (node_100000000img[i.ToString()] is WzImageProperty node)//node:100000000.img/0
                        {
                            //LoadLayer(node, i);
                            var tileObj = new TilesObjs(node, i);
                            layers[(Layer.Id)i] = tileObj;
                        }
                    }
                }
            }
            else
            {
                /*gobj_Obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>($"Prefabs/Obj/Map_{mapId}_Obj"));
           gobj_Tile = UnityEngine.Object.Instantiate<GameObject>( Resources.Load<GameObject>($"Prefabs/Tile/Map_{mapId}_Tile"));*/

                /*GameEntry.Resource.LoadAsset($"Assets/GameMain/Prefabs/Obj/Map_{mapId}_Obj.prefab", typeof(UnityEngine.GameObject), new LoadAssetCallbacks((assetName, asset, duration, userData) => { gobj_Obj = UnityEngine.Object.Instantiate<GameObject>((GameObject)asset); }));

                GameEntry.Resource.LoadAsset($"Assets/GameMain/Prefabs/Tile/Map_{mapId}_Tile.prefab", typeof(UnityEngine.GameObject), new LoadAssetCallbacks((assetName, asset, duration, userData) => { gobj_Tile = UnityEngine.Object.Instantiate<GameObject>((GameObject)asset); }));*/
                string strid = string_format.extend_id(mapid, 9);
                string prefix = Convert.ToString(mapid / 100000000);
                o_assetPath = $"Prefabs/Obj/Map{prefix}_Obj";
                t_assetPath = $"Prefabs/Tile/Map{prefix}_Tile";
                var o_asset = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(o_assetPath, $"Map{prefix}_Obj.{strid}");
                var t_asset = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(t_assetPath, $"Map{prefix}_Tile.{strid}");
                if (o_asset != null)
                    gobj_Obj = UnityEngine.Object.Instantiate<GameObject>(o_asset);
                if (t_asset != null)
                    gobj_Tile = UnityEngine.Object.Instantiate<GameObject>(t_asset);
            }
        }

        public MapTilesObjs(MapleData node_100000000img, int mapid)
        {
            if (GameUtil.Instance.Use_wz_or_ab_draw_ObjTile)
            {
                if (node_100000000img != null)
                {
                    for (int i = 0; i <= 7; i++)
                    {
                        if (node_100000000img[i.ToString()] is MapleData node)//node:100000000.img/0
                        {
                            //LoadLayer(node, i);
                            var tileObj = new TilesObjs(node, i);
                            layers[(Layer.Id)i] = tileObj;
                        }
                    }
                }
            }
            else
            {
                /*gobj_Obj = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>($"Prefabs/Obj/Map_{mapId}_Obj"));
           gobj_Tile = UnityEngine.Object.Instantiate<GameObject>( Resources.Load<GameObject>($"Prefabs/Tile/Map_{mapId}_Tile"));*/

                /*GameEntry.Resource.LoadAsset($"Assets/GameMain/Prefabs/Obj/Map_{mapId}_Obj.prefab", typeof(UnityEngine.GameObject), new LoadAssetCallbacks((assetName, asset, duration, userData) => { gobj_Obj = UnityEngine.Object.Instantiate<GameObject>((GameObject)asset); }));

                GameEntry.Resource.LoadAsset($"Assets/GameMain/Prefabs/Tile/Map_{mapId}_Tile.prefab", typeof(UnityEngine.GameObject), new LoadAssetCallbacks((assetName, asset, duration, userData) => { gobj_Tile = UnityEngine.Object.Instantiate<GameObject>((GameObject)asset); }));*/
                string strid = string_format.extend_id(mapid, 9);
                string prefix = Convert.ToString(mapid / 100000000);
                o_assetPath = $"Prefabs/Obj/Map{prefix}_Obj";
                t_assetPath = $"Prefabs/Tile/Map{prefix}_Tile";
                var o_asset = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(o_assetPath, $"Map{prefix}_Obj.{strid}");
                var t_asset = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(t_assetPath, $"Map{prefix}_Tile.{strid}");
                if (o_asset != null)
                    gobj_Obj = UnityEngine.Object.Instantiate<GameObject>(o_asset);
                if (t_asset != null)
                    gobj_Tile = UnityEngine.Object.Instantiate<GameObject>(t_asset);
            }
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
                        var tileObj = new TilesObjs(node, i);
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
            if (GameUtil.Instance.Use_wz_or_ab_draw_ObjTile)
            {
                layers[layer]?.draw(viewpos, alpha);
            }
            else
            {
                if (gobj_Obj == null || gobj_Tile == null) return;
                gobj_Obj.transform.position = new Vector3(viewpos.x(), -viewpos.y(), 0);
                gobj_Tile.transform.position = new Vector3(viewpos.x(), -viewpos.y(), -1);
            }
        }
        public void update()
        {
            if (GameUtil.Instance.Use_wz_or_ab_draw_ObjTile)
            {
                foreach (var iter in layers)
                {
                    iter.Value?.update();
                }
            }
        }

        public void Dispose()
        {
            if (GameUtil.Instance.Use_wz_or_ab_draw_ObjTile)
            {
                foreach (var iter in layers)
                {
                    iter.Value?.Dispose();
                }
            }
            else
            {
                UnityEngine.Object.Destroy(gobj_Tile);
                UnityEngine.Object.Destroy(gobj_Obj);

                AppDebug.Log("Dispose MapTilesObjs");
                /*AssetBundleLoaderMgr.Instance.UnloadAssetBundle(t_assetPath);
                AssetBundleLoaderMgr.Instance.UnloadAssetBundle(o_assetPath);*/
                //GameEntry.Resource.UnloadUnusedAssets(true);
            }
        }

        private EnumMap<Layer.Id, TilesObjs> layers = new EnumMap<Layer.Id, TilesObjs>();

        public EnumMap<Layer.Id, TilesObjs> get_layers() => layers;
    }
}
