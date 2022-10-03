using FairyGUI;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Observables;
using Loxodon.Framework.Views;
using ms;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Stage = ms.Stage;

namespace ms_Unity
{
	public partial class FGUI_ItemInventory
	{
		public InventoryListView MonoBehaviour_View;
		Inventory inventory;

		EquipTooltip equipTooltip = new EquipTooltip ();
		ItemTooltip itemTooltip = new ItemTooltip ();
		StringBuilder sb = new StringBuilder ();
		InventoryType.Id _tab;

		InventoryType.Id tab
		{
			set
			{
				_tab = value;
				_c_InventoryTypeId.selectedIndex = (int)value;
			}
			get
			{
				return (InventoryType.Id)(_c_InventoryTypeId.selectedIndex);
			}
		}

		short current_slot;

		public GList contentEQUIP => _GList_Equip;
		public GList contentUSE => _GList_Use;
		public GList contentSETUP => _GList_Setup;
		public GList contentETC => _GList_Etc;
		public GList contentCASH => _GList_Cash;

		public EnumMapNew<InventoryType.Id, ObservableSortedDictionary<short, ms.Inventory.Slot>> inventories;

		public EnumMapNew<InventoryType.Id, ObservableSortedDictionary<short, ms.Inventory.Slot>> Inventories
		{
			get { return this.inventories; }
			set
			{
				if (this.inventories == value)
					return;

				if (this.inventories != null)
				{
					this.inventories[InventoryType.Id.EQUIP].CollectionChanged -= OnEquipsChanged;
					this.inventories[InventoryType.Id.USE].CollectionChanged -= OnUsesChanged;
					this.inventories[InventoryType.Id.SETUP].CollectionChanged -= OnSetupsChanged;
					this.inventories[InventoryType.Id.ETC].CollectionChanged -= OnEtcsChanged;
					this.inventories[InventoryType.Id.CASH].CollectionChanged -= OnCashsChanged;
					this.inventories[InventoryType.Id.EQUIPPED].CollectionChanged -= OnEquippedsChanged;

				}

				this.inventories = value;

				this.OnInventoriesChanged ();

				if (this.inventories != null)
				{
					this.inventories[InventoryType.Id.EQUIP].CollectionChanged += OnEquipsChanged;
					this.inventories[InventoryType.Id.USE].CollectionChanged += OnUsesChanged;
					this.inventories[InventoryType.Id.SETUP].CollectionChanged += OnSetupsChanged;
					this.inventories[InventoryType.Id.ETC].CollectionChanged += OnEtcsChanged;
					this.inventories[InventoryType.Id.CASH].CollectionChanged += OnCashsChanged;
					this.inventories[InventoryType.Id.EQUIPPED].CollectionChanged += OnEquippedsChanged;
				}
			}
		}

		public EnumMapNew<InventoryType.Id, ObservableSortedDictionary<short, Icon>> icons = new EnumMapNew<InventoryType.Id, ObservableSortedDictionary<short, Icon>> ();

		private const ushort ROWS = 8;
		private const ushort COLUMNS = 4;
		private const ushort MAXSLOTS = ROWS * COLUMNS;
		private const ushort MAXFULLSLOTS = COLUMNS * MAXSLOTS;
		private const ushort MAXEQUIPPEDSLOTS = 19;

		UIStatsInfo uIStatsInfo;
		UIStatsInfo UIStatsInfo
		{
			get
			{
				uIStatsInfo ??= UI.get ().emplace<UIStatsInfo> (Stage.get ().get_player ().get_stats ());
				uIStatsInfo.deactivate ();
				return uIStatsInfo;
			}
		}

		public FGUI_ItemInventory OnCreate ()
		{
			_GLoader_Player.texture = new NTexture (FGUI_Manager.Instance.playerRenderTexture);

			_GList_Equip.onClickItem.Add (OnClick_ItemEquip);
			_GList_Use.onClickItem.Add (OnClick_ItemUse);
			_GList_Etc.onClickItem.Add (OnClick_ItemEtc);
			_GList_Setup.onClickItem.Add (OnClick_ItemSetup);
			_GList_Cash.onClickItem.Add (OnClick_ItemCash);
			_GList_Equipped.onClickItem.Add (OnClick_ItemEquipped);

			/*			requirements.Add (MapleStat.Id.LEVEL);
						requirements.Add (MapleStat.Id.STR);
						requirements.Add (MapleStat.Id.DEX);
						requirements.Add (MapleStat.Id.INT);
						requirements.Add (MapleStat.Id.LUK);*/

			_GList_Equip.numItems = MAXFULLSLOTS;
			_GList_Use.numItems = MAXFULLSLOTS;
			_GList_Etc.numItems = MAXFULLSLOTS;
			_GList_Setup.numItems = MAXFULLSLOTS;
			_GList_Cash.numItems = MAXFULLSLOTS;
			_GList_Equipped.numItems = MAXEQUIPPEDSLOTS;

			inventory = Stage.get ()?.get_player ()?.get_inventory ();

			if (inventory != null)
			{
				inventory.PropertyChanged += OnInventoryPropertyChanged;
				_Btn_Meso.GetChild ("title").asTextField.SetVar ("Meso", inventory.Meso.ToString ()).FlushVars ();
				inventory.PropertyChanged += OnInventoryPropertyChanged;
				for (short i = 0; i < MAXEQUIPPEDSLOTS; i++)
				{
					var child_Equipped = _GList_Equipped.GetChildAt (i) as FGUI_Itemed_ListItem;
					child_Equipped._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.EQUIPPED;
				}

				for (short i = 0; i < MAXFULLSLOTS; i++)//position:300,160
				{
					var child_Equip = _GList_Equip.GetChildAt (i) as FGUI_Itemed_ListItem;
					var child_Use = _GList_Use.GetChildAt (i) as FGUI_Itemed_ListItem;
					var child_Etc = _GList_Etc.GetChildAt (i) as FGUI_Itemed_ListItem;
					var child_Setup = _GList_Setup.GetChildAt (i) as FGUI_Itemed_ListItem;
					var child_Cash = _GList_Cash.GetChildAt (i) as FGUI_Itemed_ListItem;

					child_Equip._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.EQUIP;
					child_Use._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.USE;
					child_Etc._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.ETC;
					child_Setup._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.SETUP;
					child_Cash._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.CASH;


					if (i > inventory.get_slotmax (InventoryType.Id.EQUIP))
					{
						child_Equip.enabled = false;
					}
					if (i > inventory.get_slotmax (InventoryType.Id.USE))
					{
						child_Use.enabled = false;
					}
					if (i > inventory.get_slotmax (InventoryType.Id.ETC))
					{
						child_Etc.enabled = false;
					}
					if (i > inventory.get_slotmax (InventoryType.Id.SETUP))
					{
						child_Setup.enabled = false;
					}
					if (i > inventory.get_slotmax (InventoryType.Id.CASH))
					{
						child_Cash.enabled = false;
					}
				}

				Inventories = inventory.get_all_data ();
				foreach (var type_dict in inventories)
				{
					var type = type_dict.Key;
					foreach (var slotIndex_slot in type_dict.Value)
					{

						AppDebug.Log ($"type:{type} slotIndex:{slotIndex_slot.Key} slotname:{ItemData.get (slotIndex_slot.Value.Item_id)?.get_name ()}");
					}
				}
			}
			_Itemed_ItemDetail._Btn_Close.onClick.Add (OnClick_Btn_Close);
			_Itemed_ItemDetail._ClickToHide.onClick.Add (OnClick_Btn_Close);

			_Itemed_ItemDetail._Btn_Equip.onClick.Add (doubleclick);
			_Itemed_ItemDetail._Btn_UnEquip.onClick.Add (OnClick_Btn_UnEquip);
			_Itemed_ItemDetail._Btn_Drop.onClick.Add (OnClick_Btn_Drop);
			_Itemed_ItemDetail._Btn_Use.onClick.Add (doubleclick);
			_Itemed_ItemDetail._Btn_SetToUseSlot.onClick.Add (OnClick_SetToUseSlot);

			_Btn_Gather.onClick.Add (OnClick_Btn_Gather);
			_Btn_Meso.onClick.Add (OnClick_Btn_Meso);

			_SetupUseButtons._GList_UseBtns.onClickItem.Add (OnClick_UseSlot);

			return this;
		}

		private void OnClick_UseSlot (EventContext context)
		{
			var useSlot = context.data as FGUI_Btn_Joystick_Acton;
			int item_id = inventory.get_item_id (tab, current_slot);
			var keyconfig = UI.get ().get_element<UIKeyConfig> ();
			Keyboard.Mapping mapping = new Keyboard.Mapping (KeyType.Id.ITEM, item_id);
			keyconfig.get ().stage_mapping (useSlot.Key, mapping);
			keyconfig.get ().save_staged_mappings ();
			_SetupUseButtons.visible = false;
			_SetupUseButtons.UpdateIcon ();
			FGUI_Manager.Instance.GetFGUI<FGUI_ActionButtons> ()?.UpdateIcon ();
			OnClick_Btn_Close (context);
		}

		private void OnClick_SetToUseSlot (EventContext context)
		{
			_SetupUseButtons.visible = true;
			_SetupUseButtons.UpdateIcon ();
		}

		private void OnInventoryPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case "Meso":
					_Btn_Meso.GetChild ("title").asTextField.SetVar ("Meso", inventory.Meso.ToString ()).FlushVars ();
					break;
				default:
					break;
			}
		}

		private void OnClick_Btn_Meso (EventContext context)
		{
			var dropCount = Math.Min (ms.Stage.Instance.get_player ().get_inventory ().get_meso (), 50000);
			FGUI_EnterNumber.ShowNotice ("您想扔出多少金币？", NoticeType.ENTERNUMBER, max: dropCount, count: dropCount, numhandler: (meso) => new MesoDropPackets (meso).dispatch ());
		}
		private void DropMeso (int count)
		{
			new MesoDropPackets (count).dispatch ();
		}
		private void OnClick_Btn_Gather (EventContext context)
		{
			new GatherItemsPacket (tab).dispatch ();
		}


		#region item detial function
		private void doubleclick (EventContext context)
		{
			if (current_slot == 0)
				return;
			{
				int item_id = inventory.get_item_id (tab, current_slot);
				if (item_id != 0)
				{
					switch (tab)
					{
						case InventoryType.Id.EQUIP:
							{
								if (UI.get ().get_element<UIItemInventory> ().get ().can_wear_equip (current_slot))
								{
									new EquipItemPacket (current_slot, inventory.find_equipslot (item_id)).dispatch ();
								}

								break;
							}
						case InventoryType.Id.USE:
							{
								new UseItemPacket (current_slot, item_id).dispatch ();
								break;
							}
					}
					_Itemed_ItemDetail.visible = false;
				}
			}
		}
		private void OnClick_Btn_UnEquip (EventContext context)
		{
			_Itemed_ItemDetail.visible = false;

			if (current_slot == 0)
				return;
			{
				short freeslot = inventory.find_free_slot (InventoryType.Id.EQUIP);
				if (freeslot != 0)
				{
					new UnequipItemPacket ((short)current_slot, freeslot).dispatch ();
				}

			}
		}

		private void OnClick_Btn_Close (EventContext context)
		{
			OnHideItemDetial ();
			OnHideSetupUseSlot ();
		}

		const string dropmessage = "How many will you drop?";
		const string untradablemessage = "This item can't be taken back once thrown away.\\nWill you still drop it?";
		const string cashmessage = "You can't drop this item.";

		public void OnClick_Btn_Drop (EventContext context)
		{
			if (icons[tab].TryGetValue (current_slot, out var icon))
			{
				icon.drop_on_stage ();
			}
			OnClick_Btn_Close(context);
		}


		#endregion

		#region  OnClick list item
		private void OnClick_ItemEquip (EventContext context)
		{
			_c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.EQUIP;

			OnClick_Equip (context, _GList_Equip, Tooltip.Parent.ITEMINVENTORY);

		}
		private void OnClick_ItemUse (EventContext context)
		{
			_c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.USE;

			OnClick_Item (context, _GList_Use, Tooltip.Parent.ITEMINVENTORY, InventoryType.Id.USE);

		}
		private void OnClick_ItemSetup (EventContext context)
		{
			_c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.SETUP;

			OnClick_Item (context, _GList_Setup, Tooltip.Parent.ITEMINVENTORY, InventoryType.Id.SETUP);

		}
		private void OnClick_ItemEtc (EventContext context)
		{
			_c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.ETC;

			OnClick_Item (context, _GList_Etc, Tooltip.Parent.ITEMINVENTORY, InventoryType.Id.ETC);

		}
		private void OnClick_ItemCash (EventContext context)
		{
			_c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.CASH;
			OnClick_Item (context, _GList_Cash, Tooltip.Parent.ITEMINVENTORY, InventoryType.Id.CASH);

		}

		private void OnClick_ItemEquipped (EventContext context)
		{
			_c_InventoryTypeId.selectedIndex = _Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = (int)InventoryType.Id.EQUIPPED;

			OnClick_Equip (context, _GList_Equipped, Tooltip.Parent.EQUIPINVENTORY);

		}

		private void OnClick_Item (EventContext context, GList gList, Tooltip.Parent parent, InventoryType.Id tab)
		{
			var clicked_item = context.data as FGUI_Itemed_ListItem;
			current_slot = (short)(gList.GetChildIndex (clicked_item) + 1);

			int item_id = inventory.get_item_id (tab, current_slot);
			if (itemTooltip.set_item (item_id) == false)
			{
				OnClick_Btn_Close (context);
				return;
			}

			_Itemed_ItemDetail.visible = true;
			_Itemed_ItemDetail._ItemIcon._c_InventoryTypeId.selectedIndex = (int)tab;
			_Itemed_ItemDetail._ItemIcon.GetChild ("icon").asLoader.texture = clicked_item.GetChild ("icon").asLoader.texture;

			sb.Clear ();

			sb.AppendLine (itemTooltip.name.get_text ());

			//数量
			if (itemTooltip.untradable || itemTooltip.unique)
			{
				sb.AppendLine (itemTooltip.qual.get_text ());
			}
			sb.AppendLine (itemTooltip.desc.get_text ());

			_Itemed_ItemDetail._txt_equipStat.text = sb.ToString ();


		}
		private void OnClick_Equip (EventContext context, GList gList, Tooltip.Parent parent)
		{
			var clicked_item = context.data as FGUI_Itemed_ListItem;
			current_slot = (short)(gList.GetChildIndex (clicked_item) + 1);

			equipTooltip.set_equip (parent, (short)current_slot);
			if (!equipTooltip.hasEquip)
			{
				current_slot = 0;
				_Itemed_ItemDetail.visible = false;
				return;
			}
			_Itemed_ItemDetail.visible = true;
			_Itemed_ItemDetail._ItemIcon._c_InventoryTypeId.selectedIndex = (int)tab;
			_Itemed_ItemDetail._ItemIcon.GetChild ("icon").asLoader.texture = clicked_item.GetChild ("icon").asLoader.texture;

			sb.Clear ();

			sb.AppendLine (equipTooltip.name.get_text ());

			//装备穿戴条件
			sb.AppendLine ($"穿戴条件:");
			foreach (MapleStat.Id ms in equipTooltip.requirements)
			{
				Point_short reqpos = equipTooltip.reqstatpositions[ms];
				bool reqok = equipTooltip.canequip[ms];
				sb.AppendLine ($"\t需要 {ms} {equipTooltip.reqstatstrings[ms]}， {(reqok ? "满足" : "不满足")}");
			}

			//哪些职业可以穿戴
			sb.Append ("可以穿戴的职业:");
			foreach (var jbit in equipTooltip.okjobs)
			{
				sb.Append (jbit);
			}
			sb.AppendLine ();
			//装备类别
			sb.AppendLine (equipTooltip.category.get_text ());

			//武器速度
			if (equipTooltip.is_weapon)
			{
				sb.AppendLine (equipTooltip.wepspeed.get_text ());
			}

			//增加的属性
			foreach (var label in equipTooltip.statlabels.dict.Values)
			{
				if (label?.empty () ?? true)
				{
					continue;
				}
				sb.AppendLine (label.get_text ());
			}

			//升级次数
			if (equipTooltip.hasslots)
			{
				sb.AppendLine (equipTooltip.slots.get_text ());
			}

			//描述
			if (equipTooltip.hasdesc)
			{
				sb.AppendLine (equipTooltip.desc.get_text ());
			}

			//稀有度标签
			if (!equipTooltip.potflag.empty ())
			{
				sb.AppendLine (equipTooltip.potflag.get_text ());
			}

			//使用的金锤子
			sb.AppendLine (equipTooltip.hammers.get_text ());

			_Itemed_ItemDetail._txt_equipStat.text = sb.ToString ();
		}
		#endregion

		#region OnCollectionChanged
		protected void OnEquipsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentEQUIP, InventoryType.Id.EQUIP);
		}

		private void OnCashsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentCASH, InventoryType.Id.CASH);
		}

		private void OnEtcsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentETC, InventoryType.Id.ETC);

			//item 有变化 
			ms.Stage.get ().UpdateQuest ();
		}

		private void OnSetupsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentSETUP, InventoryType.Id.SETUP);
		}

		private void OnUsesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentUSE, InventoryType.Id.USE);
		}

		protected void OnEquippedsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, _GList_Equipped, InventoryType.Id.EQUIPPED);
		}

		protected void OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs eventArgs, GList parent, InventoryType.Id typeId)
		{
			switch (eventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					this.AddItem (eventArgs.NewStartingIndex, (KeyValuePair<short, ms.Inventory.Slot>)eventArgs.NewItems[0], parent, typeId);
					break;
				case NotifyCollectionChangedAction.Remove:
					this.RemoveItem (eventArgs.OldStartingIndex, eventArgs.OldItems[0], parent);
					break;
				case NotifyCollectionChangedAction.Replace:
					this.ReplaceItem (eventArgs.OldStartingIndex, eventArgs.OldItems[0], eventArgs.NewItems[0], parent, typeId);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.ResetItem (parent);
					break;
				case NotifyCollectionChangedAction.Move:
					this.MoveItem (eventArgs.OldStartingIndex, eventArgs.NewStartingIndex, eventArgs.NewItems[0], parent);
					break;
			}
		}
		#endregion

		#region Modify GList
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item">ms.Inventory.Slot</param>
		/// <param name="parent"></param>
		protected virtual void AddItem (int index, KeyValuePair<short, ms.Inventory.Slot> keyValuePair, GList parent, InventoryType.Id typeId)
		{
			/*			var FGUI_View = parent.AddItemFromPool () as FGUI_InventoryListItemView;
						var MonoBehaviour_View = FGUI_View.displayObject.gameObject.AddComponent<InventoryListItemView> ();
						FGUI_View.MonoBehaviour_View = MonoBehaviour_View;
						MonoBehaviour_View.FGUI_View = FGUI_View;

						FGUI_View.MonoBehaviour_View.SetDataContext (item);*/
			tab = typeId;
			update_slot (keyValuePair.Key);
			update_slot_FGUI (keyValuePair.Key, parent);

			/*var itemViewGo = Instantiate (this.itemTemplate);
			itemViewGo.GList.SetParent (parent, false);
			itemViewGo.GList.SetSiblingIndex (index);

			Button button = itemViewGo.GetComponent<Button> ();
			button.onClick.AddListener (() => OnSelectChange (itemViewGo));
			itemViewGo.SetActive (true);
			var itemView = itemViewGo.GetComponent<UIView> () as InventoryListItemView;
			itemView.image.GList.localScale = localScale;

			var slot =(ms.Inventory.Slot)item;
			itemViewGo.name = ItemData.get (slot.Item_id)?.get_name ();
			itemView.SetDataContext (item);*/
		}

		protected virtual void RemoveItem (int index, object item, GList parent)
		{
			var keyValuePair = (KeyValuePair<short, ms.Inventory.Slot>)item;
			update_slot (keyValuePair.Key);

			index = keyValuePair.Key - 1;
			var slot = keyValuePair.Value;
			FGUI_Itemed_ListItem FGUI_View = null;
			if (index >= parent.numChildren)
			{
				parent.RemoveChildToPoolAt (index);
			}
			else
			{
				FGUI_View = parent.GetChildAt (index) as FGUI_Itemed_ListItem;
				var iconGLoader = FGUI_View.GetChild ("icon")?.asLoader;
				if (iconGLoader != null)
					iconGLoader.texture = null;
			}



			/*GList GList = this.contentEQUIP.GetChild (index);
			UIView itemView = GList.GetComponent<UIView> ();
			if (itemView.GetDataContext () == item)
			{
				itemView.gameObject.SetActive (false);
				Destroy (itemView.gameObject);
			}*/
		}

		protected virtual void ReplaceItem (int index, object oldItem, object item, GList parent, InventoryType.Id typeId)
		{
			var keyValuePair_Old = (KeyValuePair<short, ms.Inventory.Slot>)oldItem;
			var keyValuePair_New = (KeyValuePair<short, ms.Inventory.Slot>)item;

			//AppDebug.Log ($"Count1:{keyValuePair.Value.Count} Count2:{keyValuePair2.Value.Count}");

			tab = typeId;

			//update_slot (keyValuePair_Old.Key);
			update_slot (keyValuePair_New.Key);

			//update_slot_FGUI (keyValuePair_Old.Key, parent);
			update_slot_FGUI (keyValuePair_New.Key, parent);

			
		}

		protected virtual void MoveItem (int oldIndex, int index, object item, GList parent)
		{
			var child = parent.GetChildAt (oldIndex);
			parent.SetChildIndex (child, index);

			/*GList GList = this.contentEQUIP.GetChild (oldIndex);
			UIView itemView = GList.GetComponent<UIView> ();
			itemView.GList.SetSiblingIndex (index);*/
		}

		protected virtual void ResetItem (GList parent)
		{
			parent.RemoveChildrenToPool ();
			/*for (int i = this.contentEQUIP.childCount - 1; i >= 0; i--)
			{
				GList GList = this.contentEQUIP.GetChild (i);
				Destroy (GList.gameObject);
			}*/
		}
		#endregion

		private void OnSlotChanged ()
		{

		}

		private void update_slot (short slot)
		{
			int item_id = inventory.get_item_id (tab, slot);
			if (item_id != 0)
			{
				short count;

				if (tab == InventoryType.Id.EQUIP)
				{
					count = -1;
				}
				else
				{
					count = inventory.get_item_count (tab, slot);
				}

				bool untradable = ItemData.get (item_id).is_untradable ();
				bool cashitem = ItemData.get (item_id).is_cashitem ();
				Texture texture = new Texture (ItemData.get (item_id).get_icon (false));
				EquipSlot.Id eqslot = inventory.find_equipslot (item_id);

				icons[tab][slot] = new Icon (new ItemIcon (UI.get ().get_element<UIItemInventory> ().get (), tab, eqslot, slot, item_id, count, untradable, cashitem), texture, count);
			}
			else if (icons[tab].count (slot) > 0)
			{
				icons[tab].Remove (slot);
			}
		}

		private void update_slot_FGUI (short Key, GList glist)
		{
			if (icons[tab].TryGetValue (Key, out var icon))
			{
				var index = Key - 1;
				FGUI_Itemed_ListItem FGUI_View = null;
				NTexture nTexture = null;
				short count = icon.count;

				if (index >= glist.numChildren)
				{
					FGUI_View = glist.AddItemFromPool () as FGUI_Itemed_ListItem;
				}
				else
				{
					FGUI_View = glist.GetChildAt (index) as FGUI_Itemed_ListItem;
				}
				FGUI_View._c_InventoryTypeId.selectedIndex = (int)tab;
				FGUI_View._c_count.selectedIndex = count > 1 ? 1 : 0;
				FGUI_View.text = count.ToString ();
				nTexture = icon.get_texture ().nTexture;
				var iconGLoader = FGUI_View.GetChild ("icon")?.asLoader;
				if (iconGLoader != null)
					iconGLoader.texture = nTexture;
			}
		}
		public override void Dispose ()
		{
			base.Dispose ();
			/*if (this.items != null)
				this.items.CollectionChanged -= OnCollectionChanged;*/
		}

		protected virtual void OnInventoriesChanged ()
		{
			foreach (var slotIndex_slot in inventories[InventoryType.Id.EQUIP])
			{
				this.AddItem (slotIndex_slot.Key - 1, slotIndex_slot, contentEQUIP, InventoryType.Id.EQUIP);
			}
			foreach (var slotIndex_slot in inventories[InventoryType.Id.USE])
			{
				this.AddItem (slotIndex_slot.Key - 1, slotIndex_slot, contentUSE, InventoryType.Id.USE);
			}
			foreach (var slotIndex_slot in inventories[InventoryType.Id.SETUP])
			{
				this.AddItem (slotIndex_slot.Key - 1, slotIndex_slot, contentSETUP, InventoryType.Id.SETUP);
			}
			foreach (var slotIndex_slot in inventories[InventoryType.Id.ETC])
			{
				this.AddItem (slotIndex_slot.Key - 1, slotIndex_slot, contentETC, InventoryType.Id.ETC);
			}
			foreach (var slotIndex_slot in inventories[InventoryType.Id.CASH])
			{
				this.AddItem (slotIndex_slot.Key - 1, slotIndex_slot, contentCASH, InventoryType.Id.CASH);
			}
			foreach (var slotIndex_slot in inventories[InventoryType.Id.EQUIPPED])
			{
				this.AddItem (slotIndex_slot.Key - 1, slotIndex_slot, _GList_Equipped, InventoryType.Id.EQUIPPED);
			}
		}

		private void OnHideItemDetial ()
		{
			current_slot = 0;
			_Itemed_ItemDetail.visible = false;
			itemTooltip.clear_set_item ();

			//_Itemed_ItemDetail._c_InventoryTypeId.selectedIndex = 0;
			_Itemed_ItemDetail._ItemIcon.GetChild ("icon").asLoader.texture = null;
		}

		private void OnHideSetupUseSlot()
		{
			_SetupUseButtons.visible = false;
		}
		protected override void OnUpdate ()
		{
			base.OnUpdate ();

			sb.Clear ();
			for (int i = 0; i < (int)StatLabel.NUM_LABELS; i++)
			{
				var textInfo = @$"{(StatLabel)i}:{UIStatsInfo.statlabels[i]?.get_text () ?? "null"}";

				sb.AppendLine (textInfo);
				//AppDebug.Log (textInfo);
			}
			var sbText = sb.ToString ();
			_StatsInfo._Txt_StatsInfo.text = sbText;
			//AppDebug.Log (@$"sbText:{sbText}");
		}
	}
}