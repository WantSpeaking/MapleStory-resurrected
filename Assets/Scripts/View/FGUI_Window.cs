using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace ms_Unity
{
	public class FGUI_Window : Window
	{
		public EventListener onInit;
		public EventListener onShown;
		public EventListener onHide;

		public FGUI_Window ()
		{
			onInit = new EventListener (this, "onInit");
			onShown = new EventListener (this, "onShown");
			onHide = new EventListener (this, "onHide");
		}

		protected override void OnInit ()
		{
			/*this.contentPane = UIPackage.CreateObject ("Basics", "WindowB").asCom;
			this.Center ();*/
			onInit.Call ();
		}

		override protected void DoShowAnimation ()
		{
			this.SetScale (0.1f, 0.1f);
			this.SetPivot (0.5f, 0.5f);
			this.TweenScale (new Vector2 (1, 1), 0.3f).OnComplete (this.OnShown);
		}

		override protected void DoHideAnimation ()
		{
			this.TweenScale (new Vector2 (0.1f, 0.1f), 0.3f).OnComplete (this.HideImmediately);
		}

		override protected void OnShown ()
		{
			//contentPane.GetTransition ("t1").Play ();
			onShown.Call ();

		}

		override protected void OnHide ()
		{
			//contentPane.GetTransition ("t1").Stop ();
			onHide.Call ();
		}
	}
}
