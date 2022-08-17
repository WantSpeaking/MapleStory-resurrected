// ms.Window
using System;
using Helper;
using ms;
using UnityEngine;

namespace ms
{
	public class Window : Singleton<Window>
	{
		private float width_Scaled;

		private float height_Scaled;

		public float ratio;

		private float width_Camera;

		private float height_Camera;

		private float half_delta_width;

		private float half_delta_height;

		private Vector3 last_mousePos;

		private bool fullscreen;

		private float opacity;

		private float opcstep;

		private Action fadeprocedure;

		private short viewwidth;

		private short viewheight;

		public Window ()
		{
			opacity = 1f;
			opcstep = 0f;
			viewwidth = Singleton<Constants>.get ().get_viewwidth ();
			viewheight = Singleton<Constants>.get ().get_viewheight ();
			height_Camera = (float)viewheight * 1f;
			ratio = (float)Screen.height / height_Camera;
			width_Camera = (float)Screen.width / ratio;
			half_delta_width = (width_Camera - (float)viewwidth) / 2f;
			half_delta_height = 0f;
		}

		public new void Dispose ()
		{
		}

		public Error init ()
		{
			fullscreen = ms.Setting<Fullscreen>.get ().load ();
			return initwindow ();
		}

		public Error initwindow ()
		{
			return Error.Code.NONE;
		}

		public bool not_closed ()
		{
			return true;
		}

		public void update ()
		{
			updateopc ();
		}

		public void begin ()
		{
		}

		public void end ()
		{
		}

		public void fadeout (float step, Action fadeproc)
		{
			opcstep = 0f - step;
			fadeprocedure = fadeproc;
		}

		public void check_events ()
		{
			short max_width = Singleton<Configuration>.get ().get_max_width ();
			short max_height = Singleton<Configuration>.get ().get_max_height ();
			short new_width = Singleton<Constants>.get ().get_viewwidth ();
			short new_height = Singleton<Constants>.get ().get_viewheight ();
			if (viewwidth != new_width || viewheight != new_height)
			{
				viewwidth = new_width;
				viewheight = new_height;
				if (new_width >= max_width || new_height >= max_height)
				{
					fullscreen = true;
				}
				initwindow ();
			}
		}

		public void HandleGUIEvents (Event evt)
		{
			if (evt.rawType == EventType.KeyDown)
			{
				Singleton<UI>.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (evt.keyCode), pressed: true);
			}
			else if (evt.rawType == EventType.KeyUp)
			{
				Singleton<UI>.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (evt.keyCode), pressed: false);
			}
			else if (evt.rawType == EventType.MouseDown)
			{
				if (evt.button == 0)
				{
					if (evt.clickCount == 1)
					{
						Singleton<UI>.get ().send_cursor (pressed: true);
					}
					else
					{
						Singleton<UI>.get ().doubleclick ();
					}
				}
				else if (evt.button == 1)
				{
					Singleton<UI>.get ().rightclick ();
				}
			}
			else if (evt.rawType == EventType.MouseUp)
			{
				Singleton<UI>.get ().send_cursor (pressed: false);
			}
			else if (Input.mousePosition != last_mousePos)
			{
				float x = evt.mousePosition.x;
				float y = evt.mousePosition.y;
				Singleton<UI>.get ().send_cursor (new Point_short ((short)((float)(short)x / ratio - half_delta_width), (short)((float)(short)y / ratio)));
			}
		}

		public Vector3 WorldToScreenPoint (Vector3 worldPos)
		{
			return new Vector3 ((worldPos.x + half_delta_width) * ratio, worldPos.y * ratio, worldPos.z);
		}

		public Vector3 ScreenToWorldPoint (Vector3 screenPos)
		{
			return new Vector3 (screenPos.x / ratio - half_delta_width, screenPos.y / ratio, screenPos.z);
		}

		public void setclipboard (string text)
		{
		}

		public string getclipboard ()
		{
			string text = null;
			return (text == null) ? text : "";
		}

		public void toggle_fullscreen ()
		{
			short max_width = Singleton<Configuration>.get ().get_max_width ();
			short max_height = Singleton<Configuration>.get ().get_max_height ();
			if (viewwidth < max_width && viewheight < max_height)
			{
				fullscreen = !fullscreen;
				ms.Setting<Fullscreen>.get ().save (fullscreen);
				initwindow ();
			}
		}

		private void updateopc ()
		{
			if (opcstep != 0f)
			{
				opacity += opcstep;
				if (opacity >= 1f)
				{
					opacity = 1f;
					opcstep = 0f;
				}
				else if (opacity <= 0f)
				{
					opacity = 0f;
					opcstep = 0f - opcstep;
					fadeprocedure ();
				}
			}
		}
	}
}