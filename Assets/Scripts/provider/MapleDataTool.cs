using System;
using System.Collections;
using System.Collections.Generic;
using MapleLib.WzLib;
using UnityEngine;

namespace provider
{
	public class MapleDataTool
	{
		public static int getIntConvert(string path, WzObject data) {
			var d = data.getChildByPath(path);
			if (d is  WzImageProperty p && p.PropertyType == WzPropertyType.String) {
				return int.Parse(getString(d));
			} else {
				return getInt(d);
			}
		}
		public static int getIntConvert(String path, WzObject data, int def) {
			var d = data.getChildByPath(path);
			if (d == null) {
				return def;
			}
			if (d is  WzImageProperty p && p.PropertyType == WzPropertyType.String) {
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
		public static int getIntConvert (WzImageProperty data, int def)
		{
			if (data == null)
			{
				return def;
			}

			if (data.PropertyType == WzPropertyType.String)
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

		public static float getFloat(WzObject o) {
			return o?.GetFloat () ?? 0;
		}
		
		public static int getInt (string path, WzObject o)
		{
			return o[path]?.GetInt () ?? 0;
		}
		public static int getInt (WzObject data, int def)
		{
			return data?.GetInt () ?? def;
		}
		public static int getInt (WzObject o)
		{
			return o?.GetInt () ?? 0;
		}
		public static int getInt(String path, WzObject data, int def) {
			return getInt(data.getChildByPath(path), def);
		}
		public static string getString (WzObject o)
		{
			return o?.ToString ();
		}
		public static string getString (WzObject o, string defaultResult)
		{
			return o?.ToString () ?? defaultResult;
		}
		
		public static String getString(String path, WzObject data, String def) {
			if (data == null) return "";
			return getString(data.getChildByPath(path), def);
		}
		
		public static String getString(String path, WzObject data) {
			return getString(data.getChildByPath(path));
		}
		public static Point getPoint(WzObject data) {
			return ((Point) data.GetPoint ());
		}
		public static Point getPoint(String path, WzObject data, Point def) {
			var pointData = data.getChildByPath(path);
			if (pointData == null) {
				return def;
			}
			return getPoint(pointData);
		}
	}
}

