using UnityEngine;

namespace Systems.Inventory

{
    public class Item
    {
        [field: SerializeField] public SerializableGuid Id;
        [field: SerializeField] public SerializableGuid detailsId;
        //[field: SerializeField] public SerializableGuid invId;
        public ItemDetails details;
        public int quantity;

        public Item(ItemDetails details, int quantity = 1)
        {
            Id = SerializableGuid.NewGuid();
            detailsId = details.Id;
            //this.invId = invID;
            this.details = details;
            this.quantity = quantity;
        }
    }
}