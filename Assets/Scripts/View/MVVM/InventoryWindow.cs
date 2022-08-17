

using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Interactivity;
using Loxodon.Framework.Views;
using Loxodon.Log;
using UnityEngine.UI;
using InputField = UnityEngine.UI.InputField;
using Text = UnityEngine.UI.Text;
using Button = UnityEngine.UI.Button;
using TMPro;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Localizations;
using Loxodon.Framework.Examples;
using Loxodon.Framework.Binding.Converters;
using FairyGUI;

namespace ms_Unity
{
    public partial class InventoryWindow : Loxodon.Framework.Views.Window
    {
        public FGUI_Inventory FGUI_View;

        private InventoryListViewViewModel viewModel;

        public Button addButton;

        public Button removeButton;

        public Button clearButton;

        public Button changeIconButton;

        public InventoryListView listView;

        
		protected override void OnCreate (IBundle bundle)
		{

        }

		protected override void Start ()
		{
            /* ApplicationContext context = Context.GetApplicationContext ();
 BindingServiceBundle bindingService = new BindingServiceBundle (context.GetContainer ());
 bindingService.Start ();*/

            /*Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite> ();
            foreach (var sprite in Resources.LoadAll<Sprite> ("EquipTextures"))
            {
                if (sprite != null)
                    sprites.Add (sprite.name, sprite);
            }
            */
            FGUI_View = (FGUI_Inventory)UIPackage.CreateObject (FGUI_Manager.Instance.packageName, "Inventory");
            GRoot.inst.AddChild (FGUI_View);
            FGUI_View.MonoBehaviour_View = this;

            /*var mono_inventoryListView = this.FGUI_View._InventoryListView.displayObject.gameObject.AddComponent<InventoryListView> ();
            var fgui_uI_Inventory = this.FGUI_View._InventoryListView;
            mono_inventoryListView.FGUI_View = fgui_uI_Inventory;
            fgui_uI_Inventory.MonoBehaviour_View = mono_inventoryListView;*/

            viewModel = new InventoryListViewViewModel ();
            /* for (int i = 0; i < 3; i++)
             {
                 viewModel.AddItem ();
             }
             viewModel.Items[0].IsSelected = true;*/

            /*var bindingContext = this.BindingContext ();
            bindingContext.DataContext = viewModel;*/

            var bindingSet = this.CreateBindingSet (viewModel);
           // bindingSet.Bind (this.FGUI_View._InventoryListView.MonoBehaviour_View).For (v => v.Inventories).To (vm => vm.Inventories).OneWay ();
            /*bindingSet.Bind (this.listView).For (v => v.OnSelectChanged).To<int> (vm => vm.Select).OneWay ();
            bindingSet.Bind (this.addButton).For (v => v.onClick).To (vm => vm.AddItem);
            bindingSet.Bind (this.removeButton).For (v => v.onClick).To (vm => vm.RemoveItem);
            bindingSet.Bind (this.clearButton).For (v => v.onClick).To (vm => vm.ClearItem);
            bindingSet.Bind (this.changeIconButton).For (v => v.onClick).To (vm => vm.ChangeItemIcon);*/

            bindingSet.Build ();



        }
	}
}