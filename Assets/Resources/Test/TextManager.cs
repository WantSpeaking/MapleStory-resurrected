using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FairyGUI;
using MapleLib.WzLib;
using ms;
using UnityEngine;
using Camera = UnityEngine.Camera;

public class TextManager : SingletonMono<TextManager>
{
	public ConcurrentDictionary<ms.Text, GRichTextField> texture_GObj_Dict = new ConcurrentDictionary<ms.Text, GRichTextField> ();

	public GRichTextField TryGetGText (ms.Text hashCode, Func<GRichTextField> create = null)
	{
		if (!texture_GObj_Dict.TryGetValue (hashCode, out var result))
		{
			result = create?.Invoke ();
			Parent.AddChild (result);
			if (result != null)
			{
				texture_GObj_Dict.TryAdd (hashCode, result);
			}
		}
		else if (result == null)
		{
			texture_GObj_Dict.TryRemove (hashCode, out var _);
		}
		return result;
	}

	public void TryDraw (ms.Text texture, string text, DrawArgument args, Range_short vertical)
	{
		if (string.IsNullOrEmpty (text))
		{
			return;
		}

		GRichTextField gRichTextField = TryGetGText (texture, texture.CreateGRichText);

		if (gRichTextField != null)
		{
			gRichTextField.visible = true;


			short drawPosX = args.getpos ().x ();
			int drawPosY = -args.getpos ().y ();

			var screenPos = UnityEngine.Camera.main.WorldToScreenPoint (new UnityEngine.Vector3 (args.getpos ().x (), args.getpos ().y (), 1));
			screenPos.y = screenPos.y - UnityEngine.Screen.height;
			gRichTextField.position = GRoot.inst.GlobalToLocal (screenPos);

			//gRichTextField.SetPosition ((float)drawPosX * Singleton<ms.Window>.Instance.ratio, (float)(-drawPosY) * Singleton<ms.Window>.Instance.ratio, -99f);
			gRichTextField.text = text;
		}
	}

	public void HideAll ()
	{
		foreach (GRichTextField gobj in texture_GObj_Dict.Values)
		{
			gobj.visible = false;
		}
	}

	private GComponent parent;
	private GComponent Parent
	{
		get
		{
			if (parent == null)
			{
				parent = new GComponent ();
				parent.name = "TextParent";
				GRoot.inst.AddChild (parent);
			}
			return parent;

		}
	}

	private void Awake ()
	{
		//GRoot.inst.container.renderMode = RenderMode.WorldSpace;
	}

	private void Start ()
	{
		/*gTextInput = new GTextInput ();
		gTextInput.text = "11111111111111111111111111111111";
		gTextInput.border = 5;
		GRoot.inst.AddChild (gTextInput);*/
	}

	private new void Update ()
	{
		/*Vector3 screenPos = StageCamera.main.WorldToScreenPoint (test.transform.position);
		screenPos.y = (float)Screen.height - screenPos.y;
		Vector2 localPos = GRoot.inst.GlobalToLocal (screenPos);
		gTextInput.SetPosition (localPos.x, localPos.y, -99f);
		gTextInput.SetSize (200f, 100f);
		Vector3 position = GRoot.inst.touchTarget?.position ?? Vector3.zero;*/
	}
}
