using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using FairyGUI;
using MapleLib.WzLib;
using ms;
using UnityEngine;
using Camera = UnityEngine.Camera;

public class TextInputManager : SingletonMono<TextInputManager>
{
	public ConcurrentDictionary<ms.Textfield, GTextInput> msTextfield_GTextInput_Dict = new ConcurrentDictionary<ms.Textfield, GTextInput> ();

	public GTextInput TryGet (ms.Textfield textfield, Func<GTextInput> create = null)
	{
		if (!msTextfield_GTextInput_Dict.TryGetValue (textfield, out var result))
		{
			result = create?.Invoke ();
			Parent.AddChild (result);
			if (result != null)
			{
				msTextfield_GTextInput_Dict.TryAdd (textfield, result);
			}
		}
		else if (result == null)
		{
			msTextfield_GTextInput_Dict.TryRemove (textfield, out var _);
		}
		return result;
	}
	public void TryDraw (ms.Textfield textfield, Point_short pos, out GTextInput gTextInput)
	{
		gTextInput = TryGet (textfield, textfield.CreateGTextInput);

		if (gTextInput != null)
		{
			gTextInput.visible = true;

			var screenPos = UnityEngine.Camera.main.WorldToScreenPoint (new UnityEngine.Vector3 (pos.x (), pos.y (), 1));
			screenPos.y = screenPos.y - UnityEngine.Screen.height;
			gTextInput.position = GRoot.inst.GlobalToLocal (screenPos);
			//gRichTextField.SetPosition ((float)drawPosX * Singleton<ms.Window>.Instance.ratio, (float)(-drawPosY) * Singleton<ms.Window>.Instance.ratio, -99f);
		}
	}

	public void HideAll ()
	{
		foreach (var gobj in msTextfield_GTextInput_Dict.Values)
		{
			gobj.visible = false;
		}
	}

	public void Clear ()
	{
		msTextfield_GTextInput_Dict.Clear ();
	}

	private GComponent parent;
	private GComponent Parent
	{
		get
		{
			if (parent == null)
			{
				parent = new GComponent ();
				parent.name = "GTextInputs";
				parent.displayObject.name = "GTextInputs";
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
