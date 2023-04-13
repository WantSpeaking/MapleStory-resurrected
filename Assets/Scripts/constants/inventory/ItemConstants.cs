using System;
using System.Collections;
using System.Collections.Generic;
using client.inventory;
using UnityEngine;

namespace constants.inventory
{
    public class ItemConstants
    {
        protected static Dictionary<int, MapleInventoryType> inventoryTypeCache = new Dictionary<int, MapleInventoryType> ();

        public static MapleInventoryType getInventoryType ( int itemId)
        {
            if (inventoryTypeCache.ContainsKey (itemId))
            {
                return inventoryTypeCache.TryGetValue (itemId);
            }

            MapleInventoryType ret = MapleInventoryType.UNDEFINED;

             byte type = (byte)(itemId / 1000000);
            if (type >= 1 && type <= 5)
            {
                ret = (MapleInventoryType) (type);
            }

            inventoryTypeCache.Add (itemId, ret);
            return ret;
        }

        public static bool isMedal (int itemId)
        {
            return itemId >= 1140000 && itemId < 1143000;
        }

        public static bool isThrowingStar(int itemId)
        {
            return itemId / 10000 == 207;
        }

        public static bool isBullet(int itemId)
        {
            return itemId / 10000 == 233;
        }
        public static bool isRechargeable(int itemId)
        {
            return isThrowingStar(itemId) || isBullet(itemId);
        }
    }
}

