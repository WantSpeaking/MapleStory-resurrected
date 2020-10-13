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
//# include "NxFiles.h"

//#if USE_NX
/*# include <fstream>

# include <nlnx/node.hpp>
# include <nlnx/nx.hpp>*/

namespace ms
{
	public class NxFiles
	{
		// Number of needed files
		short NUM_FILES = 19;

		// Names of the needed game files
		string[] filenames =
		{
			"Base.nx",
			"Character.nx",
			"Effect.nx",
			"Etc.nx",
			"Item.nx",
			"Map.nx",
			"Map001.nx", // From latest
			"MapLatest.nx", // From latest
			"MapPretty.nx", // From v167
			"Mob.nx",
			"Morph.nx",
			"Npc.nx",
			"Quest.nx",
			"Reactor.nx",
			"Skill.nx",
			"Sound.nx", // From latest
			"String.nx",
			"TamingMob.nx",
			"UI.nx" // From latest
		};

		/*Error init()
		{
			/*for (var filename in filenames)
				if (std::ifstream{ filename }.good() == false)
					return Error(Error::Code::MISSING_FILE, filename);#1#

			try
			{
				nl::nx::load_all();
			}
			catch (const std::exception&ex)
			{
				static const std::string message = ex.what();

				return Error(Error::Code::NLNX, message.c_str());
			}

			constexpr const char* POSTCHAOS_BITMAP = "Login.img/WorldSelect/BtChannel/layer:bg";

			if (nl::nx::ui.resolve(POSTCHAOS_BITMAP).data_type() != nl::node::type::bitmap)
				return Error::Code::WRONG_UI_FILE;

			return Error::Code::NONE;
			}
		}*/
		public static void init (string suffix)
		{
			nl.nx.load_all(suffix);
		}
		
	}
}
//#endif