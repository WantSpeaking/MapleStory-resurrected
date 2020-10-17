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
    public class Constants : Singleton<Constants>
    {
        public Constants()
        {
            VIEWWIDTH = 800;
            VIEWHEIGHT = 600;
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        public short get_viewwidth()
        {
            return VIEWWIDTH;
        }

        public void set_viewwidth(short width)
        {
            VIEWWIDTH = width;
        }

        public short get_viewheight()
        {
            return VIEWHEIGHT;
        }

        public void set_viewheight(short height)
        {
            VIEWHEIGHT = height;
        }

        // Window and screen width.
        private short VIEWWIDTH;
        // Window and screen height.
        private short VIEWHEIGHT;


        public const ushort TIMESTEP = 8;

        public float walkSpeed = 1;
        public float jumpSpeed = 1;
        public float climbSpeed = 1;
        public float flySpeed = 1;
        public float fallSpeed = 1;
        public float animSpeed = 1;

        public float frameDelay = 0.5f;
    }
}