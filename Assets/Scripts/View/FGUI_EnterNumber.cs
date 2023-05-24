/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using ms;
using NodeCanvas.Tasks.Conditions;
using UnityEngine;

namespace ms_Unity
{
	public partial class FGUI_EnterNumber
	{
		public string message;
		public NoticeType t;
		public Text.Alignment a;
		public long count;
		public long max;
		public System.Action<int> numhandler;

		public void OnCreate ()
		{
			this._gTextInput_Number.onChanged.Add (On_gTextInput_Number_Changed);
			this._Btn_Yes.onClick.Add (OnClick_Btn_Yes);
			this._Btn_No.onClick.Add (deactivate);
			_Btn_1_4.onClick.Add (OnClick_1_4);
			_Btn_2_4.onClick.Add (OnClick_2_4);
			_Btn_3_4.onClick.Add (OnClick_3_4);
			_Btn_Max.onClick.Add (OnClick_Max);
		}

		private void OnClick_1_4 ()
		{
			_gTextInput_Number.text = ((int)Math.Floor(max * 0.25f)).ToString();
		}
		private void OnClick_2_4 ()
		{
			_gTextInput_Number.text = ((int)Math.Floor(max * 0.5f)).ToString();
		}
		private void OnClick_3_4 ()
		{
			_gTextInput_Number.text = ((int)Math.Floor(max * 0.75f)).ToString();
		}
		private void OnClick_Max ()
		{
			_gTextInput_Number.text = max.ToString ();
		}
		private void On_gTextInput_Number_Changed (EventContext context)
		{
			if (_gTextInput_Number.text.ToInt () > max)
			{
				_gTextInput_Number.text = max.ToString ();
			}
		}
		private void OnClick_Btn_Yes (EventContext context)
		{
			handlestring (_gTextInput_Number.text);
		}

		private void handlestring (string numstr)
		{
			int num = -1;
			bool has_only_digits = (numstr.find_first_not_of ("0123456789") == -1);

			//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Lambda expressions cannot be assigned to 'var':
			Action<bool> okhandler = (bool UnnamedParameter1) =>
			{
				//numfield.set_state (Textfield.State.FOCUSED);
				//buttons[(int)Buttons.OK].set_state (Button.State.NORMAL);
			};

			if (!has_only_digits)
			{
				//numfield.set_state (Textfield.State.DISABLED);
				//UI.get ().emplace<UIOk> ("Only numbers are allowed.", okhandler);
				FGUI_OK.ShowNotice (message: "Only numbers are allowed.", okhandler: okhandler);

				return;
			}
			else
			{
				num = Convert.ToInt32 (numstr);
			}

			if (num < 1)
			{
				//numfield.set_state (Textfield.State.DISABLED);

				//UI.get ().emplace<UIOk> ("You may only enter a number equal to or higher than 1.", okhandler);

				FGUI_OK.ShowNotice (message: "You may only enter a number equal to or higher than 1.", okhandler: okhandler);

				return;
			}
			else if (num > max)
			{
				//numfield.set_state (Textfield.State.DISABLED);
				//UI.get ().emplace<UIOk> ("You may only enter a number equal to or lower than " + Convert.ToString (max) + ".", okhandler);
				FGUI_OK.ShowNotice (message: "You may only enter a number equal to or lower than " + Convert.ToString (max) + ".", okhandler: okhandler);

				return;
			}
			else
			{
				numhandler (num);
				deactivate ();
			}

			//buttons[(int)Buttons.OK].set_state (Button.State.NORMAL);
		}
		private void deactivate()
		{
			ms_Unity.FGUI_Manager.Instance.CloseFGUI<FGUI_EnterNumber> ();
			GRoot.inst.HidePopup (this);

		}
		public static void ShowNotice (string message, System.Action<int> nh, long max, long quantity)
		{
            ms_Unity.FGUI_Manager.Instance.PanelOpening = true;
            var thisNotice = ms_Unity.FGUI_Manager.Instance.OpenFGUI<FGUI_EnterNumber> () as FGUI_EnterNumber;
			GRoot.inst.ShowPopup (thisNotice);

			thisNotice.message = message;
			thisNotice.max = max;
			thisNotice.count = quantity;
			thisNotice.numhandler = nh;

			thisNotice.Center ();

			thisNotice._tet_Title.text = message;

			thisNotice._gTextInput_Number.text = quantity.ToString ();
		}
	}
}