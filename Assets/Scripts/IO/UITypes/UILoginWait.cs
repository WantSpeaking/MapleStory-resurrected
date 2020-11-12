#define USE_NX

using System;

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
	public class UILoginWait : UIElement
	{
		public const Type TYPE = UIElement.Type.LOGINWAIT;
		public const bool FOCUSED = true;
		public const bool TOGGLED = false;

//C++ TO C# CONVERTER CRACKED BY X-CRACKER 2017 TODO TASK: The implementation of the following method could not be found:
//		UILoginWait() : UILoginWait(() => TangibleLambdaToken69UILoginWait(System.Action okhandler);

		public UILoginWait () : this ((Action)null)
		{
		}

		public UILoginWait (params object[] args) : this ((System.Action)args[0])
		{
		}

		public UILoginWait (System.Action okhandler)
		{
			this.okhandler = okhandler;
			var Loading = nl.nx.wzFile_ui["Login.img"]["Notice"]["Loading"];
			var backgrnd = Loading["backgrnd"];

			sprites.Add (backgrnd);
			sprites.Add (new Sprite(Loading["circle"], new Point<short> (127, 70)));

			buttons[(uint)Buttons.CANCEL] = new MapleButton (Loading["BtCancel"], new Point<short> (101, 106));

			position = new Point<short> (276, 229);
			dimension = new Texture (backgrnd).get_dimensions ();
		}

		public override UIElement.Type get_type ()
		{
			return TYPE;
		}

		public void close ()
		{
			deactivate ();
			okhandler ();
		}

		public System.Action get_handler ()
		{
			return okhandler;
		}

		public override Button.State button_pressed (ushort id)
		{
			Session.get ().reconnect ();

			close ();

			return Button.State.NORMAL;
		}

		private enum Buttons : ushort
		{
			CANCEL
		}

		private System.Action okhandler;
	}
}