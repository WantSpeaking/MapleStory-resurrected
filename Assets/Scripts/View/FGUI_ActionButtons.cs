using System;
using FairyGUI;
using FairyGUI.Utils;
using Helper;
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
		LightAttack,
		HeavyAttack
	}

	public partial class FGUI_ActionButtons
	{
		public EventListener onMove { get; private set; }
		public EventListener onEnd { get; private set; }
		public EventListener onClickAction { get; private set; }

		public void OnCreate ()
		{
			onMove = new EventListener (this, "onMove");
			onEnd = new EventListener (this, "onEnd");
			onClickAction = new EventListener (this, "onClickAction");

			//_Btn_LightAttack.onClick.Add (OnClick_Btn_LightAttack);
			_Btn_LightAttack.onTouchBegin.Add (OnTouchBegin_Btn_LightAttack);
			_Btn_LightAttack.onTouchMove.Add (OnTouchMove_Btn_LightAttack);
			_Btn_LightAttack.onTouchEnd.Add (OnTouchEnd_Btn_LightAttack);

			_Btn_HeavyAttack.onTouchBegin.Add (OnTouchBegin_Btn_HeavyAttack);
			_Btn_HeavyAttack.onTouchMove.Add (OnTouchMove_Btn_HeavyAttack);
			_Btn_HeavyAttack.onTouchEnd.Add (OnTouchEnd_Btn_HeavyAttack);

			_Btn_Dodge.onTouchBegin.Add (OnTouchBegin_Btn_DodgeAttack);
			_Btn_Dodge.onTouchEnd.Add (OnTouchEnd_Btn_DodgeAttack);

			_Btn_Jump.onClick.Add (OnClick_Btn_Jump);
			_Btn_Dodge.onClick.Add (OnClick_Btn_Dodge);
            _Btn_PickUp.onTouchBegin.Add(OnTouchBegin_Btn_PickUp);
            _Btn_PickUp.onTouchMove.Add(OnTouchEnd_Btn_PickUp);

            _Btn_Skill1.Key = KeyConfig.Key.NUM1;
			_Btn_Skill2.Key = KeyConfig.Key.NUM2;
			_Btn_Skill3.Key = KeyConfig.Key.NUM3;
			_Btn_Skill4.Key = KeyConfig.Key.NUM4;
			_Btn_Skill5.Key = KeyConfig.Key.NUM5;
			_Btn_Skill6.Key = KeyConfig.Key.NUM6;
			_Btn_Skill7.Key = KeyConfig.Key.NUM7;
			_Btn_Skill8.Key = KeyConfig.Key.NUM8;
			_Btn_Skill9.Key = KeyConfig.Key.NUM9;
            _Btn_Skill10.Key = KeyConfig.Key.NUM0;
            _Btn_Skill11.Key = KeyConfig.Key.MINUS;
            _Btn_Skill12.Key = KeyConfig.Key.EQUAL;

            _Btn_Skill1.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill2.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill3.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill4.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill5.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill6.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill7.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill8.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
			_Btn_Skill9.onTouchBegin.Add (OnTouchBegin_Btn_Skill);
            _Btn_Skill10.onTouchBegin.Add(OnTouchBegin_Btn_Skill);
            _Btn_Skill11.onTouchBegin.Add(OnTouchBegin_Btn_Skill);
            _Btn_Skill12.onTouchBegin.Add(OnTouchBegin_Btn_Skill);

            _Btn_Skill1.onTouchEnd.Add (onTouchEnd_Btn_Skill);
			_Btn_Skill2.onTouchEnd.Add (onTouchEnd_Btn_Skill);
			_Btn_Skill3.onTouchEnd.Add (onTouchEnd_Btn_Skill);
			_Btn_Skill4.onTouchEnd.Add (onTouchEnd_Btn_Skill);
            _Btn_Skill5.onTouchBegin.Add(onTouchEnd_Btn_Skill);
            _Btn_Skill6.onTouchBegin.Add(onTouchEnd_Btn_Skill);
            _Btn_Skill7.onTouchBegin.Add(onTouchEnd_Btn_Skill);
            _Btn_Skill8.onTouchBegin.Add(onTouchEnd_Btn_Skill);
            _Btn_Skill9.onTouchBegin.Add(onTouchEnd_Btn_Skill);
            _Btn_Skill10.onTouchBegin.Add(onTouchEnd_Btn_Skill);
            _Btn_Skill11.onTouchBegin.Add(onTouchEnd_Btn_Skill);
            _Btn_Skill12.onTouchBegin.Add(onTouchEnd_Btn_Skill);

            _Btn_Skill1.onTouchMove.Add (onTouchMove_Btn_Skill);
			_Btn_Skill2.onTouchMove.Add (onTouchMove_Btn_Skill);
			_Btn_Skill3.onTouchMove.Add (onTouchMove_Btn_Skill);
			_Btn_Skill4.onTouchMove.Add (onTouchMove_Btn_Skill);
            _Btn_Skill5.onTouchBegin.Add(onTouchMove_Btn_Skill);
            _Btn_Skill6.onTouchBegin.Add(onTouchMove_Btn_Skill);
            _Btn_Skill7.onTouchBegin.Add(onTouchMove_Btn_Skill);
            _Btn_Skill8.onTouchBegin.Add(onTouchMove_Btn_Skill);
            _Btn_Skill9.onTouchBegin.Add(onTouchMove_Btn_Skill);
            _Btn_Skill10.onTouchBegin.Add(onTouchMove_Btn_Skill);
            _Btn_Skill11.onTouchBegin.Add(onTouchMove_Btn_Skill);
            _Btn_Skill12.onTouchBegin.Add(onTouchMove_Btn_Skill);

            for (int i = 0; i < ms.Constants.get().UseBtnKeys.Length; i++)
			{
				var key = ms.Constants.get().UseBtnKeys[i];
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
			UI.get ().send_key ((int)clicked_ActionButton.Key, true, true, false);
		}

		private void onTouchEnd_Btn_Skill (EventContext context)
		{
			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;
			UI.get ().send_key ((int)clicked_ActionButton.Key, false, true, true);
		}
		private void onTouchMove_Btn_Skill (EventContext context)
		{
			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;
			UI.get ().send_key ((int)clicked_ActionButton.Key, true, true, true);
			//AppDebug.Log ($"onTouchMove_Btn_Skill: {clicked_ActionButton.Key}");
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
			
		}
        private void OnTouchBegin_Btn_PickUp(EventContext context)
		{
            ms.Stage.get().send_key(KeyType.Id.ACTION, (int)KeyAction.Id.PICKUP, true);
        }

        private void OnTouchEnd_Btn_PickUp(EventContext context)
        {
            ms.Stage.get().send_key(KeyType.Id.ACTION, (int)KeyAction.Id.PICKUP, false);
        }

        private void OnClick_Btn_Jump (EventContext context)
		{
			ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP, true);
		}
		private void OnClick_Btn_Dodge (EventContext context)
		{
			//ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.JUMP, true);
		}
		#region LightAttack
		private void OnTouchBegin_Btn_LightAttack (EventContext context)
		{
			//ms.UI.get ().send_key ((int)ms.KeyConfig.Key.LEFT_CONTROL, true, true);
			//ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.ATTACK, true, false);

			ms.Stage.get ().get_combat ().use_move (0, true, false);
			MyJoystickInput.SetButton (Enum_ActionButton.LightAttack, true);
		}
		private void OnTouchMove_Btn_LightAttack (EventContext context)
		{
			MyJoystickInput.SetButton (Enum_ActionButton.LightAttack, true);

		}
		private void OnTouchEnd_Btn_LightAttack (EventContext context)
		{
			//ms.UI.get ().send_key ((int)ms.KeyConfig.Key.LEFT_CONTROL, false, true);
			//ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.ATTACK, false, true);

			ms.Stage.get ().get_combat ().use_move (0, false, false);
			MyJoystickInput.SetButton (Enum_ActionButton.LightAttack, false);

		}
		#endregion

		#region HeavyAttack
		private void OnTouchBegin_Btn_HeavyAttack (EventContext context)
		{
			//ms.UI.get ().send_key ((int)ms.KeyConfig.Key.LEFT_CONTROL, true, true);
			//ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.MUTE, true, false);

			ms.Stage.get ().get_combat ().use_move (1, true, false);
			MyJoystickInput.SetButton (Enum_ActionButton.HeavyAttack, true);

		}
		private void OnTouchMove_Btn_HeavyAttack (EventContext context)
		{
			MyJoystickInput.SetButton (Enum_ActionButton.HeavyAttack, true);


		}
		private void OnTouchEnd_Btn_HeavyAttack (EventContext context)
		{
			//ms.UI.get ().send_key ((int)ms.KeyConfig.Key.LEFT_CONTROL, false, true);
			//ms.Stage.get ().send_key (KeyType.Id.ACTION, (int)KeyAction.Id.MUTE, false, true);

			ms.Stage.get ().get_combat ().use_move (1, false, false);
			MyJoystickInput.SetButton (Enum_ActionButton.HeavyAttack, false);


		}
		#endregion

		private void OnTouchBegin_Btn_DodgeAttack (EventContext context)
		{
			ms.Stage.get ().get_combat ().use_move (1121006, true, false);
		}

		private void OnTouchEnd_Btn_DodgeAttack (EventContext context)
		{
			ms.Stage.get ().get_combat ().use_move (1121006, false, true);
		}

		public void RefreshPos()
		{
			_Btn_HeavyAttack.SetXY(ms.Setting<PosBtnHeavyAttack>.get().load().x(), ms.Setting<PosBtnHeavyAttack>.get().load().y());
			_Btn_LightAttack.SetXY(ms.Setting<PosBtnLightAttack>.get().load().x(), ms.Setting<PosBtnLightAttack>.get().load().y());
			_Btn_Jump.SetXY(ms.Setting<PosBtnJump>.get().load().x(), ms.Setting<PosBtnJump>.get().load().y());
			_Btn_PickUp.SetXY(ms.Setting<PosBtnPickUp>.get().load().x(), ms.Setting<PosBtnPickUp>.get().load().y());
			_Btn_Skill1.SetXY(ms.Setting<PosBtnSkill1>.get().load().x(), ms.Setting<PosBtnSkill1>.get().load().y());
			_Btn_Skill2.SetXY(ms.Setting<PosBtnSkill2>.get().load().x(), ms.Setting<PosBtnSkill2>.get().load().y());
			_Btn_Skill3.SetXY(ms.Setting<PosBtnSkill3>.get().load().x(), ms.Setting<PosBtnSkill3>.get().load().y());
			_Btn_Skill4.SetXY(ms.Setting<PosBtnSkill4>.get().load().x(), ms.Setting<PosBtnSkill4>.get().load().y());
			_Btn_Skill5.SetXY(ms.Setting<PosBtnSkill5>.get().load().x(), ms.Setting<PosBtnSkill5>.get().load().y());
            _Btn_Skill6.SetXY(ms.Setting<PosBtnSkill6>.get().load().x(), ms.Setting<PosBtnSkill6>.get().load().y());
        }
	}
}