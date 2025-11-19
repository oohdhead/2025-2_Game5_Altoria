using Common;
using GameInteract;
using GameItem;
using static Define;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Build.Content;

namespace GameUI
{
    public class Hotbar : UIWidget
    {
        [SerializeField] List<HotbarSlot> slots;

        void Awake()
        {
            for(int i = (int)ContentType.Fish; i <= (int)ContentType.Mining; i++)
            {
                var item = GameSystem.Inventory.GetEquipItem((ContentType)i);
                if(item != null)
                    slots[i - (int)ContentType.Fish].Bind(item.ItemData);
            }
        }

        void OnEnable() => Subscribe();
        void OnDisable() => Unsubscribe();

        void Subscribe()
        {
            GameSystem.Inventory.OnItemEquipped += HandleEquipped;
            GameSystem.Inventory.OnItemUnequipped += HandleUnequipped;
        }
        void Unsubscribe()
        {
            GameSystem.Inventory.OnItemEquipped -= HandleEquipped;
            GameSystem.Inventory.OnItemUnequipped -= HandleUnequipped;
        }

        void HandleEquipped(EquipItem item)
        {
            var index = FindSlotIndexByItem(item.ItemData.Content);

            Debug.Log($"Hotbar: HandleEquipped - Item {item.ItemData.ID} equipped to slot {index}");

            if (index >= 0)
                slots[index].Bind(item.ItemData);
        }
        void HandleUnequipped(EquipItem item)
        {
            var index = FindSlotIndexByItem(item.ItemData.Content);

            if (index >= 0) 
                slots[index].Clear();
        }

        public void LevelChanged(string itemID, int newCount)
        {
            //int idx = FindSlotIndexByItem(itemID);
            //if (idx < 0) return;

            //if (newCount == -1)
            //    slots[idx].Clear();
            //else
            //{
            //    // TODO: 강화 단계만 변경
            //}
        }

        int FindSlotIndexByItem(ContentType type)
        {
            for (int i = (int)ContentType.Fish; i <= (int)ContentType.Mining; i++)
            {
                if ((ContentType)i == type)
                    return i - (int)ContentType.Fish;
            }
            return -1;
        }
    }
}
