using System;
using Helper;
using UnityEngine;

//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////
//	This file is part of the continued Journey MMORPG client					//
//	Copyright (C) 2015-2019  Daniel Allendorf, Ryan Payton						//
//																				//
//	This program is free software: you can redistribute it and/or modify		//
//	it under the terms of the GNU Affero General Public License as published by	//
//	the Free Software Foundation, either version 3 of the License, or			//
//	(at your option) any later version.											//
//																				//
//	This program is distributed in the hope that it will be useful,				//
//	but WITHOUT ANY WARRANTY; without even the implied warranty of				//
//	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the				//
//	GNU Affero General Public License for more details.							//
//																				//
//	You should have received a copy of the GNU Affero General Public License	//
//	along with this program.  If not, see <https://www.gnu.org/licenses/>.		//
//////////////////////////////////////////////////////////////////////////////////


namespace ms
{
	public class Window : Singleton<Window>
	{
		public Window ()
		{
			//context = null;
			//glwnd = null;
			opacity = 1.0f;
			opcstep = 0.0f;
			width = Constants.get ().get_viewwidth ();
			height = Constants.get ().get_viewheight ();
		}

		public new void Dispose ()
		{
			//glfwTerminate();
			//base.Dispose();
		}

		public Error init ()
		{
			fullscreen = Setting<Fullscreen>.get ().load ();

			/*if (!glfwInit())
			{
				return Error.Code.GLFW;
			}

			glfwWindowHint(GLFW_VISIBLE, GL_FALSE);
			context = glfwCreateWindow(1, 1, "", null, null);
			glfwMakeContextCurrent(context);
			glfwSetErrorCallback(ms.GlobalMembers.error_callback);
			glfwWindowHint(GLFW_VISIBLE, GL_TRUE);
			glfwWindowHint(GLFW_RESIZABLE, GL_FALSE);

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: Variables cannot be declared in if/while/switch conditions in C#:
			if (Error error = GraphicsGL.get().init())
			{
				return error;
			}*/

			return initwindow ();
		}

		public Error initwindow ()
		{
			/*if (glwnd != null)
			{
				glfwDestroyWindow(glwnd);
			}

			glwnd = glfwCreateWindow(width, height, Configuration.get().get_title(), fullscreen ? glfwGetPrimaryMonitor() : null, context);

			if (glwnd == null)
			{
				return Error.Code.WINDOW;
			}

			glfwMakeContextCurrent(glwnd);

			bool vsync = Setting<VSync>.get().load();
			glfwSwapInterval(vsync ? 1 : 0);

			glViewport(0, 0, width, height);
			glMatrixMode(GL_PROJECTION);
			glLoadIdentity();

			glfwSetInputMode(glwnd, GLFW_CURSOR, GLFW_CURSOR_HIDDEN);

			double xpos;
			double ypos;

			glfwGetCursorPos(glwnd, xpos, ypos);
			cursor_callback(glwnd, xpos, ypos);

			glfwSetInputMode(glwnd, GLFW_STICKY_KEYS, GL_TRUE);
			glfwSetKeyCallback(glwnd, ms.GlobalMembers.key_callback);
			glfwSetMouseButtonCallback(glwnd, ms.GlobalMembers.mousekey_callback);
			glfwSetCursorPosCallback(glwnd, ms.GlobalMembers.cursor_callback);
			glfwSetWindowFocusCallback(glwnd, ms.GlobalMembers.focus_callback);
			glfwSetScrollCallback(glwnd, ms.GlobalMembers.scroll_callback);
			glfwSetWindowCloseCallback(glwnd, ms.GlobalMembers.close_callback);

			string buf = new string(new char[256]);
			GetCurrentDirectoryA(256, buf);
			buf += "\\Icon.png";

			GLFWimage[] images = Arrays.InitializeWithDefaultInstances<GLFWimage>(1);

			var stbi = stbi_load(buf, images[0].width, images[0].height, 0, 4);

			if (stbi == null)
			{
				return new Error(Error.Code.MISSING_ICON, stbi_failure_reason());
			}

			images[0].pixels = stbi;

			glfwSetWindowIcon(glwnd, 1, images);
			stbi_image_free(images[0].pixels);

			GraphicsGL.get().reinit();*/

			return Error.Code.NONE;
		}

		public bool not_closed ()
		{
			//return glfwWindowShouldClose(glwnd) == 0;
			return true;
		}

		public void update ()
		{
			updateopc ();
		}

		public void begin ()
		{
			//GraphicsGL.get().clearscene();
		}

		public void end ()
		{
			//GraphicsGL.get().flush(opacity);
			//glfwSwapBuffers(glwnd);
		}

		public void fadeout (float step, System.Action fadeproc)
		{
			opcstep = -step;
			fadeprocedure = fadeproc;
		}

		public void check_events ()
		{
			short max_width = Configuration.get ().get_max_width ();
			short max_height = Configuration.get ().get_max_height ();
			short new_width = Constants.get ().get_viewwidth ();
			short new_height = Constants.get ().get_viewheight ();

			if (width != new_width || height != new_height)
			{
				width = new_width;
				height = new_height;

				if (new_width >= max_width || new_height >= max_height)
				{
					fullscreen = true;
				}

				initwindow ();
			}

			//glfwPollEvents();
		}

		private Vector3 last_mousePos;

		public void HandleGUIEvents (Event evt)
		{
			if (evt.rawType == EventType.KeyDown)
			{
				UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (evt.keyCode), true);
			}
			else if (evt.rawType == EventType.KeyUp)
			{
				UI.get ().send_key (GLFW_Util.UnityKeyCodeToGLFW_KEY (evt.keyCode), false);
			}
			else if (evt.rawType == EventType.MouseDown)
			{
				if (evt.button == 0)
				{
					if (evt.clickCount == 1)
					{
						UI.get ().send_cursor (true);
					}
					else
					{
						UI.get ().doubleclick ();
					}
				}
				else if (evt.button == 1)
				{
					UI.get ().rightclick ();
				}
			}
			else if (evt.rawType == EventType.MouseUp)
			{
				UI.get ().send_cursor (false);
			}
			else if (Input.mousePosition != last_mousePos)
			{
				last_mousePos = Input.mousePosition;
				var mousePos = new Point_short ((short)last_mousePos.x, (short)(Constants.get ().get_viewheight () - last_mousePos.y));
				//Debug.Log ($"Input.mousePosition:{Input.mousePosition}\t mousePos:{mousePos}\t Screen.height:{Screen.height}\t evt.mousePosition:{evt.mousePosition}");
				UI.get ().send_cursor (mousePos);
			}
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void setclipboard(const string& text) const
		public void setclipboard (string text)
		{
			//glfwSetClipboardString(glwnd, text);
		}

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: string getclipboard() const
		public string getclipboard ()
		{
//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: C# does not have an equivalent to pointers to value types:
//ORIGINAL LINE: const sbyte* text = glfwGetClipboardString(glwnd);
			/*sbyte text = glfwGetClipboardString(glwnd)*/
			;
			string text = null;

			return text == null ? text : "";
		}

		public void toggle_fullscreen ()
		{
			short max_width = Configuration.get ().get_max_width ();
			short max_height = Configuration.get ().get_max_height ();

			if (width < max_width && height < max_height)
			{
				fullscreen = !fullscreen;
				Setting<Fullscreen>.get ().save (fullscreen);

				initwindow ();
				//glfwPollEvents();
			}
		}

		private void updateopc ()
		{
			if (opcstep != 0.0f)
			{
				opacity += opcstep;

				if (opacity >= 1.0f)
				{
					opacity = 1.0f;
					opcstep = 0.0f;
				}
				else if (opacity <= 0.0f)
				{
					opacity = 0.0f;
					opcstep = -opcstep;

					fadeprocedure ();
				}
			}
		}

		//private GLFWwindow glwnd;
		//private GLFWwindow context;
		private bool fullscreen;
		private float opacity;
		private float opcstep;
		private System.Action fadeprocedure;
		private short width;
		private short height;
	}
}