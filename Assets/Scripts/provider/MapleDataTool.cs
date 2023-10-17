using System;
using System.Collections;
using System.Collections.Generic;
using MapleLib.WzLib;
using provider.wz;
using UnityEngine;
using Newtonsoft.Json;

namespace provider
{
	public class MapleDataTool
	{
		public const string JsonNodeName_CanvasFullpath = "_file_";

        public static int getIntConvert(string path, MapleData data) {
			var d = data.getChildByPath(path);
			if (d.Type ==  MapleDataType.STRING) {
				return int.Parse(getString(d));
			} else {
				return getInt(d);
			}
		}
		public static int getIntConvert(String path, MapleData data, int def) {
			var d = data.getChildByPath(path);
			if (d == null) {
				return def;
			}
            if (d.Type == MapleDataType.STRING)
            {
				try {
					return int.Parse(getString(d));
				} catch (Exception nfe) {
					//nfe.printStackTrace();
					return def;
				}
			} else {
				return getInt(d, def);
			}
		}
		public static int getIntConvert (MapleData data, int def)
		{
			if (data == null)
			{
				return def;
			}

			if (data.Type == MapleDataType.STRING)
			{
				string dd = getString (data);
				if (dd.EndsWith ("%"))
				{
					dd = dd.Substring (0, dd.Length - 1);
				}

				try
				{
					return int.Parse (dd);
				}
				catch (Exception nfe)
				{
					return def;
				}
			}
			else
			{
				return getInt (data, def);
			}
		}

		public static float getFloat(MapleData data) {
			return float.Parse((data?.Data ?? 0).ToString());
		}
		
		public static int getInt (string path, MapleData data)
		{
			return getInt(data.getChildByPath(path));
		}
		public static int getInt (MapleData data, int def)
		{
            if (data == null || data.Data == null)
            {
                return def;
            }

			int.TryParse(getString(data), out def);
            return def;
            
		}
		public static int getInt (MapleData data)
		{
			if (data == null || data.Data == null)
			{
				return 0;
			}
			return int.Parse((data?.Data ?? 0).ToString());
        }
		public static int getInt(String path, MapleData data, int def) {
			return getInt(data.getChildByPath(path), def);
		}
		public static string getString (MapleData data)
		{
			return data?.Data?.ToString();
		}
		public static string getString (MapleData data, string def)
		{
            if (data == null || data.Data == null)
            {
                return def;
            }
            return data?.Data?.ToString();
        }
		
		public static String getString(String path, MapleData data, String def) {
			if (data == null) return "";
			return getString(data.getChildByPath(path), def);
		}
		
		public static String getString(String path, MapleData data) {
			return getString(data.getChildByPath(path));
		}
		public static Point getPoint(MapleData data) {
			return (new Point(getInt(data["x"]), getInt(data["y"])));
		}
		public static Point getPoint(String path, MapleData data, Point def) {
			var pointData = data.getChildByPath(path);
			if (pointData == null) {
				return def;
			}
			return getPoint(pointData);
		}

		public static MapleDataType JsonTokenToMapleDataType (Newtonsoft.Json.Linq.JTokenType? jsonToken)
		{
            MapleDataType r = MapleDataType.NONE;

			if (jsonToken == null) { return r; }

            switch (jsonToken)
            {
                case Newtonsoft.Json.Linq.JTokenType.None:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Object:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Array:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Constructor:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Property:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Comment:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Integer:
                    r = MapleDataType.INT;
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Float:
                    r = MapleDataType.FLOAT;
                    break;
                case Newtonsoft.Json.Linq.JTokenType.String:
                    r = MapleDataType.STRING;
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Boolean:
                    r = MapleDataType.INT;
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Null:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Undefined:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Date:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Raw:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Bytes:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Guid:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.Uri:
                    break;
                case Newtonsoft.Json.Linq.JTokenType.TimeSpan:
                    break;
                case null:
                    break;
                default:
                    break;
            }
            
			return r;
        }
    }
}

