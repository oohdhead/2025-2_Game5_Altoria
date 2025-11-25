using Common;
using GameData;
using GameItem;
using GameUI;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameInteract
{
    public class UpgradePopUp : UIPopUp
    {
        [SerializeField] Transform slotRoot;
        [SerializeField] Transform selectItemSlotRoot;
        [SerializeField] Transform meterialSlotRoot;
        [SerializeField] TextMeshProUGUI gradeTxt;

        List<GameObject> rightSlotItems = new List<GameObject>();
        GameObject selectedItemGO;
        GameObject meterialsItem;
        ItemData selectItemData;
        string upgradeMaterialID = "10080072";

        public override bool Init()
        {
            if (base.Init() == false) return false;

            SetItemSlot();

            return true;
        }

        void SetItemSlot()
        {
            if (rightSlotItems.Count != 0)
            {
                for (int i = 0; i < rightSlotItems.Count; i++)
                {
                    Destroy(rightSlotItems[i]);
                }
                rightSlotItems.Clear();
            }

            var equipItemList = GameSystem.Inventory.GetAllItems()
                    .Where(item => item.item is EquipItem)
                    .ToList();

            for (int i = 0; i < equipItemList.Count; i++)
            {
                GameObject upgradeSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSlot));
                var newGO = Instantiate(upgradeSlotPrefab, slotRoot);
                if (newGO.TryGetComponent<UpgradeSlot>(out var slot))
                {
                    slot.Init(equipItemList[i].item.ItemData, i);
                    slot.OnClickAction = (i) => SetUpgradeData(equipItemList[i].item.ItemData);
                }
                rightSlotItems.Add(newGO);
            }
        }

        void SetUpgradeData(ItemData itemData)
        {
            selectItemData = itemData;
            Destroy(meterialsItem);
            Destroy(selectedItemGO);

            GameObject selectdSlotPrefab = Resources.Load<GameObject>(nameof(UpgradeSelectItem));
            if (selectdSlotPrefab.TryGetComponent<UpgradeSelectItem>(out var slot))
            {
                var selectdItem = Common.GameSystem.Inventory.GetItem(itemData.ID);
                if (selectdItem?.item is EquipItem selectEquipItem)
                {
                    slot.Init(itemData);
                    gradeTxt.text = $"{(selectEquipItem.Level)}°­ -> {(selectEquipItem.Level + 1)}°­";

                    GameObject meterialSlotPrefab = Resources.Load<GameObject>(nameof(ItemSlot));
                    var newGO = Instantiate(meterialSlotPrefab, meterialSlotRoot);
                    meterialsItem = newGO;
                    if (newGO.TryGetComponent<ItemSlot>(out var item))
                        item.SetSlot(upgradeMaterialID, GameDB.GetUpgradeData(selectEquipItem.Level).Material);
                }
                else
                {
                    slot.Init(itemData);
                    gradeTxt.text = "";
                }

            }
            selectedItemGO = Instantiate(selectdSlotPrefab, selectItemSlotRoot);
        }

        public void OnClickUpgradeBtn()
        {
            var item = GameSystem.Inventory.GetItem(upgradeMaterialID);
            if(item == null)
            { 
                Manager.UI.ShowPopup<NoItemPopUp>();
                return;
            }

            int curCnt = item.count;
            var eqiupItem = GameSystem.Inventory.GetItem(selectItemData.ID).item as EquipItem;
            Debug.Log($"{GetType()} : {eqiupItem.ItemData.ID} : Level {eqiupItem.Level}");
            int needCnt = GameDB.GetUpgradeData(eqiupItem.Level).Material;

            if(curCnt < needCnt)
                Manager.UI.ShowPopup<NoItemPopUp>();
            else
            {
                var popUp = Manager.UI.ShowPopup<UpgradeResultPopUp>();
                GameSystem.Inventory.RemoveItem(upgradeMaterialID, needCnt);
                GameSystem.Life.AddExp<UpgradeInteractComponent>(10);
                popUp.SetResult(selectItemData);
                popUp.OnClosed += SetItemSlot;
                popUp.OnClosed += () => SetUpgradeData(selectItemData);
            }
        }
    }
}
