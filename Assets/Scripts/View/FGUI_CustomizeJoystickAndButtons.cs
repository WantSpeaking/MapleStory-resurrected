using FairyGUI;
using FairyGUI.Utils;
using ms;
using System;
using static ms.UIActionButton;

namespace ms_Unity
{
    public partial class FGUI_CustomizeJoystickAndButtons
    {
        

        public void OnCreate()
        {
            _ActionButtons._Btn_HeavyAttack.draggable = true;
            _ActionButtons._Btn_LightAttack.draggable = true;
            _ActionButtons._Btn_Jump        .draggable = true;
            _ActionButtons._Btn_PickUp      .draggable = true;
            _ActionButtons._Btn_Skill1.draggable = true;
            _ActionButtons._Btn_Skill2.draggable = true;
            _ActionButtons._Btn_Skill3.draggable = true;
            _ActionButtons._Btn_Skill4.draggable = true;
            _ActionButtons._Btn_Skill5.draggable = true;
            _ActionButtons._Btn_Skill6.draggable = true;

            _ActionButtons._c_Mode.selectedIndex = 1;

            _Btn_OK.onClick.Add(OnClick_OK);
            _Btn_Cancel.onClick.Add(OnClick_Cancel);
            _Btn_Reset.onClick.Add(OnClick_Reset);
        }


        private void OnClick_OK(EventContext context)
        {
            _ActionButtons.SaveUIPosToSetting();

            FGUI_Manager.Instance.GetFGUI<FGUI_ActionButtons>().LoadSettingPosToUI();
            Close();
        }

        private void OnClick_Cancel(EventContext context)
        {
            Close();

        }

        private void OnClick_Reset()
        {
            _ActionButtons.ResetToDefaultPos();
        }

        private void Close()
        {
            FGUI_Manager.Instance.CloseFGUI<FGUI_CustomizeJoystickAndButtons>();
        }

        public void OnVisiblityChanged(bool isVisible)
        {
            if (isVisible)
            {
                _ActionButtons.LoadSettingPosToUI();
            }
        }

        //todo 打开的时候 设置成 和 原按键一样的位置
    }
}