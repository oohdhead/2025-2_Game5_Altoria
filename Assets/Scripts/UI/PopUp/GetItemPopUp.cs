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
        [SerializeField] TextMeshProUGUI etcTxt;

        string itemID;
        int itemCount;

        public void SetData(string id, int count)
        {
            itemID = id;
            itemCount = count;
            itemName.text = $"'{GameDB.GetItemData(id).Name}' 획득";
            GameSystem.Inventory.AddItem(itemID, itemCount);

            itemSlot.SetSlot(id, count);
        }

        public void SetEtcText(string text)
        {
            etcTxt.text = text;
            etcTxt.gameObject.SetActive(true);
        }
    }
}