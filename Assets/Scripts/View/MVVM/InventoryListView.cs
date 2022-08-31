using FairyGUI;
using Loxodon.Framework.Binding;
using Loxodon.Framework.Observables;
using Loxodon.Framework.Views;
using ms;
using System;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace ms_Unity
{
	public class InventoryListView : UIView
	{
		public class ItemClickedEvent : UnityEvent<int>
		{
			public ItemClickedEvent ()
			{
			}
		}
		public FGUI_ItemInventory FGUI_View;

		private ObservableList<InventoryListItemViewModel> items;

		public GList contentEQUIP => FGUI_View._GList_Equip;
		public GList contentUSE => FGUI_View._GList_Use;
		public GList contentSETUP => FGUI_View._GList_Setup;
		public GList contentETC => FGUI_View._GList_Etc;
		public GList contentCASH => FGUI_View._GList_Cash;

		public GameObject itemTemplate;

		public ItemClickedEvent OnSelectChanged = new ItemClickedEvent ();

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
				}
			}
		}

		protected void OnEquipsChanged (object sender, NotifyCollectionChangedEventArgs eventArgs)
		{
			OnCollectionChanged (sender, eventArgs, contentEQUIP);
		}

		private void OnCashsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentCASH);
		}

		private void OnEtcsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentETC);
		}

		private void OnSetupsChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentSETUP);
		}

		private void OnUsesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			OnCollectionChanged (sender, e, contentUSE);
		}

		public ObservableList<InventoryListItemViewModel> Items
		{
			get { return this.items; }
			set
			{
				if (this.items == value)
					return;

				if (this.items != null)
					this.items.CollectionChanged -= OnCollectionChanged;

				this.items = value;

				this.OnInventoriesChanged ();

				if (this.items != null)
					this.items.CollectionChanged += OnCollectionChanged;
			}
		}

		protected override void OnDestroy ()
		{
			if (this.items != null)
				this.items.CollectionChanged -= OnCollectionChanged;
		}
		protected void OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs eventArgs)
		{

		}
		protected void OnCollectionChanged (object sender, NotifyCollectionChangedEventArgs eventArgs, GList parent)
		{
			switch (eventArgs.Action)
			{
				case NotifyCollectionChangedAction.Add:
					this.AddItem (eventArgs.NewStartingIndex, eventArgs.NewItems[0], parent);
					break;
				case NotifyCollectionChangedAction.Remove:
					this.RemoveItem (eventArgs.OldStartingIndex, eventArgs.OldItems[0], parent);
					break;
				case NotifyCollectionChangedAction.Replace:
					this.ReplaceItem (eventArgs.OldStartingIndex, eventArgs.OldItems[0], eventArgs.NewItems[0], parent);
					break;
				case NotifyCollectionChangedAction.Reset:
					this.ResetItem (parent);
					break;
				case NotifyCollectionChangedAction.Move:
					this.MoveItem (eventArgs.OldStartingIndex, eventArgs.NewStartingIndex, eventArgs.NewItems[0], parent);
					break;
			}
		}
		protected virtual void OnInventoriesChanged ()
		{
			int tempIndex = 0;
			foreach (var slot in inventories[InventoryType.Id.EQUIP].Values)
			{
				this.AddItem (tempIndex++, slot, contentEQUIP);
			}
			tempIndex = 0;
			foreach (var slot in inventories[InventoryType.Id.USE].Values)
			{
				this.AddItem (tempIndex++, slot, contentUSE);
			}
			tempIndex = 0;
			foreach (var slot in inventories[InventoryType.Id.SETUP].Values)
			{
				this.AddItem (tempIndex++, slot, contentSETUP);
			}
			tempIndex = 0;
			foreach (var slot in inventories[InventoryType.Id.ETC].Values)
			{
				this.AddItem (tempIndex++, slot, contentETC);
			}
			tempIndex = 0;
			foreach (var slot in inventories[InventoryType.Id.CASH].Values)
			{
				this.AddItem (tempIndex++, slot, contentCASH);
			}
		}

		protected virtual void OnSelectChange (GameObject itemViewGo)
		{
			/*if (this.OnSelectChanged == null || itemViewGo == null)
				return;

			for (int i = 0; i < this.contentEQUIP.childCount; i++)
			{
				var child = this.contentEQUIP.GetChild (i);
				if (itemViewGo.GList == child)
				{
					this.OnSelectChanged.Invoke (i);
					break;
				}
			}*/
		}

		private Vector3 localScale = new Vector3 (1, -1, 1);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item">ms.Inventory.Slot</param>
		/// <param name="parent"></param>
		protected virtual void AddItem (int index, object item, GList parent)
		{
			var FGUI_View = parent.AddItemFromPool () as FGUI_Itemed_ListItem;
			var MonoBehaviour_View = FGUI_View.displayObject.gameObject.AddComponent<InventoryListItemView> ();
			FGUI_View.MonoBehaviour_View = MonoBehaviour_View;
			MonoBehaviour_View.FGUI_View = FGUI_View;

			FGUI_View.MonoBehaviour_View.SetDataContext (item);

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
			parent.RemoveChildToPoolAt (index);
			/*GList GList = this.contentEQUIP.GetChild (index);
			UIView itemView = GList.GetComponent<UIView> ();
			if (itemView.GetDataContext () == item)
			{
				itemView.gameObject.SetActive (false);
				Destroy (itemView.gameObject);
			}*/
		}

		protected virtual void ReplaceItem (int index, object oldItem, object item, GList parent)
		{
			var FGUI_View =  parent.GetChildAt (index) as FGUI_Itemed_ListItem;
			if (FGUI_View.MonoBehaviour_View.GetDataContext () == oldItem)
			{
				FGUI_View.MonoBehaviour_View.SetDataContext (item);
			}

			/*GList GList = this.contentEQUIP.GetChild (index);
			UIView itemView = GList.GetComponent<UIView> ();
			if (itemView.GetDataContext () == oldItem)
			{
				itemView.SetDataContext (item);
			}*/
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
	}

}