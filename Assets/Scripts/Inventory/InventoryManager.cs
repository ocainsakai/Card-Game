using System.Collections.Generic;

namespace Systems.Inventory.Manager
{
    public class InventoryManager
    {
        private Dictionary<SerializableGuid, InventoryController> _inventories = new();
        public void RegisterInventory(SerializableGuid id, InventoryController inventory)
        {
            if (!_inventories.ContainsKey(id))
                _inventories.Add(id, inventory);
        }

        public void UnregisterInventory(SerializableGuid id)
        {
            _inventories.Remove(id);
        }

        public InventoryController GetInventory(SerializableGuid id)
        {
            return _inventories.TryGetValue(id, out var inv) ? inv : null;
        }

        public bool TransferItem(
            SerializableGuid fromId, int fromIndex,
            SerializableGuid toId, int toIndex)
        {
            var fromInv = GetInventory(fromId);
            var toInv = GetInventory(toId);
            if (fromInv == null || toInv == null) return false;

            //fromInv.TransferTo(toInv, fromIndex, toIndex);
            return true;
        }

        public void ClearAll() => _inventories.Clear();
    }
}