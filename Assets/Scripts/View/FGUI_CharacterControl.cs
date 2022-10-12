/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System;
using FairyGUI;
using FairyGUI.Utils;
using Helper;
using ms;

namespace ms_Unity
{
	public partial class FGUI_CharacterControl
	{
		public void OnCreate ()
		{
			_Joystick.onMove.Add (__joystickMove);
			_Joystick.onEnd.Add (__joystickEnd);

		}

		
		private void __joystickEnd (EventContext context)
		{
			ReleaseKey ();
		}

		void ReleaseKey ()
		{
/*			UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Up), false);
			UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Down), false);
			UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Left), false);
			UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Right), false);*/
		}

		private void __joystickMove (EventContext context)
		{
			ReleaseKey ();

			float degree = (float)context.data;


			var absDegree = Math.Abs (degree);
			if (absDegree > 0 && absDegree < 45)
			{
				//AppDebug.Log ($"right");
				//UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Right), true);
			}
			else if (absDegree > 45 && absDegree < 135)
			{
				if (degree < 0)
				{
					//AppDebug.Log ($"up");
					//UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Up), true);

				}
				else
				{
					//AppDebug.Log ($"down");
					//UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Down), true);

				}
			}
			else
			if (absDegree > 135 && absDegree <= 180)
			{
				//AppDebug.Log ($"left");
				//UI.get ().send_key (GLFW_Util.XNAKeyCodeToGLFW_KEY (Keys.Left), true);

			}

		}
	}
}