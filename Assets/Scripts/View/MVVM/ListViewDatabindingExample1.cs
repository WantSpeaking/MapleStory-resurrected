
using Loxodon.Framework.Binding;
using Loxodon.Framework.Binding.Builder;
using Loxodon.Framework.Binding.Contexts;
using Loxodon.Framework.Binding.Converters;
using Loxodon.Framework.Contexts;
using Loxodon.Framework.Observables;
using Loxodon.Framework.Tutorials;
using Loxodon.Framework.ViewModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ms_Unity
{
   

   

    public class ListViewDatabindingExample : MonoBehaviour
    {
        private InventoryListViewViewModel viewModel;

        public Button addButton;

        public Button removeButton;

        public Button clearButton;

        public Button changeIconButton;

        public InventoryListView listView;

        void Awake()
        {
            ApplicationContext context = Context.GetApplicationContext();
            BindingServiceBundle bindingService = new BindingServiceBundle(context.GetContainer());
            bindingService.Start();

            Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
            foreach (var sprite in Resources.LoadAll<Sprite>("EquipTextures"))
            {
                if (sprite != null)
                    sprites.Add(sprite.name, sprite);
            }
            IConverterRegistry converterRegistry = context.GetContainer().Resolve<IConverterRegistry>();
            converterRegistry.Register("spriteConverter", new SpriteConverter(sprites));
        }

        void Start()
        {
            viewModel = new InventoryListViewViewModel();
            for (int i = 0; i < 3; i++)
            {
                viewModel.AddItem();
            }
            viewModel.Items[0].IsSelected = true;

            IBindingContext bindingContext = this.BindingContext();
            bindingContext.DataContext = viewModel;

            BindingSet<ListViewDatabindingExample, InventoryListViewViewModel> bindingSet = this.CreateBindingSet<ListViewDatabindingExample, InventoryListViewViewModel>();
            bindingSet.Bind(this.listView).For(v => v.Items).To(vm => vm.Items).OneWay();
            bindingSet.Bind(this.listView).For(v => v.OnSelectChanged).To<int>(vm => vm.Select).OneWay();

            bindingSet.Bind(this.addButton).For(v => v.onClick).To(vm => vm.AddItem);
            bindingSet.Bind(this.removeButton).For(v => v.onClick).To(vm => vm.RemoveItem);
            bindingSet.Bind(this.clearButton).For(v => v.onClick).To(vm => vm.ClearItem);
            bindingSet.Bind(this.changeIconButton).For(v => v.onClick).To(vm => vm.ChangeItemIcon);

            bindingSet.Build();
        }
    }
}