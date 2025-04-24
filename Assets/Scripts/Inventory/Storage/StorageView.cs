
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Systems.Inventory
{
    public abstract class StorageView : MonoBehaviour
    {
        private static readonly List<StorageView> inventories = new();
        public static IReadOnlyList<StorageView> Inventories => inventories;

        protected virtual void Awake()
        {
            // register
            if (!inventories.Contains(this))
                inventories.Add(this);
        }
        public Slot[] Slots;
        protected static GameObject ghost;

        static bool isDragging;
        static Slot originalSlot;

        protected GameObject root;
        protected GameObject container;

        public event Action<Slot, Slot> OnDrop;
        //public event Action<Slot, Slot> OnCrossDrop;

        void Start()
        {
            foreach (var slot in Slots)
            {
                slot.OnStartDrag += OnPointerDown;
            }
        }

        public abstract IEnumerator InitializeView(ViewModel viewModel);

        static void OnPointerDown(Vector2 postion, Slot slot)
        {
            isDragging = true;

            originalSlot = slot;
            originalSlot.Icon.sprite = originalSlot.Background;
            
            ActiveGhost(postion, originalSlot.BaseSprite);
            //originalSlot.StackLabel.visible = false
            // TODO show stack size on ghost icon
        }
        protected void Update()
        {
            if (!isDragging) return;
            SetGhostIconPosition(MouseHelper.GetMousePosition());
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("moue up");
                OnPointerUp();
            }
        }
        void OnPointerUp()
        {
            if (!isDragging) return;
            

            var closestSlot = GetClosest();
            if (closestSlot != null)
            {
                
                Debug.Log("swap in same");
                OnDrop?.Invoke(originalSlot, closestSlot);
                
            }
            else
            {
                originalSlot.Icon.sprite = originalSlot.BaseSprite;
            }

            isDragging = false;
            originalSlot = null;
            ghost.SetActive(false);
        }
        Slot GetClosest()
        {
            Slot closestSlot = null;
            float min = ghost.GetComponent<SpriteRenderer>().bounds.extents.magnitude;
            float closestDistance = min;

            //Vector2 box = ghost.GetComponent<SpriteRenderer>().bounds.size;
            //Vector2 center = ghost.transform.position;


            //var colliders = Physics2D.OverlapBoxAll(center, box, 0);

            //foreach (var col in colliders)
            //{
            //    Slot slot = col.GetComponent<Slot>();
            //    if (slot != null)
            //    {
            //        float dist = Vector2.Distance(center, slot.transform.position);
            //        if (dist < closestDistance)
            //        {
            //            closestDistance = dist;
            //            closestSlot = slot;
            //        }
            //    }
            //}
            Vector2 center = ghost.transform.position;

            foreach (var inventory in inventories)
            {
                foreach (var slot in inventory.Slots)
                {
                    if (slot != null)
                    {
                        float dist = Vector2.Distance(center, slot.transform.position);
                        if (dist < closestDistance)
                        {
                            closestDistance = dist;
                            closestSlot = slot;
                        }
                    }
                }
            }
            return closestSlot;
        }

        static void ActiveGhost(Vector2 position, Sprite sprite)
        {
            ghost.SetActive(true);
            ghost.transform.position = position;
            var art = ghost.GetComponent<SpriteRenderer>();
            art.sprite = sprite;
            art.sortingOrder = 100;
        }
        static void SetGhostIconPosition(Vector2 position)
        {
            ghost.transform.DOMove(position, 0.1f);
        }

        void OnDestroy()
        {
            foreach (var slot in this.Slots)
            {
                slot.OnStartDrag -= OnPointerDown;
            }
            inventories.Remove(this);
        }
    }
}
