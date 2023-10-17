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

namespace provider.wz
{
    public class JsonMapleData : MapleData
    {
        public override MapleDataType Type => MapleDataTool.JsonTokenToMapleDataType( (node as JProperty)?.Value.Type);

        public static List<MapleData> emptyMapleDataList = new List<MapleData>();
        public override IList<MapleData> Children
        {
            get
            {
                if (node is JContainer c)
                {
                    return c.Select(n => (MapleData)new JsonMapleData(n)).ToList();
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
                if(node is JValue jv)
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
                var g = node[""];g.ToString();
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
            _name = path;
            string[] segments = path.Split('/');
            if (segments[0].Equals(".."))
            {
                return ((MapleData)Parent).getChildByPath(path.Substring(path.IndexOf("/", StringComparison.Ordinal) + 1));
            }

            var myNode = node;
            foreach (string s in segments)
            {
                myNode = myNode[s];
                _name = s;
            }
            if (myNode == null)
            {
                return null;
            }
            var ret = new JsonMapleData(myNode);
            //ret.imageDataDir = Directory.GetParent(Path.Combine(imageDataDir, Name, path)).Parent.FullName;
            return ret;
        }

        public override bool IsTexture()
        {
            return node[MapleDataTool.JsonNodeName_CanvasFullpath]!=null;
        }
        public override Point GetPoint()
        {
            return new Point((int)node?["x"], (int)node?["y"]);
        }

        private JToken node;
        private string imageDataDir;

        public JsonMapleData(string jasonText,string imageDataDir)
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
