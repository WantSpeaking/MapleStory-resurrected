using System;
using System.Collections.Generic;
using FairyGUI;
using Helper;
using ms;
using NodeCanvas.Framework;
using UnityEngine;

namespace ms_Unity
{
	public partial class FGUI_Joystick
	{
		float _InitX;
		float _InitY;
		float _startStageX;
		float _startStageY;
		float _lastStageX;
		float _lastStageY;
		GButton _button;
		GObject _touchArea;
		GObject _thumb;
		GObject _center;
		int touchId;
		GTweener _tweener;

		public EventListener onMove { get; private set; }
		public EventListener onEnd { get; private set; }

		public int radius { get; set; }

		List<FGUI_Btn_Joystick_Acton> _skillBtns;
		Player _player => ms.Stage.get ().get_player ();
		void OnCreate ()
		{
			onMove = new EventListener (this, "onMove");
			onEnd = new EventListener (this, "onEnd");

			_button = _joystick.asButton;
			_button.changeStateOnClick = false;
			_thumb = _button.GetChild ("thumb");
			_touchArea = _joystick_touch;
			_center = _joystick_center;

			_InitX = _center.x + _center.width / 2;
			_InitY = _center.y + _center.height / 2;
			touchId = -1;
			radius = 150;

			_touchArea.onTouchBegin.Add (this.OnTouchBegin);
			_touchArea.onTouchMove.Add (this.OnTouchMove);
			_touchArea.onTouchEnd.Add (this.OnTouchEnd);

			_skillBtns = new List<FGUI_Btn_Joystick_Acton> ();
			_skillBtns.Add (_Btn_Skill_Up);
			_skillBtns.Add (_Btn_Skill_Down);
			_skillBtns.Add (_Btn_Skill_Left);
			_skillBtns.Add (_Btn_Skill_Right);

			foreach (var btn in _skillBtns)
			{
				btn.onTouchBegin.Add (OnTouchBegin_SkillBtn);
				btn.onTouchMove.Add (OnTouchMove_SkillBtn);
				btn.onTouchEnd.Add (OnTouchEnd_SkillBtn);
			}
		}

		public void Trigger (EventContext context)
		{
			OnTouchBegin (context);
		}

		private void OnTouchBegin (EventContext context)
		{
			if (touchId == -1)//First touch
			{
				InputEvent evt = (InputEvent)context.data;
				touchId = evt.touchId;

				if (_tweener != null)
				{
					_tweener.Kill ();
					_tweener = null;
				}

				Vector2 pt = GRoot.inst.GlobalToLocal (new Vector2 (evt.x, evt.y));
				float bx = pt.x;
				float by = pt.y;
				_button.selected = true;

				if (bx < 0)
					bx = 0;
				else if (bx > _touchArea.width)
					bx = _touchArea.width;

				if (by > GRoot.inst.height)
					by = GRoot.inst.height;
				else if (by < _touchArea.y)
					by = _touchArea.y;

				_lastStageX = bx;
				_lastStageY = by;
				_startStageX = bx;
				_startStageY = by;

				_center.visible = true;
				_center.SetXY (bx - _center.width / 2, by - _center.height / 2);
				_button.SetXY (bx - _button.width / 2, by - _button.height / 2);

				float deltaX = bx - _InitX;
				float deltaY = by - _InitY;
				float degrees = Mathf.Atan2 (deltaY, deltaX) * 180 / Mathf.PI;
				_thumb.rotation = degrees + 90;

				context.CaptureTouch ();
			}
		}

		private void OnTouchEnd (EventContext context)
		{
			InputEvent inputEvt = (InputEvent)context.data;
			if (touchId != -1 && inputEvt.touchId == touchId)
			{
				touchId = -1;
				_thumb.rotation = _thumb.rotation + 180;
				_center.visible = false;
				_tweener = _button.TweenMove (new Vector2 (_InitX - _button.width / 2, _InitY - _button.height / 2), 0.3f).OnComplete (() =>
				{
					_tweener = null;
					_button.selected = false;
					_thumb.rotation = 0;
					_center.visible = true;
					_center.SetXY (_InitX - _center.width / 2, _InitY - _center.height / 2);
				}
				);

				this.onEnd.Call ();
				__joystickEnd (context);
			}
		}

		private void OnTouchMove (EventContext context)
		{
			InputEvent evt = (InputEvent)context.data;
			if (touchId != -1 && evt.touchId == touchId)
			{
				Vector2 pt = GRoot.inst.GlobalToLocal (new Vector2 (evt.x, evt.y));
				float bx = pt.x;
				float by = pt.y;
				float moveX = bx - _lastStageX;
				float moveY = by - _lastStageY;
				_lastStageX = bx;
				_lastStageY = by;
				float buttonX = _button.x + moveX;
				float buttonY = _button.y + moveY;

				float offsetX = buttonX + _button.width / 2 - _startStageX;
				float offsetY = buttonY + _button.height / 2 - _startStageY;

				float rad = Mathf.Atan2 (offsetY, offsetX);
				float degree = rad * 180 / Mathf.PI;
				_thumb.rotation = degree + 90;

				float maxX = radius * Mathf.Cos (rad);
				float maxY = radius * Mathf.Sin (rad);
				if (Mathf.Abs (offsetX) > Mathf.Abs (maxX))
					offsetX = maxX;
				if (Mathf.Abs (offsetY) > Mathf.Abs (maxY))
					offsetY = maxY;

				buttonX = _startStageX + offsetX;
				buttonY = _startStageY + offsetY;
				if (buttonX < 0)
					buttonX = 0;
				if (buttonY > GRoot.inst.height)
					buttonY = GRoot.inst.height;

				_button.SetXY (buttonX - _button.width / 2, buttonY - _button.height / 2);

				this.onMove.Call (degree);
				__joystickMove (degree, offsetX, offsetY);
			}
		}

		private void __joystickEnd (EventContext context)
		{
			ReleaseKey ();
		}

		void ReleaseKey ()
		{
			UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.UpArrow), false);
			UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.DownArrow), false);
			UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.LeftArrow), false);
			UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.RightArrow), false);
		}

		public static float OffsetX;
		public static float OffsetY;

        private void __joystickMove (float degree,float offsetX,float offsetY)
		{
			ReleaseKey ();
			//AppDebug.Log($"degree:{degree}\t offsetX：{offsetX}\t offsetY:{offsetY}");
			OffsetX = offsetX;
			OffsetY = offsetY;

            var absDegree = Math.Abs(degree);
            if (absDegree > 0 && absDegree < 22.5)
            {
                //AppDebug.Log ($"right");
                UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.RightArrow), true);
            }
            if (absDegree > 22.5 && absDegree < 67.5)
            {
                UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.RightArrow), true);

                if (degree < 0)
                {
                    //AppDebug.Log ($"up");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.UpArrow), true);

                }
                else
                {
                    //AppDebug.Log ($"down");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.DownArrow), true);

                }
            }
            
            if (absDegree > 67.5 && absDegree <= 112.5)
            {
                if (degree < 0)
                {
                    //AppDebug.Log ($"up");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.UpArrow), true);

                }
                else
                {
                    //AppDebug.Log ($"down");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.DownArrow), true);

                }

            }
            
            if (absDegree > 112.5 && absDegree <= 157.5)
            {
                //AppDebug.Log ($"left");
                UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.LeftArrow), true);
                if (degree < 0)
                {
                    //AppDebug.Log ($"up");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.UpArrow), true);

                }
                else
                {
                    //AppDebug.Log ($"down");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.DownArrow), true);

                }
            }
           
            if (absDegree > 157.5 && absDegree <= 180)
            {
                //AppDebug.Log ($"left");
                UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.LeftArrow), true);

            }
            /*var absDegree = Math.Abs(degree);
            if (absDegree > 45 && absDegree < 135)
            {
                if (degree < 0)
                {
                    //AppDebug.Log ($"up");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.UpArrow), true);

                }
                else
                {
                    //AppDebug.Log ($"down");
                    UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.DownArrow), true);

                }
            }

            if (absDegree > 0 && absDegree < 90)
            {
                UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.RightArrow), true);
            }
            else if (absDegree > 90 && absDegree < 180)
            {
                UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.LeftArrow), true);
            }*/
            /*           if (offsetX > 0)
                       {
                           UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.RightArrow), true);
                       }
                       else
                       {
                           UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.LeftArrow), true);
                       }

                       if (offsetY > 0)
                       {
                           UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.DownArrow), true);
                       }
                       else
                       {
                           UI.get().send_key(GLFW_Util.UnityKeyCodeToGLFW_KEY(KeyCode.UpArrow), true);
                       }*/

            /*var absDegree = Math.Abs (degree);
			if (absDegree > 0 && absDegree < 45)
			{
				//AppDebug.Log ($"right");
				UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.RightArrow), true);
			}
			else if (absDegree > 45 && absDegree < 135)
			{
				if (degree < 0)
				{
					//AppDebug.Log ($"up");
					UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.UpArrow), true);

				}
				else
				{
					//AppDebug.Log ($"down");
					UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.DownArrow), true);

				}
			}
			else
			if (absDegree > 135 && absDegree <= 180)
			{
				//AppDebug.Log ($"left");
				UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (KeyCode.LeftArrow), true);

			}
*/
        }

		public void EnterChargePhase (Dictionary<string, string> SkillId_Name_Dict)
		{
			int index = 0;
			foreach (var skillIdString in SkillId_Name_Dict.Keys)
			{
				var skillId = int.Parse (skillIdString);
				var skillIcon = SkillData.get (skillId).get_icon (SkillData.Icon.NORMAL).nTexture;
				var skillBtn = _skillBtns.TryGet (index);
				skillBtn._GLoader_Icon.texture = skillIcon;
				skillBtn.SkillId = skillId;
				skillBtn.SkillIndex = index;
				index++;
			}
		}

		private void OnTouchBegin_SkillBtn (EventContext context)
		{
			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;
			ms.Stage.get ().get_combat ().use_move (clicked_ActionButton.SkillId, true, false);
			_player.Current_ElementAtrr = (Enum_SkillElemAttr)clicked_ActionButton.SkillIndex;
			_player.GetBehaviourTreeOwner ().graph.blackboard.SetVariableValue (typeof (Enum_SkillElemAttr).Name, _player.Current_ElementAtrr);
		}

		private void OnTouchMove_SkillBtn (EventContext context)
		{
			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;
			ms.Stage.get ().get_combat ().use_move (clicked_ActionButton.SkillId, true, true);
		}

		private void OnTouchEnd_SkillBtn (EventContext context)
		{
			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;
			ms.Stage.get ().get_combat ().use_move (clicked_ActionButton.SkillId, false, true);

			//_t_HideBtnSkill.Play ();
			_player.GetBehaviourTreeOwner ().SendEvent (Enum_BTreeEvent.ChooseSkill.ToString());
		}
	}
}