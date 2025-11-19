using Common;
using GameData;
using System;
using TMPro;
using UnityEngine;

namespace GameUI
{
    public class GetItemPopUp : UIPopUp
    {
        [SerializeField] TextMeshProUGUI itemName;
        [SerializeField] ItemSlot itemSlot;

        string itemID;
        int itemCount;

        public void SetData(string id, int count)
        {
            itemID = id;
            itemCount = count;
            itemName.text = GameDB.GetItemData(id).Name;

            itemSlot.SetSlot(id, count);
        }

        public void OnClickOKButton() => GameSystem.Inventory.AddItem(itemID, itemCount);
    }
}