using System;
using TMPro;
using UnityEngine;

namespace Systems.Inventory
{
    public class Slot : MonoBehaviour
    {
        public SpriteRenderer Icon => GetComponent<SpriteRenderer>();
        public TextMeshPro StackLabel;

        public int Index => transform.GetSiblingIndex();
        public SerializableGuid ItemId { get; private set; } = SerializableGuid.Empty;
        public Sprite BaseSprite;
        public Sprite Background;

        public event Action<Vector2, Slot> OnStartDrag = delegate { };
        private void OnMouseDown()
        {
            if (ItemId.Equals(SerializableGuid.Empty)) return;
            OnStartDrag.Invoke(MouseHelper.GetMousePosition(), this);
        }
        public void Set(SerializableGuid id, Sprite icon, int qty = 0)
        {
            ItemId = id;
            BaseSprite = icon;
            Icon.sprite = BaseSprite != null ? BaseSprite : Background;
            var col = GetComponent<BoxCollider2D>();
            col.size = Icon.sprite.bounds.size;
            col.offset = Icon.sprite.bounds.center;
            //StackLabel.text = qty > 1 ? qty.ToString() : string.Empty;
            //StackLabel.enabled = qty > 1;
        }
        public void Remove()
        {
            ItemId = SerializableGuid.Empty;
            Icon.sprite = Background;
        }
        public static Slot CreateSlot(Transform parent, Sprite background)
        {
            var slotGO = parent.gameObject.CreateBoxView("Slot");
            var rb = slotGO.AddComponent<Rigidbody2D>();
            var slot = slotGO.AddComponent<Slot>(); 


            var sprite = slotGO.GetComponent<SpriteRenderer>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            slot.Background = background;
            return slot;
        }
    }
}