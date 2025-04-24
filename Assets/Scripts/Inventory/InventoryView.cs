
using System.Collections;
using UnityEngine;


namespace Systems.Inventory
{
    public class InventoryView : StorageView
    {
        [SerializeField] string panelName = "Inventory";
        [SerializeField] Sprite _slotBackground;
        [SerializeField] Sprite _frameBackground;
        public override IEnumerator InitializeView(ViewModel viewModel)
        {
            if (ghost == null)
            {
                ghost = new GameObject("Ghost", typeof(SpriteRenderer));
            }
            Slots = new Slot[viewModel.Capacity];
            
            for (int i = 0; i < viewModel.Capacity; i++)
            {
                var slot = Slot.CreateSlot(transform, _slotBackground);
                
                Slots[i] = slot;
            }

            //var coins = inventory.CreateChild("coins");
            //var coinsLabel = new Label();
            //coins.CreateChild("coinsIcon");
            //coins.Add(coinsLabel);
            //coins.dataSource = viewModel.Coins;

            //coinsLabel.SetBinding(nameof(Label.text), new DataBinding
            //{
            //    dataSourcePath = new PropertyPath(nameof(BindableProperty<string>.Value)),
            //    bindingMode = BindingMode.ToTarget
            //});
            yield return null;
        }
    }
}
