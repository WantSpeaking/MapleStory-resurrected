
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
    public class InventoryListItemViewModel : ViewModelBase
    {
        private string title;
        private string icon;
        private float price;
        private bool selected;

        public string Title
        {
            get { return this.title; }
            set { this.Set<string> (ref title, value, "Title"); }
        }
        public string Icon
        {
            get { return this.icon; }
            set { this.Set<string> (ref icon, value, "Icon"); }
        }

        public float Price
        {
            get { return this.price; }
            set { this.Set<float> (ref price, value, "Price"); }
        }

        public bool IsSelected
        {
            get { return this.selected; }
            set { this.Set<bool> (ref selected, value, "IsSelected"); }
        }
    }
}