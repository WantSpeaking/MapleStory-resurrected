using FairyGUI;
using FairyGUI.Utils;
using UnityEngine;

namespace ms_Unity
{
    public partial class FGUI_Panel_Debug
    {
        public void OnCreate()
        {
			UnityEngine.Application.logMessageReceived += ApplicationOnlogMessageReceived;
        }

        private void ApplicationOnlogMessageReceived (string condition, string stacktrace, LogType type)
        {
	        if (type == LogType.Error || type == LogType.Exception)
	        {
		        GRoot.inst.AddChild (this);
                _c1.selectedIndex = 1;
		        _Txt_Log.text = condition +"\r\n"+ stacktrace;
	        }
        }
    }
}