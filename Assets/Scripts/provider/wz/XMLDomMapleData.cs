using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

/*
	This file is part of the OdinMS Maple Story Server
    Copyright (C) 2008 Patrick Huy <patrick.huy@frz.cc>
		       Matthias Butz <matze@odinms.de>
		       Jan Christian Meyer <vimes@odinms.de>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation version 3 as published by
    the Free Software Foundation. You may not use, modify or distribute
    this program under any other version of the GNU Affero General Public
    License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
namespace provider.wz
{
	public class XMLDomMapleData : MapleData
	{
		private XmlNode XmlNode;
		private DirectoryInfo imageDataDir;

		public XMLDomMapleData (Stream fis, DirectoryInfo imageDataDir)
		{
			try
			{
				XmlDocument document = new XmlDocument ();
				document.Load (fis);
				this.XmlNode = document.ChildNodes[1];
			}
			catch (Exception e)
			{
				throw e;
			}

			this.imageDataDir = imageDataDir;
		}

		private XMLDomMapleData (XmlNode XmlNode)
		{
			this.XmlNode = XmlNode;
		}

		public override MapleData getChildByPath (string path)
		{ // the whole XML reading system seems susceptible to give nulls on strenuous read scenarios
			lock (this)
			{
				string[] segments = path.Split ('/');
				if (segments[0].Equals (".."))
				{
					return ((MapleData)Parent).getChildByPath (path.Substring (path.IndexOf ("/", StringComparison.Ordinal) + 1));
				}

				XmlNode myNode;
				myNode = XmlNode;
				foreach (string s in segments)
				{
					var childNodes = myNode.ChildNodes;
					bool foundChild = false;
					for (int i = 0; i < childNodes.Count; i++)
					{
						XmlNode childNode = childNodes[i];
						if (childNode.NodeType == XmlNodeType.Element && childNode.Attributes.GetNamedItem ("name").Value.Equals (s))
						{
							myNode = childNode;
							foundChild = true;
							break;
						}
					}
					if (!foundChild)
					{
						return null;
					}
				}

				XMLDomMapleData ret = new XMLDomMapleData (myNode);
				ret.imageDataDir = new DirectoryInfo (imageDataDir.FullName + "/" + Name + "/" + path).Parent;
				return ret;
			}
		}

		public override IList<MapleData> Children
		{
			get
			{
				lock (this)
				{
					IList<MapleData> ret = new List<MapleData> ();

					var childNodes = XmlNode.ChildNodes;
					for (int i = 0; i < childNodes.Count; i++)
					{
						XmlNode childNode = childNodes[i];
						if (childNode.NodeType == XmlNodeType.Element)
						{
							XMLDomMapleData child = new XMLDomMapleData (childNode);
							child.imageDataDir = new DirectoryInfo (imageDataDir.FullName + "/" + Name);
							ret.Add (child);
						}
					}

					return ret;
				}
			}
		}

		public override object Data
		{
			get
			{
				lock (this)
				{
					var attributes = XmlNode.Attributes;
					MapleDataType type = Type;
					switch (type)
					{
						case provider.wz.MapleDataType.DOUBLE:
						case provider.wz.MapleDataType.FLOAT:
						case provider.wz.MapleDataType.INT:
						case provider.wz.MapleDataType.SHORT:
							{
								string value = attributes.GetNamedItem ("value").Value;

								switch (type)
								{
									case provider.wz.MapleDataType.DOUBLE:
										return double.Parse (value);
									case provider.wz.MapleDataType.FLOAT:
										return float.Parse (value);
									case provider.wz.MapleDataType.INT:
										return int.Parse (value);
									case provider.wz.MapleDataType.SHORT:
										return short.Parse (value);
									default:
										return null;
								}
							}
						case provider.wz.MapleDataType.STRING:
						case provider.wz.MapleDataType.UOL:
							{
								string value = attributes.GetNamedItem ("value").Value;
								return value;
							}
						case provider.wz.MapleDataType.VECTOR:
							{
								string x = attributes.GetNamedItem ("x").Value;
								string y = attributes.GetNamedItem ("y").Value;
								return new ms.Point_short (short.Parse (x), short.Parse (y));
							}
						case provider.wz.MapleDataType.CANVAS:
							{
								string width = attributes.GetNamedItem ("width").Value;
								string height = attributes.GetNamedItem ("height").Value;
								return new FileStoredPngMapleCanvas (int.Parse (width), int.Parse (height), new DirectoryInfo (imageDataDir.FullName + "/" + Name + ".png"));
							}
						default:
							return null;
					}
				}
			}
		}

		public override MapleDataType Type
		{
			get
			{
				lock (this)
				{
					string nodeName = XmlNode.Name;

					switch (nodeName)
					{
						case "imgdir":
							return MapleDataType.PROPERTY;
						case "canvas":
							return MapleDataType.CANVAS;
						case "convex":
							return MapleDataType.CONVEX;
						case "sound":
							return MapleDataType.SOUND;
						case "uol":
							return MapleDataType.UOL;
						case "double":
							return MapleDataType.DOUBLE;
						case "float":
							return MapleDataType.FLOAT;
						case "int":
							return MapleDataType.INT;
						case "short":
							return MapleDataType.SHORT;
						case "string":
							return MapleDataType.STRING;
						case "vector":
							return MapleDataType.VECTOR;
						case "null":
							return MapleDataType.IMG_0x00;
					}
					return MapleDataType.NONE;
				}
			}
		}

		public override MapleDataEntity Parent
		{
			get
			{
				lock (this)
				{
					XmlNode parentNode;
					parentNode = XmlNode.ParentNode;
					if (parentNode.NodeType == XmlNodeType.Document)
					{
						return null;
					}
					XMLDomMapleData parentData = new XMLDomMapleData (parentNode);
					parentData.imageDataDir = imageDataDir.Parent;
					return parentData;
				}
			}
		}

		public override string Name
		{
			get
			{
				lock (this)
				{
					return XmlNode.Attributes.GetNamedItem ("name").Value;
				}
			}
		}

		public override MapleData this[string name]
		{
			get
			{
				return getChildByPath (name);
			}
		}

		public override IEnumerator<MapleData> GetEnumerator ()
		{
			lock (this)
			{
				return Children.GetEnumerator ();
			}
		}

		public override string FullPath => imageDataDir.FullName + "\\" + Name;
	}

}