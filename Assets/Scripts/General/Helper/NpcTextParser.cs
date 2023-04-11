using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

//using FairyGUI;

namespace ms.Util
{
	public class NpcTextParser
	{
		public static NpcTextParser inst = new NpcTextParser ();

		string _text;
		int _readPos;

		protected Dictionary<string, TagHandler> handlers;

		public int defaultImgWidth = 0;
		public int defaultImgHeight = 0;

		protected delegate string TagHandler (string tagName, bool end, string attr);

		public NpcTextParser ()
		{
			handlers = new Dictionary<string, TagHandler> ();
			/*handlers["url"] = onTag_URL;
			handlers["img"] = onTag_IMG;
			handlers["b"] = onTag_Simple;
			handlers["i"] = onTag_Simple;
			handlers["u"] = onTag_Simple;
			handlers["sup"] = onTag_Simple;
			handlers["sub"] = onTag_Simple;
			handlers["color"] = onTag_COLOR;
			handlers["font"] = onTag_FONT;
			handlers["size"] = onTag_SIZE;
			handlers["align"] = onTag_ALIGN;*/

			handlers["c"] = onTag_ItemCount;
			handlers["h"] = onTag_PlayerName;
			handlers["m"] = onTag_MapName;
			handlers["o"] = onTag_MonsterName;
			handlers["p"] = onTag_NpcName;
			handlers["q"] = onTag_SkillName;
			handlers["s"] = onTag_SkillImage;
			handlers["t"] = onTag_ItemName;
			handlers["i"] = onTag_ItemImage;
			handlers["z"] = onTag_ItemName;
			handlers["v"] = onTag_ItemImage;

			handlers["B"] = onTag_ProgressBar;
			handlers["f"] = onTag_Image;
			handlers["F"] = onTag_Image;
			//handlers["L"] = onTag_OptionStart;
			//handlers["l"] = onTag_OptionEnd;
		}

		protected string onTag_ItemCount (string tagName, bool end, string attr)
		{
			if (!end)
			{
				var count = 0;

				return count.ToString ();
			}
			else
				return "";
		}

		protected string onTag_PlayerName (string tagName, bool end, string attr)
		{
			return Stage.get ().get_player ()?.get_name () ?? attr;
		}

		protected string onTag_MapName (string tagName, bool end, string attr)
		{
			return NxHelper.Map.get_map_info_by_id (int.Parse (attr)).name;
		}

		protected string onTag_MonsterName (string tagName, bool end, string attr)
		{
			return ms.wz.wzFile_string["Mob.img"][attr]?["name"]?.ToString () ?? attr;
		}

		protected string onTag_NpcName (string tagName, bool end, string attr)
		{
			return ms.wz.wzFile_string["Npc.img"][attr]?["name"]?.ToString () ?? attr;
		}

		protected string onTag_SkillName (string tagName, bool end, string attr)
		{
			return attr;
		}

		protected string onTag_SkillImage (string tagName, bool end, string attr)
		{
			return attr;
		}

		protected string onTag_ItemName (string tagName, bool end, string attr)
		{
			return ms.wz.wzFile_string["Consume.img"][attr]?["name"].ToString () ?? attr;
		}

		protected string onTag_ItemImage (string tagName, bool end, string attr)
		{
			return attr;
		}

		protected string onTag_ProgressBar (string tagName, bool end, string attr)
		{
			return attr;
		}

		protected string onTag_Image (string tagName, bool end, string attr)
		{
			return attr;
		}

		protected string onTag_OptionStart (string tagName, bool end, string attr)
		{
			return "<a href=\"" + attr + "\" target=\"_blank\">";
			/* if (attr != null)
					return "<a href=\"" + attr + "\" target=\"_blank\">";
				else
				{
					string href = GetTagText(false);
					return "<a href=\"" + href + "\" target=\"_blank\">";
				}
				else
				return "</a>";
		   */
		}

		protected string onTag_OptionEnd (string tagName, bool end, string attr)
		{
			return "</a>";
		}

		/*protected string onTag_URL(string tagName, bool end, string attr)
		{
			if (!end)
			{
				if (attr != null)
					return "<a href=\"" + attr + "\" target=\"_blank\">";
				else
				{
					string href = GetTagText(false);
					return "<a href=\"" + href + "\" target=\"_blank\">";
				}
			}
			else
				return "</a>";
		}

		protected string onTag_IMG(string tagName, bool end, string attr)
		{
			if (!end)
			{
				string src = GetTagText(true);
				if (src == null || src.Length == 0)
					return null;

				if (defaultImgWidth != 0)
					return "<img src=\"" + src + "\" Width=\"" + defaultImgWidth + "\" height=\"" + defaultImgHeight + "\"/>";
				else
					return "<img src=\"" + src + "\"/>";
			}
			else
				return null;
		}

		protected string onTag_Simple(string tagName, bool end, string attr)
		{
			return end ? ("</" + tagName + ">") : ("<" + tagName + ">");
		}

		protected string onTag_COLOR(string tagName, bool end, string attr)
		{
			if (!end)
				return "<font color=\"" + attr + "\">";
			else
				return "</font>";
		}

		protected string onTag_FONT(string tagName, bool end, string attr)
		{
			if (!end)
				return "<font face=\"" + attr + "\">";
			else
				return "</font>";
		}

		protected string onTag_SIZE(string tagName, bool end, string attr)
		{
			if (!end)
				return "<font size=\"" + attr + "\">";
			else
				return "</font>";
		}

		protected string onTag_ALIGN(string tagName, bool end, string attr)
		{
			if (!end)
				return "<p align=\"" + attr + "\">";
			else
				return "</p>";
		}*/

		protected string GetTagText (bool remove)
		{
			int pos1 = _readPos;
			int pos2;
			StringBuilder buffer = null;
			while ((pos2 = _text.IndexOf ('[', pos1)) != -1)
			{
				if (buffer == null)
					buffer = new StringBuilder ();

				if (_text[pos2 - 1] == '\\')
				{
					buffer.Append (_text, pos1, pos2 - pos1 - 1);
					buffer.Append ('[');
					pos1 = pos2 + 1;
				}
				else
				{
					buffer.Append (_text, pos1, pos2 - pos1);
					break;
				}
			}

			if (pos2 == -1)
				return null;

			if (remove)
				_readPos = pos2;

			return buffer.ToString ();
		}

		public string Parse (string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            StringBuilder buffer = null;
            if (buffer == null)
                buffer = new StringBuilder();

            //AppDebug.Log($"before replace:{@text}");
            //_text = text.Replace('\r', ' ' );
            //_text = text.Replace("\\r", " ").Replace("\\n", " ");
            _text = text.Replace ("\\r\\n", "\\n");
			_text = _text.Replace ("\\n", "\r\n");
			_text = _text.Replace ("#l#l", "#l");
			_text = _text.Replace ("#l", "");
            //AppDebug.Log($"after  replace:{@_text}");

            //_text = Regex.Replace(text, @"\\r"," ");

            if (_text.Contains("#L"))
			{
                var spiltArrayTuple = _text.Split(new Regex("#L[0-9]+#"), false);
                var optionTxts1 = spiltArrayTuple.Item1;
                var matchTxts = spiltArrayTuple.Item2;

                buffer.Append(optionTxts1.TryGet(0));
                for (int i = 1; i < optionTxts1.Length; i++)
                {
                    var matchValue = Regex.Match(matchTxts[i - 1], "[0-9]+")?.Value;
                    buffer.Append($"<a href=\"{matchValue}\" target=\"_blank\">{optionTxts1[i]}</a>");
                }
                _text = buffer.ToString();
				buffer.Clear();
            }
			
            int pos1 = 0, pos2, pos3;
			bool end = false;
			string tag, attr;
			string repl;
			
			TagHandler func;
			while ((pos2 = _text.IndexOf ('#', pos1)) != -1)
			{
				

				/*   if (pos2 > 0 && _text[pos2 - 1] == '\\')
				  {
				      buffer.Append(_text, pos1, pos2 - pos1 - 1);
				      buffer.Append('[');
				      pos1 = pos2 + 1;
				      continue;
				  } */

				buffer.Append (_text, pos1, pos2 - pos1);
				pos1 = pos2;
				pos2 = _text.IndexOf ('#', pos1 + 1);
				if (pos2 == -1)
					//break;
				{
					pos2 = pos1 + 2;
				}

				if (pos2 == pos1 + 1)
				{
					buffer.Append (_text, pos1, 2);
					pos1 = pos2 + 1;
					continue;
				}

				//end = _text[pos1 + 1] == '/';
				//pos3 = end ? pos1 + 2 : pos1 + 1;
				pos3 = pos1 + 1;
				tag = _text.Substring (pos3, pos2 - pos3);
				_readPos = pos2 + 1;
				attr = null;
				repl = null;
				attr = tag.Substring (1);
				tag = tag.Substring (0, 1);
				if (tag == "b" || tag == "d" || tag == "g" || tag == "k" || tag == "r" || tag == "e" || tag == "n" || tag == "l")
				{
					var tempBufferString = buffer.ToString ();

					var last_rn = tempBufferString.LastIndexOf (@"\r\n", tempBufferString.Length - 1, StringComparison.InvariantCulture);
					var insertPos = last_rn != -1 ? last_rn + 1 : 0;
					if (tag == "l")
					{
						buffer.Append ("</a>");
					}
					/*else if (tag == "b" || tag == "d" || tag == "g" || tag == "k" || tag == "r")
					{
						buffer.Append ("</font>");

						*//*if (!end)
							return "<font color=\"" + attr + "\">";
						else
							return "</font>";*//*
						var hexColor = HexColor.MSColorTagToHexColor (tag);
						buffer.Insert (insertPos, $"<font color=\"{hexColor}\">");
					}*/
					else if (tag == "e")
					{
						//return end ? ("</" + tagName + ">") : ("<" + tagName + ">");
						buffer.Append ($"</{tag}>");
						buffer.Insert (insertPos, $"<{tag}>");
					}

					pos1 = _readPos = pos1 + 2;
					continue;
				}

				/*  pos3 = tag.IndexOf('=');
				 if (pos3 != -1)
				 {
				     attr = tag.Substring(pos3 + 1);
				     tag = tag.Substring(0, pos3);
				 }
				 tag = tag.ToLower(); */
				if (handlers.TryGetValue (tag, out func))
				{
					repl = func (tag, end, attr);
					if (repl != null)
						buffer.Append (repl);
				}
				else
				{
					buffer.Append (_text, pos1, pos2 - pos1 + 1);
				}

				pos1 = _readPos;
			}

			if (buffer == null)
            {
                var tempText = _text;
				_text = null;
				return tempText;
			}
			else
			{
				if (pos1 < _text.Length)
					buffer.Append (_text, pos1, _text.Length - pos1);

				_text = null;
				return buffer.ToString ();
			}
		}
	}
}