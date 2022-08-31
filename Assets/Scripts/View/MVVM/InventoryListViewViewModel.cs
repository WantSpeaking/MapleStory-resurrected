
using System;
using System.Text.RegularExpressions;

using Loxodon.Log;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Prefs;
using Loxodon.Framework.Asynchronous;
using Loxodon.Framework.Commands;
using Loxodon.Framework.ViewModels;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Observables;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Examples;
using ms;
using Random = UnityEngine.Random;
using UnityEngine;

namespace ms_Unity
{
	public class InventoryListViewViewModel : ViewModelBase
	{
		private readonly ObservableList<InventoryListItemViewModel> items = new ObservableList<InventoryListItemViewModel> ();
		private EnumMapNew<InventoryType.Id, ObservableSortedDictionary<short, ms.Inventory.Slot>> inventories;

		public InventoryListViewViewModel ()
		{
			var inventory = Stage.get ().get_player ().get_inventory ();
			inventories = inventory.get_all_data ();
			foreach (var keyValuePair in inventories)
			{
				var key = keyValuePair.Key;
				foreach (var item in keyValuePair.Value)
				{
					
					Debug.Log ($"type:{key} name:{ItemData.get (item.Value.Item_id)?.get_name ()}");
				}
			}
		}

		public ObservableList<InventoryListItemViewModel> Items
		{
			get { return this.items; }
		}

		public InventoryListItemViewModel SelectedItem
		{
			get
			{
				foreach (var item in items)
				{
					if (item.IsSelected)
						return item;
				}
				return null;
			}
		}

		public EnumMapNew<InventoryType.Id, ObservableSortedDictionary<short, Inventory.Slot>> Inventories { get => inventories;  }

		public void AddItem ()
		{
			int i = this.items.Count;
			int iconIndex = Random.Range (1, 30);
			this.items.Add (new InventoryListItemViewModel () { Title = "Equip " + i, Icon = string.Format ("EquipImages_{0}", iconIndex), Price = Random.Range (10f, 100f) });
		}

		public void RemoveItem ()
		{
			if (this.items.Count <= 0)
				return;

			int index = Random.Range (0, this.items.Count - 1);
			this.items.RemoveAt (index);
		}

		public void ClearItem ()
		{
			if (this.items.Count <= 0)
				return;

			this.items.Clear ();
		}

		public void ChangeItemIcon ()
		{
			if (this.items.Count <= 0)
				return;

			foreach (var item in this.items)
			{
				int iconIndex = Random.Range (1, 30);
				item.Icon = string.Format ("EquipImages_{0}", iconIndex);
			}
		}

		public void Select (int index)
		{
			if (index <= -1 || index > this.items.Count - 1)
				return;

			for (int i = 0; i < this.items.Count; i++)
			{
				if (i == index)
				{
					items[i].IsSelected = !items[i].IsSelected;
					if (items[i].IsSelected)
						Debug.LogFormat ("Select, Current Index:{0}", index);
					else
						Debug.LogFormat ("Cancel");
				}
				else
				{
					items[i].IsSelected = false;
				}
			}
		}
	}
}