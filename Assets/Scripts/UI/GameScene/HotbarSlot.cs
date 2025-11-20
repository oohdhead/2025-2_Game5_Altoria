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
        // TODO: Default 상태로 변경
        item.SetActive(false);
        itemGradeImg.sprite = Manager.Resource.Load<Sprite>("DefaultFrame");
    }
}
