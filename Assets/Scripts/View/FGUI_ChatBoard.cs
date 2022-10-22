/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Linq;
using System.Text;
using FairyGUI;
using FairyGUI.Utils;
using ms;

namespace ms_Unity
{
	public partial class FGUI_ChatBoard : GComponent
	{


		UIChatBar chatBar;
		public void OnCreate ()
		{
			_txt_ChatInput.onSubmit.Add (OnSubmit_ChatInput);
			_Btn_SendMessage.onClick.Add (OnSubmit_ChatInput);
			//_txt_ChatInput.onChanged.Add (OnSubmit_ChatInput);
		}

		private void OnSubmit_ChatInput (EventContext context)
		{
			AppDebug.Log ($"OnSubmit_ChatInput:{_txt_ChatInput.text}");
			new GeneralChatPacket (_txt_ChatInput.text, true).dispatch ();
			_txt_ChatInput.text = string.Empty;
			UnFocus ();
		}

		public void Focus()
		{
			chatBar.Focus_chatfield();
			//AppDebug.Log ("Focus");
		}
		public void UnFocus ()
		{
			chatBar.UnFocus_chatfield ();
			//AppDebug.Log ("UnFocus");

		}
		StringBuilder stringBuilder = new StringBuilder ();
		int textCount = 0;
		protected override void OnUpdate ()
		{
			base.OnUpdate ();
			if (chatBar == null)
			{
				chatBar = UI.get ().get_element<UIChatBar> ();
				_txt_ChatInput.onFocusIn.Add (Focus);
				_txt_ChatInput.onFocusOut.Add (UnFocus);
			}

			if (chatBar != null && textCount != chatBar.rowtexts.Count)
			{
				textCount = chatBar.rowtexts.Count;
				stringBuilder.Clear ();

				/*				foreach (var text in chatBar.rowtexts.Values)
								{
									stringBuilder.AppendLine (text.get_text ());
								}
								_txt_Chat.text = stringBuilder.ToString ();*/
				_txt_Chat.text = chatBar.rowtexts.Values.Last ()?.get_text ();
			}

		}
	}
}