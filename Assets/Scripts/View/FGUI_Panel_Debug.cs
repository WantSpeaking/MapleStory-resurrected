using FairyGUI;
using FairyGUI.Utils;
using System;
using UnityEngine;

namespace ms_Unity
{
    public partial class FGUI_Panel_Debug
    {
        public void OnCreate()
        {
            _Btn_Close.onClick.Add(OnCLick_Btn_Close);
        }

        private void OnCLick_Btn_Close(EventContext context)
        {
            FGUI_Manager.Instance.CloseFGUI<FGUI_Panel_Debug>();
        }

        public void SetLogMessage(string message)
        {
            _c1.selectedIndex = 1;
            _Txt_Log.text = message;
        }

       
    }
}