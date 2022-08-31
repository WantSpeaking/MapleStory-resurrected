using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Views;
using UnityEngine;
using UnityEngine.UI;
using FairyGUI;
namespace ms_Unity
{
    public class InventoryListItemView : UIView
    {
        public FGUI_Itemed_ListItem FGUI_View;
        public Text title;
        public TMPro.TMP_Text price;
        public RawImage image;
        public GameObject border;
        public GLoader loader;
        protected override void Start()
        {
            BindingSet<InventoryListItemView, ms.Inventory.Slot> bindingSet = this.CreateBindingSet<InventoryListItemView, ms.Inventory.Slot> ();
			//bindingSet.Bind (this.title).For (v => v.text).To (vm => vm.Title).OneWay ();
			bindingSet.Bind (FGUI_View.GetChild ("icon").asLoader).For (v => v.texture).To (vm => vm.Item_id).WithConversion (typeof(ItemId_NTexture_Converter).Name).OneWay ();
			//bindingSet.Bind (this.price).For (v => v.text).ToExpression (vm => string.Format ("${0:0.00}", vm.Price)).OneWay ();
			//bindingSet.Bind (this.border).For (v => v.activeSelf).To (vm => vm.IsSelected).OneWay ();
			bindingSet.Build();

        }
    }
}
