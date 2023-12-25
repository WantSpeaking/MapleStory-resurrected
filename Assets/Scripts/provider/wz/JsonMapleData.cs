using MapleLib.WzLib;
using ms;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.HableCurve;

namespace provider.wz
{
    public class JsonMapleData : MapleData
    {
        public override MapleDataType Type => MapleDataTool.JsonTokenToMapleDataType((node as JProperty)?.Value.Type);

        public List<MapleData> emptyMapleDataList = new List<MapleData>();
        public override IList<MapleData> Children
        {
            get
            {
                if (emptyMapleDataList.Count>0)
                {
					return emptyMapleDataList;
				}

                if (node is JProperty p)
                {
                    emptyMapleDataList.AddRange(p.Value.Children().Select(n => (MapleData)new JsonMapleData(n)));
                }
                else if (node is JContainer c)
                {
                    emptyMapleDataList.AddRange(c.Children().Select(n => (MapleData)new JsonMapleData(n)));
                }
                //AppDebug.Log($"JsonMapleData: node==null?:{node == null} {Name} {_path} {node?.Type} {node?.GetType()}");
                return emptyMapleDataList;
            }
        }

        //public override object Data => (node as JProperty)?.Value;
        public override object Data
        {
            get
            {
                if (node is JValue jv)
                {
                    return jv.Value;
                }
                else if (node is JProperty jp)
                {
                    //return (jp.Value as JValue)?.Value;
                    return jp.Value.ToString();
                }
                return node.ToString();
            }
        }

        public override string Name => (string)(node as JProperty)?.Name;
        //public override string Name => _name;
        public override string FullPath => node.Path;

        public override MapleDataEntity Parent
        {
            get
            {
                JObject parentNode;
                parentNode = (JObject)node.Parent;
                /*if (parentNode.Type == Node.DOCUMENT_NODE)
                {
                    return null;
                }*/
                var g = node[""]; g.ToString();
                var parentData = new JsonMapleData(parentNode);
                parentData.imageDataDir = System.IO.Directory.GetParent(imageDataDir).FullName;
                return parentData;
            }
        }

        public override MapleData this[string dataPath] => getChildByPath(dataPath);

        /*public MapleData getChildByPathOld(string path)
        {
            string[] segments = path.Split('/') ;
            if (segments[0].Equals(".."))
            {
                return ((MapleData)Parent).getChildByPath(path.Substring(path.IndexOf("/", StringComparison.Ordinal) + 1));
            }

            var myNode = node;
            foreach (string s in segments)
            {
                var childNodes = myNode.Children();
                bool foundChild = false;
                foreach (var childNode in myNode)
                {
                    if (childNode.Key.Equals(s))
                    {
                        myNode = childNode.Value;
                        foundChild = true;
                        break;
                    }
                }
                
                if (!foundChild)
                {
                    return null;
                }
            }

            var ret = new JasonMapleData(myNode);
            ret.imageDataDir = Directory.GetParent(Path.Combine(imageDataDir, Name, path)).Parent.FullName;
            return ret;
        }*/
        string _name;
        public override MapleData getChildByPath(string path)
        {
            if (path == null) return null;
            _name = path;
            string[] segments = path.Split('/');
            if (segments[0].Equals(".."))
            {
                return ((MapleData)Parent).getChildByPath(path.Substring(path.IndexOf("/", StringComparison.Ordinal) + 1));
            }
            var myNode = getRealChild(node, path);
            /*var myNode = node;

            foreach (string s in segments)
            {
                if (myNode is JProperty p)
                {
                    if (p.Value is JValue jv)
                    {
                        if (p.Name.Contains(s))
                        {
                            myNode = jv;
                        }
                        else
                        {
                            myNode = null;
                        }
                    }
                    else
                    {
                        myNode = p.Value[s];
                    }
                }
                else
                {
                    myNode = myNode[s];
                }

                _name = s;
            }*/
            if (myNode == null)
            {
                return null;
            }
            var ret = new JsonMapleData(myNode);
            //ret.imageDataDir = Directory.GetParent(Path.Combine(imageDataDir, Name, path)).Parent.FullName;
            return ret;
        }
        static JToken getRealChild(JToken node,string path)
        {
            if (path == null) return null;
            string[] segments = path.Split('/');

            var myNode = node;

            foreach (string s in segments)
            {
                if (myNode is JProperty p)
                {
                    if (p.Value is JValue jv)
                    {
                        if (p.Name.Contains(s))
                        {
                            myNode = jv;
                        }
                        else
                        {
                            myNode = null;
                        }
                    }
                    else
                    {
                        myNode = p.Value[s];
                    }
                }
                else if(myNode is JValue v)
                {
                    if (v.Type == JTokenType.String && (string)v.Value== "_null_")
                    {
                        myNode = null;
					}
                }
                else
                {
                    myNode = myNode[s];
                }
            }

            return myNode;
        }
        public override bool IsTexture()
        {
            return getRealChild(node, MapleDataTool.JsonNodeName_CanvasFullpath) != null;
            if (node is JProperty p && p.Value.Type == JTokenType.Object)
            {
                return p.Value[MapleDataTool.JsonNodeName_CanvasFullpath] != null;
            }
            return node[MapleDataTool.JsonNodeName_CanvasFullpath] != null;
        }
        public override Point GetPoint()
        {
            return new Point((int)getRealChild(node,"x"), (int)getRealChild(node, "y"));
            if (node is JProperty p && p.Value.Type == JTokenType.Object)
            {
                return new Point((int)p?.Value?["x"], (int)p?.Value?["y"]);
            }
            return new Point((int)node?["x"], (int)node?["y"]);
        }

        private JToken node;
        private string imageDataDir;

        public JsonMapleData(string jasonText, string imageDataDir)
        {
            this.node = JObject.Parse(jasonText);
            this.imageDataDir = imageDataDir;
        }

        public JsonMapleData(JToken node)
        {
            this.node = node;
        }

        public JsonMapleData(JToken node, string nodeName)
        {
            this.node = node;
            _name = nodeName;
        }
    }
}
