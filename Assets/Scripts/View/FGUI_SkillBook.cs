/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_SkillBook
	{
		UISkillBook UISkillBook => UI.get ().get_element<UISkillBook> ();
		public void OnCreate ()
		{
			//UISkillBook = UI.get ().emplace<UISkillBook> (ms.Stage.get ().get_player ().get_stats (), ms.Stage.get ().get_player ().get_skills ());
			_GList_SkillInfo.itemRenderer = ItemRenderer;
			_GList_SkillInfo.onClickItem.Add (OnClick_SkillInfo);
			UISkillBook.skills.CollectionChanged += Skills_CollectionChanged;

			//_ActionButtons.onClickAction.Add (OnClickAction);

			_SetupActionButtons._Btn_Skill1.onClick.Add (OnClick_Btn_Skill);
			_SetupActionButtons._Btn_Skill2.onClick.Add (OnClick_Btn_Skill);
			_SetupActionButtons._Btn_Skill3.onClick.Add (OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill4.onClick.Add (OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill5.onClick.Add(OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill6.onClick.Add(OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill7.onClick.Add(OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill8.onClick.Add(OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill9.onClick.Add(OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill10.onClick.Add(OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill11.onClick.Add(OnClick_Btn_Skill);
            _SetupActionButtons._Btn_Skill12.onClick.Add(OnClick_Btn_Skill);

            _SetupActionButtons.UpdateIcon ();
			//_SetupActionButtons._PlaceHolder.onClick.Add (CancelSetupAction);
			
			this.onClick.Add (OnClick_SkillBook);

			_BT_TAB0.onClick.Add (() => UISkillBook.button_pressed ((ushort)ms.UISkillBook.Buttons.BT_TAB0));
			_BT_TAB1.onClick.Add (() => UISkillBook.button_pressed ((ushort)ms.UISkillBook.Buttons.BT_TAB1));
			_BT_TAB2.onClick.Add (() => UISkillBook.button_pressed ((ushort)ms.UISkillBook.Buttons.BT_TAB2));
			_BT_TAB3.onClick.Add (() => UISkillBook.button_pressed ((ushort)ms.UISkillBook.Buttons.BT_TAB3));
			_BT_TAB4.onClick.Add (() => UISkillBook.button_pressed ((ushort)ms.UISkillBook.Buttons.BT_TAB4));

		}

		private void OnClick_SkillBook (EventContext context)
		{
			/*if (isSettingupAction)
			{
				CancelSetupAction ();
			}*/
		}

		void CancelSetupAction ()
		{
			_c_SetupAction.selectedIndex = 0;
			//_Btn_SetupSkill.visible = false;
		}

		bool isSettingupAction => _GList_SkillInfo.selectedIndex != -1 && _c_SetupAction.selectedIndex == 1;

		private void OnClick_Btn_Skill (EventContext context)
		{
			if (isSettingupAction == false)
				return;

			var clicked_ActionButton = (FGUI_Btn_Joystick_Acton)context.sender;

			var skillInfo = UISkillBook.skills[_GList_SkillInfo.selectedIndex];

			var keyconfig = UI.get ().get_element<UIKeyConfig> ();

			ms.Keyboard.Mapping mapping = new ms.Keyboard.Mapping (KeyType.Id.SKILL, skillInfo.get_id ());


			keyconfig.get ().stage_mapping (clicked_ActionButton.Key, mapping);
			keyconfig.get ().save_staged_mappings ();

			_SetupActionButtons.UpdateIcon ();

			CancelSetupAction ();

			FGUI_Manager.Instance.GetFGUI<FGUI_ActionButtons> ().UpdateIcon ();

			 /*GGraph holder = new GGraph();
    holder.SetSize(100, 100);
    holder.DrawRect(200,100,1,UnityEngine.Color.black,UnityEngine.Color.red);
    this.AddChild(holder);
	AppDebug.Log("testdraw");*/
		}

		private void OnClickAction (EventContext context)
		{


		}

		private void Skills_CollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			SetGList ();
		}

		private void OnClick_SkillInfo (EventContext context)
		{
			var uiSkillInfo = context.data as FGUI_ListItem_SkillInfo;
			var skillInfo = UISkillBook.skills[_GList_SkillInfo.selectedIndex];

			_Txt_Desc.text = skillInfo.get_full_desc();
		}

		bool localVisible = false;
		protected override void OnUpdate ()
		{
			base.OnUpdate ();
			/*		if (localVisible != visible)
					{
						localVisible = visible;
						OnVisiblityChanged (localVisible);
					}*/
		}
		void change_sp ()
		{
			_Txt_RemainSP.SetVar ("RemainSP",  ms.Stage.get ().get_player ().get_stats ().get_stat(MapleStat.Id.SP).ToString()).FlushVars();
		}
		public void OnVisiblityChanged (bool isVisible)
		{
            UISkillBook.SetFGUI (this);
			if (isVisible)
			{
				SetGList ();
			}
		}

		public void SetJobLevelTab(int index)
		{
			_c_Tab_Visible.selectedIndex = index;
		}

		void SetGList ()
		{
			var jobLevel = (int)UISkillBook.job.get_level ();
			_c_BT_TAB.selectedIndex = jobLevel;
			_c_Tab_Visible.selectedIndex = jobLevel;

			foreach (var child in _GList_SkillInfo.GetChildren())
			{
				if (child is FGUI_ListItem_SkillInfo item_SkillInfo)
				{
					item_SkillInfo._Btn_BT_SPUP0.onClick.Clear ();
					item_SkillInfo._Btn_SetupSkill.onClick.Clear ();
				}
			}

			_GList_SkillInfo.numItems = UISkillBook.skills.Count;
			var string_SkillName = string.Empty;
			foreach (var child in _GList_SkillInfo.GetChildren ())
			{
				if (child is FGUI_ListItem_SkillInfo item_SkillInfo)
				{
					item_SkillInfo._Btn_BT_SPUP0.onClick.Add(OnClick_Btn_BT_SPUP0);
					item_SkillInfo._Btn_SetupSkill.onClick.Add(OnClick_Btn_SetupSkill);
					string_SkillName = string_SkillName.append (item_SkillInfo._Txt_Name.text.Replace(" ","") +" = "+ UISkillBook.skills[_GList_SkillInfo.GetChildIndex(child)].get_id() + ",\n");
				}
			}
			//AppDebug.LogError (string_SkillName);
			change_sp ();
		}

		private void OnClick_Btn_BT_SPUP0 (EventContext context)
		{
			var clickedBtnSPUP = context.sender as GObject;
			var selectedIndex = _GList_SkillInfo.GetChildIndex (clickedBtnSPUP.parent);
			_GList_SkillInfo.selectedIndex = selectedIndex;
			var skillInfo = UISkillBook.skills.TryGet(selectedIndex);
			
			UISkillBook.spend_sp (skillInfo.get_id ());
			UISkillBook.change_sp ();
			change_sp ();
			clickedBtnSPUP.visible = UISkillBook.can_raise (skillInfo.get_id ());
		}
		private void OnClick_Btn_SetupSkill (EventContext context)
		{
			_c_SetupAction.selectedIndex = 1;
		}
		private void ItemRenderer (int index, GObject item)
		{
			var skillInfo = UISkillBook.skills[index];
			var glistItem = item as FGUI_ListItem_SkillInfo;
			glistItem._GLoader_Icon.texture = skillInfo.nTexture_Icon_Normal;
			glistItem._Txt_Name.text = skillInfo.get_namestr ();
			glistItem._Txt_Level.SetVar ("level", skillInfo.get_levelstr ()).FlushVars();
			glistItem._Btn_BT_SPUP0.visible = UISkillBook.can_raise (skillInfo.get_id ());
			glistItem._Btn_SetupSkill.visible = (!SkillData.get (skillInfo.get_id ()).is_passive())&& skillInfo.get_level()!= 0;
			
		}
	}
}