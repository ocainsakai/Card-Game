using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Systems.Inventory
{
    public class ViewModel
    {
        public readonly int Capacity;
        public readonly BindableProperty<string> Coins;
        
        public ViewModel(InventoryModel model, int capacity)
        {
            Capacity = capacity;
            //Coins = BindableProperty<string>.Bind(() => model.Coins.ToString());
        }
    }
    public class InventoryController 
    {
        readonly InventoryView view;
        readonly InventoryModel model;
        readonly int capacity;

        InventoryController(InventoryView view, InventoryModel model, int capacity)
        {
            Debug.Assert(view != null, "View is null");
            Debug.Assert(model != null, "Model is null");
            Debug.Assert(capacity > 0, "Capacity is less than 1");
            this.view = view;
            this.model = model;
            this.capacity = capacity;

            view.StartCoroutine(Initialize());
        }
        IEnumerator Initialize()
        {
            yield return view.InitializeView(new ViewModel(model, capacity));

            //model.AddCoins(10);

            view.OnDrop += HandleDrop;
            //view.OnCrossDrop += HandleCrossDrop;
            model.OnModelChanged += HandleModelChanged;

            
            RefreshView();
        }


        public void Bind(InventoryData data) => model.Bind(data);
        public void AddCoins(int amount) => model.AddCoins(amount);
        void HandleDrop(Slot originalSlot, Slot closestSlot)
        {
            //var src = model.Get(originalSlot.Index);
            //var target = model.Get(closestSlot.Index);
            // Moving to Same Slot or Empty Slot
            if (originalSlot.ItemId == closestSlot.ItemId || closestSlot.ItemId.Equals(SerializableGuid.Empty))
            {
                Swap(originalSlot.Index, closestSlot.Index);
                return;
            }
            // Moving to Non-Empty Slot
            //var sourceItemId = model.Get(originalSlot.Index).details.Id;
            var sourceItemId = originalSlot.ItemId;
            //var targetItemId = model.Get(closestSlot.Index).details.Id;
            var targetItemId = closestSlot.ItemId;

            if (sourceItemId.Equals(targetItemId) && model.Get(closestSlot.Index).details.maxStack > 1)
            {
                model.Combine(originalSlot.Index, closestSlot.Index);
            }
            else
            {
                Swap(originalSlot.Index, closestSlot.Index);
            }

           
            // TODO Cross Inventory drops

            // TODO world drops

            // TODO Hotbar drops
            //RefreshModel();
        }
        void HandleModelChanged(IList<Item> items) => RefreshView();
        void Swap(int src, int target)
        {
            var srcItem = model.Get(src);
            var targetItem = model.Get(target);
            if (srcItem == null || srcItem.Id.Equals(SerializableGuid.Empty))
            {
                view.Slots[target].Set(SerializableGuid.Empty, null);
            }
            else
            {
                view.Slots[target].Set(srcItem.Id, srcItem.details.Icon, srcItem.quantity);
            }
            if (targetItem == null || targetItem.Id.Equals(SerializableGuid.Empty))
            {
                view.Slots[src].Set(SerializableGuid.Empty, null);
            }
            else
            {
                view.Slots[src].Set(targetItem.Id, targetItem.details.Icon, targetItem.quantity);
            }
            //RefreshModel();
            model.Swap(src, target);
        }
        
        void RefreshView()
        {
            for (int i = 0; i < capacity; i++)
            {
                
                var item = model.Get(i);
                if (item == null || item.Id.Equals(SerializableGuid.Empty))
                {
                    view.Slots[i].Set(SerializableGuid.Empty, null);
                }
                else
                {
                    view.Slots[i].Set(item.Id, item.details.Icon, item.quantity);
                }
            }
        }
        void RefreshModel()
        {
            for (int i = 0;i < capacity; i++)
            {
                var target = model.GetIndex(view.Slots[i].ItemId);
                model.Swap(i, target);
            }
        }

        #region Builder

        public class Builder
        {
            InventoryView view;
            IEnumerable<ItemDetails> itemDetails;
            int capacity = 20;

            public Builder(InventoryView view)
            {
                this.view = view;
            }

            public Builder WithStartingItems(IEnumerable<ItemDetails> itemDetails)
            {
                this.itemDetails = itemDetails;
                return this;
            }

            public Builder WithCapacity(int capacity)
            {
                this.capacity = capacity;
                return this;
            }

            public InventoryController Build()
            {
                InventoryModel model = itemDetails != null
                    ? new InventoryModel(itemDetails, capacity)
                    : new InventoryModel(Array.Empty<ItemDetails>(), capacity);

                return new InventoryController(view, model, capacity);
            }
        }

        #endregion Builder
    }
}