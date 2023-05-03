using constants.inventory;
using FairyGUI;
using FairyGUI.Utils;
using ms;
using System;
using System.ComponentModel;
using UnityEngine.Rendering;
using static ms.Configuration;
using static ms.UIShop;

namespace ms_Unity
{
    public partial class FGUI_Shop
    {
        private ms.UIShop _uiShop;
        private Loxodon.Framework.Observables.ObservableList<UIShop.SellItem> _sellItems => _uiShop.sellstate.items;
        private Loxodon.Framework.Observables.ObservableList<UIShop.BuyItem> _buyItems => _uiShop.buystate.items;
        private ms.UIShop.BuyState _buyState => _uiShop.buystate;
        private ms.UIShop.SellState _sellState => _uiShop.sellstate;

        private Inventory inventory;

        public void OnCreate()
        {
            _GLoader_Player.texture = new NTexture(FGUI_Manager.Instance.playerRenderTexture);

            _GList_Sell.itemRenderer = ItemRenderer_Sell;
            _GList_Buy.itemRenderer = ItemRenderer_Buy;
            _GList_Sell.onClickItem.Add(OnClick_SellItem);
            _GList_Buy.onClickItem.Add(OnClick_BuyItem);

            _uiShop = ms.UI.get().get_element<UIShop>();

            _c_Buy.onChanged.Add(On_c_Buy_Changed);
            _c_Sell.onChanged.Add(On_c_Sell_Changed);
            _Btn_Leave.onClick.Add(OnClick_Leave);

            inventory = ms.Stage.get()?.get_player()?.get_inventory();
            _Btn_Meso.GetChild("title").asTextField.SetVar("Meso", inventory.Meso.ToString()).FlushVars();
            inventory.PropertyChanged += OnInventoryPropertyChanged;

        }
        private void ItemRenderer_Sell(int index, GObject item)
        {
            var sellItem = _sellItems[index];
            ItemData idata = ItemData.get(sellItem.get_id());

            if (item is FGUI_Shop_SellItem glistItem)
            {
                glistItem._GLoader_Icon.texture = sellItem.get_icon().nTexture;
                glistItem._Txt_Name.text = idata.get_name();
                glistItem._Txt_Price.SetVar("Meso", idata.get_price().ToString()).FlushVars();

                glistItem._c_count.selectedIndex = sellItem.get_sellable() > 1 ? 1 : 0;

                glistItem._c_IsCharge.selectedIndex = 0;
                if(ItemConstants.isRechargeable(idata.get_id()))
                {
                     var buyItemIndex = getBuyItem(sellItem.get_id());
                    if(buyItemIndex != -1)
                    {
                        if(_buyItems.TryGet(buyItemIndex) is ms.UIShop.BuyItem buyItem)
                        {
                            if(sellItem.get_sellable() < buyItem.get_buyable())
                            {
                                glistItem._c_IsCharge.selectedIndex = 1;
                                glistItem._Btn_Charge.GetChild("title").asTextField.SetVar("count", buyItem.get_chargePrice().ToString()).FlushVars();
                                glistItem._Btn_Charge.onClick.Clear();
                                glistItem._Btn_Charge.onClick.Add(OnClickBtnCharge);

                            }
                        }

                    }
                }
                glistItem.text = sellItem.get_sellable().ToString();
            }
        }

        private void OnClickBtnCharge(EventContext context)
        {
            var selection = _GList_Sell.GetChildIndex(((GObject)context.sender).parent);
            _sellState.selection = (short)(selection);
            _sellState.recharge();
            context.StopPropagation();
        }

        private int getSlotMax(int sellItemId)
        {
            int ret= 0;
            foreach ( var buyItem in _buyItems )
            {
                if (buyItem.get_id()== sellItemId)
                {
                    ret = buyItem.get_buyable();
                }
            }
            return ret;
        }
        private int getBuyItem(int sellItemId)
        {
            int ret = -1;
            for (int i = 0; i < _buyItems.Count; i++)
            {
                if (_buyItems[i].get_id() == sellItemId)
                {
                    ret = i;
                }
            }
            
            return ret;
        }
        private void ItemRenderer_Buy(int index, GObject item)
        {
            var buyItem = _buyItems[index];
            ItemData idata = ItemData.get(buyItem.get_id());

            if (item is FGUI_Shop_BuyItem glistItem)
            {
                glistItem._GLoader_Icon.texture = buyItem.get_icon().nTexture;
                glistItem._Txt_Name.text = idata.get_name();
                glistItem._Txt_Price.SetVar("Meso", idata.get_price().ToString()).FlushVars();
            }
        }

        private void OnClick_SellItem(EventContext context)
        {
            /*var uiSkillInfo = context.data as FGUI_ListItem_SkillInfo;
            var skillInfo = UISkillBook.skills[_GList_SkillInfo.selectedIndex];

            _Txt_Desc.text = skillInfo.get_full_desc();*/
            var selection = _GList_Sell.GetChildIndex((GObject)context.data);
            _sellState.selection = (short)(selection);
            _sellState.sell(false);
        }
        private void OnClick_BuyItem(EventContext context)
        {
            /*var uiSkillInfo = context.data as FGUI_ListItem_SkillInfo;
            var skillInfo = UISkillBook.skills[_GList_SkillInfo.selectedIndex];

            _Txt_Desc.text = skillInfo.get_full_desc();*/
            var selection = _GList_Buy.GetChildIndex((GObject)context.data);
            _buyState.selection = (short)selection;
            _buyState.buy();
        }

        private void On_c_Buy_Changed(EventContext context)
        {
        }

        private void On_c_Sell_Changed(EventContext context)
        {
            _uiShop.changeselltab((InventoryType.Id)_c_Sell.selectedIndex);
        }
        private void Sell_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            _GList_Sell.numItems = _sellItems.Count;
            //SetGList ();
        }
        private void Buy_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //SetGList ();
            _GList_Buy.numItems = _buyItems.Count;
        }

        public FGUI_Shop OnVisiblityChanged(bool visible, ms.UIShop uiShop)
        {
            _uiShop = uiShop;
            //AppDebug.Log($"OnCreate _uiShop：{_uiShop.GetHashCode()}\t items:{_uiShop.sellstate.items.GetHashCode()}");

            _GLoader_Npc.texture = _uiShop.get_npc_texture().nTexture;



            _c_Sell.selectedIndex = 1;
            return this;
        }

        public void Refresh()
        {
            _GList_Sell.numItems = _sellItems.Count;
            _GList_Buy.numItems = _buyItems.Count;


        }

        private void OnClick_Leave(EventContext context)
        {
            _uiShop.exit_shop();
        }

        private void OnInventoryPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Meso":
                    _Btn_Meso.GetChild("title").asTextField.SetVar("Meso", inventory.Meso.ToString()).FlushVars();
                    break;
                default:
                    break;
            }
        }
    }
}