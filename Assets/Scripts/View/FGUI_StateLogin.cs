/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using System;

namespace ms_Unity
{
    public partial class FGUI_StateLogin
    {
        void OnCreate ()
		{
            _Btn_Start.onClick.Add(OnClick_Btn_Start);
		}

        public void StartCheck()
        {
            this.visible = true;
            
            AndroidIntentCallToUnZipData.Instance.CheckData(OnChecking, OnCheckComplete, OnError);
        }
        /// <summary>
        /// progress (0,1)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="progress"></param>
        public void OnChecking(string message, float progress)
        {
            _c_State.selectedIndex = 0;
            _Txt_Message.text = message;
            _ProgressBar_CheckData.value = progress * 100;
            //AppDebug.Log(message + progress);
        }

        public void OnCheckComplete(string message, bool result)
        {
            _c_State.selectedIndex = 1;
            //AppDebug.Log("OnCheckComplete");
        }
        public void OnError(string message)
        {
            AppDebug.Log($"OnError:{message}");
            FGUI_Manager.Instance.ShowPopupMessage(message);
        }
        private void OnClick_Btn_Start(EventContext context)
        {
            this.visible = false;
            MapleStory.Instance.init();
        }
    }
}