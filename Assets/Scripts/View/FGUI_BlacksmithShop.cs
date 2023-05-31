/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;
using ms;
using System;
using System.Collections;
using System.Collections.Generic;
using static ms.Inventory;
using static ms.KeyConfig;
using static ms.UIUserList;
using static Unity.VisualScripting.Member;

namespace ms_Unity
{
    public partial class FGUI_BlacksmithShop
    {

        public void OnCreate()
        {
            _GList_Equip.onClickItem.Add(OnClick_ItemEquip);
            _GList_Equip.itemRenderer = ListItemRenderer_Equip;

            _GList_Scroll.onClickItem.Add(OnClick_ItemScroll);
            _GList_Scroll.itemRenderer = ListItemRenderer_Scroll;

            _Btn_Enchance.onClick.Add(OnClick_Btn_Enchance);
            _Btn_Close.onClick.Add(OnClick_Close);

        }

        private void OnClick_Close(EventContext context)
        {
            _GList_Equip.selectedIndex = -1;
            _GList_Scroll.selectedIndex = -1;

            FGUI_Manager.Instance.CloseFGUI<FGUI_BlacksmithShop>();
        }

        private void OnClick_Btn_Enchance(EventContext context)
        {
            if (_GList_Scroll.selectedIndex == -1 || _GList_Equip.selectedIndex == -1)
            {
                return;
            }

            var type_slot_scroll = type_slot_scroll_list.TryGet(_GList_Scroll.selectedIndex);
            var type_slot_equip = type_slot_equip_list.TryGet(_GList_Equip.selectedIndex);

            //var equipSLot = inventory.get_all_data()[type_slot_equip.Item1].TryGetValue(type_slot_equip.Item2);

            //EquipData.get(equipSLot.Item_id).get_eqslot();
            //enabled = false;
            //AppDebug.Log($"type_slot_scroll.Item2:{type_slot_scroll.Item2} type_slot_equip.Item2:{type_slot_equip.Item2}");
            new ScrollEquipPacket(type_slot_scroll.Item2, type_slot_equip.Item2, type_slot_equip.Item1 == InventoryType.Id.EQUIPPED).dispatch();
        }
        const string equipLevel = "+{count=0}";

        void ListItemRenderer_Equip(int index, object o)
        {
            var uiItem = o as FGUI_Itemed_ListItem;
            var type_slot = type_slot_equip_list.TryGet(index);

            uiItem._c_EquipStatus.selectedIndex = type_slot.Item1 == InventoryType.Id.EQUIPPED ? 1 : 0;

            uiItem.GetChild("icon").asLoader.texture = getNTextureByIndex(index, type_slot_equip_list);
            /*if (inventory.get_all_data()[type_slot.Item1].TryGetValue(type_slot.Item2, out var slot))
            {
                
            }*/

            var oequip = inventory.get_equip(type_slot.Item1, type_slot.Item2);
            if (oequip)
            {
                var level = oequip.get().get_level();
                uiItem._c_ShowEnchanceCount.selectedIndex = level > 0 ? 1 : 0;
                uiItem._txt_EnchanceCount.SetVar("count", level.ToString()).FlushVars();
            }


        }
        void ListItemRenderer_Scroll(int index, object o)
        {
            var uiItem = o as FGUI_Itemed_ListItem;
            var type_slot = type_slot_scroll_list.TryGet(index);

            uiItem._c_InventoryTypeId.selectedIndex = (int)type_slot.Item1;

            uiItem.GetChild("icon").asLoader.texture = getNTextureByIndex(index, type_slot_scroll_list);


        }
        NTexture getNTextureByIndex(int index, List<(InventoryType.Id, short)> list)
        {
            var type_slot = list.TryGet(index);

            if (inventory.get_all_data()[type_slot.Item1].TryGetValue(type_slot.Item2, out var slot))
            {
                var tex = ItemData.get(slot.Item_id).get_icon(false);

                return tex.nTexture;
            }
            return null;
        }

        ItemData getItemDataByIndex(int index, List<(InventoryType.Id, short)> list)
        {
            var type_slot = list.TryGet(index);

            if (inventory.get_all_data()[type_slot.Item1].TryGetValue(type_slot.Item2, out var slot))
            {
                var tex = ItemData.get(slot.Item_id);

                return tex;
            }
            return null;
        }
        private void OnClick_ItemEquip(EventContext context)
        {
            /*var index = _GList_Equip.GetChildIndex(context.sender as GObject);
            _GLoader_ToEnchanceEquip.texture = getNTextureByIndex(_GList_Equip.selectedIndex);*/

            /* _c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.EQUIP;

             OnClick_Equip(context, _GList_Equip, Tooltip.Parent.ITEMINVENTORY);*/

        }
        private void OnClick_ItemScroll(EventContext context)
        {
            /*var index = _GList_Equip.GetChildIndex(context.sender as GObject);
            _GLoader_ToEnchanceEquip.texture = getNTextureByIndex(_GList_Equip.selectedIndex);*/

            /* _c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.EQUIP;

             OnClick_Equip(context, _GList_Equip, Tooltip.Parent.ITEMINVENTORY);*/

            var itemData = getItemDataByIndex(_GList_Scroll.selectedIndex, type_slot_scroll_list);
            if (itemData != null)
            {
                _txt_EnchanceDesc.text = $"{itemData.get_name()}\r\n{itemData.get_desc()}";
            }

        }

        List<(InventoryType.Id, short)> type_slot_equip_list = new List<(InventoryType.Id, short)>();
        List<(InventoryType.Id, short)> type_slot_scroll_list = new List<(InventoryType.Id, short)>();

        Inventory inventory;


        public void Refresh()
        {
            //enabled = true;

            _GList_Equip.selectedIndex = -1;
            _GList_Scroll.selectedIndex = -1;

            type_slot_equip_list.Clear();

            inventory = ms.Stage.get()?.get_player()?.get_inventory();

            foreach (var item in inventory.get_all_data()[InventoryType.Id.EQUIPPED])
            {
                type_slot_equip_list.Add((InventoryType.Id.EQUIPPED, item.Key));
            }

            foreach (var item in inventory.get_all_data()[InventoryType.Id.EQUIP])
            {
                type_slot_equip_list.Add((InventoryType.Id.EQUIP, item.Key));
            }

            _GList_Equip.numItems = type_slot_equip_list.Count;



            type_slot_scroll_list.Clear();

            inventory = ms.Stage.get()?.get_player()?.get_inventory();

            foreach (var item in inventory.get_all_data()[InventoryType.Id.USE])
            {
                var isScroll = ItemData.get(item.Value.Item_id)?.isScroll() ?? false;

                if (isScroll)
                {
                    //AppDebug.Log($"{ItemData.get(item.Value.Item_id).get_id() / 10000}");
                    type_slot_scroll_list.Add((InventoryType.Id.USE, item.Key));
                }
            }

            _GList_Scroll.numItems = type_slot_scroll_list.Count;
        }
    }
}