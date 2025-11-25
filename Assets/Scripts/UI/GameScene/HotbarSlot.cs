using GameInteract;
using UnityEngine;

public class HotbarSlot : UpgradeSelectItem
{
    public ItemData ItemData { private set; get; }

    [SerializeField] GameObject item;

    public void Bind(ItemData itemData)
    {
        ItemData = itemData;
        Init(itemData);
        item.SetActive(true);
    }

    public void Clear()
    {
        item.SetActive(false);
        itemGradeImg.sprite = Manager.Resource.Load<Sprite>("DefaultFrame");
    }
}
