/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_YesNo
	{
		public string message;
		public NoticeType t;
		public Text.Alignment a;
		public int count;
		public int max;
		public System.Action<bool> yesnohandler;
		
		public void OnCreate ()
		{
            this._Btn_Yes.onClick.Add(OnClick_Btn_Yes);
            this._Btn_No.onClick.Add(OnClick_Btn_No);
        }
        private void OnClick_Btn_Yes(EventContext context)
        {
			yesnohandler?.Invoke(true);
            deactivate();
        }
        private void OnClick_Btn_No(EventContext context)
        {
            yesnohandler?.Invoke(false);
            deactivate();
        }
        private void deactivate()
        {
            ms_Unity.FGUI_Manager.Instance.CloseFGUI<FGUI_YesNo>();
            GRoot.inst.HidePopup(this);
        }

        public static void ShowNotice (string message, System.Action<bool> yh = null, Text.Alignment alignment = Text.Alignment.CENTER)
		{
            ms_Unity.FGUI_Manager.Instance.PanelOpening = true;
            var thisNotice = ms_Unity.FGUI_Manager.Instance.OpenFGUI<FGUI_YesNo> () as FGUI_YesNo;

			thisNotice.message = message;
			thisNotice.a = alignment;
			thisNotice.yesnohandler = yh;

			thisNotice._tet_message.text = message;

			thisNotice.Center ();

		}
	}
}