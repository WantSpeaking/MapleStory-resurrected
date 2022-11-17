using System;
using System.Collections;
using System.Collections.Generic;
using MapleLib.WzLib;
using UnityEngine;

namespace provider
{
	public class MapleDataTool
	{
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
		public static string getString (WzObject o)
		{
			return o?.ToString ();
		}
		public static string getString (WzObject o, string defaultResult)
		{
			return o?.ToString () ?? defaultResult;
		}
	}
}

