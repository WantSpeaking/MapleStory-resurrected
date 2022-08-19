/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_NpcTalk
	{
		public UINpcTalk.TalkType type;
		public UINpcTalk uINpcTalk;

		public void OnCreate ()
		{
			_Txt_text.onClickLink.Add (onClickLink);

			_Btn_Close.onClick.Add (onClick_Btn_Close);
			_Btn_Next.onClick.Add (onClick_Btn_Next);
			_Btn_OK.onClick.Add (onClick_Btn_Next);
			_Btn_Prev.onClick.Add (onClick_Btn_Prev);
			_Btn_YES.onClick.Add (onClick_Btn_Next);
			_Btn_No.onClick.Add (onClick_Btn_Prev);
			_Btn_Accept.onClick.Add (onClick_Btn_Next);
			_Btn_Decline.onClick.Add (onClick_Btn_Prev);
		}
		private new void onClickLink (EventContext context)
		{
			int.TryParse ((string)context.data, out var selection);
			new NpcTalkMorePacket (selection).dispatch ();
			AppDebug.Log ($"onClickLink,L{context.data}");
		}
		private void onClick_Btn_Close (EventContext context)
		{
			//ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_NpcTalk> ();
			UI.get ().get_element<UINpcTalk> ().get()?.deactivate();
			new NpcTalkMorePacket ((sbyte)type, -1).dispatch ();
		}
		private void onClick_Btn_Next (EventContext context)
		{
			//ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_NpcTalk> ();
			UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

			new NpcTalkMorePacket ((sbyte)type, 1).dispatch ();
		}
		private void onClick_Btn_Prev (EventContext context)
		{
			UI.get ().get_element<UINpcTalk> ().get ()?.deactivate ();

			//ms_Unity.FGUI_Manager.Instance.CloseFGUI<ms_Unity.FGUI_NpcTalk> ();

			new NpcTalkMorePacket ((sbyte)type, 0).dispatch ();
		}

		public void OnVisiblityChanged (bool isVisible, UINpcTalk uINpcTalk)
		{
			this.uINpcTalk = uINpcTalk;
			uINpcTalk.SetFGUI_NpcTalk (this);
			if (isVisible)
			{
				//SetGList ();
			}
			AppDebug.Log ($"uINpcTalk OnVisiblityChanged isVisible:{isVisible}");

		}

		public void change_text()
		{
			_GLoader_speaker.texture = uINpcTalk.speaker.nTexture;
			_GLoader_nametag.texture = uINpcTalk.nametag.nTexture;
			_Txt_name.text = uINpcTalk.name.get_text();
			_Txt_text.text = uINpcTalk.text.get_text();

			_c_TalkType.selectedIndex = (int)uINpcTalk.type;
		}
	}
}