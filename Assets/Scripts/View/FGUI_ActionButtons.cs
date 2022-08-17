using System;
using FairyGUI;
using FairyGUI.Utils;
using Helper;
using Microsoft.Xna.Framework.Input;
using ms;

namespace ms_Unity
{
	public enum Enum_ActionButton
	{
		Skill1,
		Skill2,
		Skill3,
		Skill4,
		Use1,
		Use2,
		Use3,
		Use4,
	}

	public partial class FGUI_ActionButtons
	{
		public EventListener onMove { get; private set; }
		public EventListener onEnd { get; private set; }
		public EventListener onClickAction { get; private set; }

		KeyConfig.Key[] keys = new KeyConfig.Key[]
{
			KeyConfig.Key.NUM5, KeyConfig.Key.NUM6, KeyConfig.Key.NUM7, KeyConfig.Key.NUM8,
			KeyConfig.Key.NUM9, KeyConfig.Key.NUM0, KeyConfig.Key.MINUS, KeyConfig.Key.EQUAL,
			KeyConfig.Key.X, KeyConfig.Key.C, KeyConfig.Key.V, KeyConfig.Key.B
};
		public void OnCreate ()
		{
			onMove = new EventListener (this, "onMove");
			onEnd = new EventListener (this, "onEnd");
			onClickAction = new EventListener (this, "onClickAction");

			//_Btn_LightAttack.onClick.Add (OnClick_Btn_LightAttack);
			_Btn_LightAttack.onTouchBegin.Add (OnTouchBegin_Btn_LightAttack);
			_Btn_LightAttack.onTouchEnd.Add (OnTouchEnd_Btn_LightAttack);

			_Btn_Jump.onClick.Add (OnClick_Btn_Jump);
			_Btn_Dodge.onClick.Add (OnClick_Btn_Dodge);
			_Btn_PickUp.onClick.Add (OnClick_Btn_PickUp);

			_Btn_Skill1.Key = KeyConfig.Key.NUM1;
			_Btn_Skill2.Key = KeyConfig.Key.NUM2;
			_Btn_Skill3.Key = KeyConfig.Key.NUM3;
			_Btn_Skill4.Key = KeyConfig.Key.NUM4;

			_Btn_Skill1.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill2.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill3.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill4.onTouchBegin.Add (OnTouchBegin_Btn_Skill);

			_Btn_Skill1.onTouchEnd.Add (onTouchEnd_Btn_Skill);
			_Btn_Skill2.onTouchEnd.Add (onTouchEnd_Btn_Skill);
			_Btn_Skill3.onTouchEnd.Add (onTouchEnd_Btn_Skill);
			_Btn_Skill4.onTouchEnd.Add (onTouchEnd_Btn_Skill);

			for (int i = 0; i < keys.Length; i++)
			{
				var key = keys[i];
				if (i < _GList_UseBtns.numChildren)
				{
					if (_GList_UseBtns.GetChildAt (i) is FGUI_Btn_Joystick_Acton _Btn_Joystick_Acton)
					{
						_Btn_Joystick_Acton.Key = key;
					}
				}
			}
			_GList_UseBtns.onClickItem.Add (OnClick_UseBtn);
		}

		private void OnClick_UseBtn (EventContext context)
		{
			var btn_Use = context.data as FGUI_Btn_Joystick_Acton;
			ms.Stage.get ().send_keyDown ((int)btn_Use.Key);
			btn_Use.UpdateIcon ();
		}

		bool hasInit = false;
		public void Init ()
		{
			UpdateIcon ();

		}
		protected override void OnUpdate ()
		{
			base.OnUpdate ();
			if (!hasInit)
			{
				hasInit = true;
				Init ();
			}
		}
		private void OnTouchBegin_Btn_Skill (EventContext context)
		{
			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;
			UI.get ().send_key ((int)clicked_ActionButton.Key, true, true);
		}

		private void onTouchEnd_Btn_Skill (EventContext context)
		{
			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;
			UI.get ().send_key ((int)clicked_ActionButton.Key, false, true);
		}

		public void UpdateIcon ()
		{
			foreach (var child in this.GetChildren ())
			{
				if (child is FGUI_Btn_Joystick_Acton _ActonBtn)
				{
					_ActonBtn.UpdateIcon ();
				}
			}

			foreach (var child in _GList_UseBtns.GetChildren ())
			{
				if (child is FGUI_Btn_Joystick_Acton _ActonBtn)
				{
					_ActonBtn.UpdateIcon ();
				}
			}
		}

		private void OnDrop_Btn_Skill (EventContext context)
		{
			onClickAction?.Call (context);
		}

		private void OnClick_Btn_PickUp (EventContext context)
		{
			ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.PICKUP, true);
		}

		private void OnClick_Btn_Jump (EventContext context)
		{
			ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP, true);
		}
		private void OnClick_Btn_Dodge (EventContext context)
		{
			//ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP, true);
		}

		private void OnTouchBegin_Btn_LightAttack (EventContext context)
		{
			ms.UI.get ().send_key ((int)ms.KeyConfig.Key.LEFT_CONTROL, true, true);
		}

		private void OnTouchEnd_Btn_LightAttack (EventContext context)
		{
			ms.UI.get ().send_key ((int)ms.KeyConfig.Key.LEFT_CONTROL, false, true);
		}

		private void OnClick_Btn_LightAttack (EventContext context)
		{

		}
	}
}