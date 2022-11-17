using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace client.inventory
{
	public enum MapleInventoryType
	{
		UNDEFINED = 0,
		EQUIP = 1,
		USE = 2,
		SETUP = 3,
		ETC = 4,
		CASH = 5,
		CANHOLD = 6,   //Proof-guard for inserting after removal checks
		EQUIPPED = -1 //Seems nexon screwed something when removing an item T_T
	}

	public class MapleInventoryTypeExt
	{

	}
}

