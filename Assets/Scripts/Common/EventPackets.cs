using GameInteract;
using UnityEngine;

[System.Serializable]
public struct CraftingStartedEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public CraftingRecipe Recipe;
    public CraftingStartedEvent(CraftingType type, int slotIndex=0, CraftingRecipe recipe =null)
    {
        Type = type;
        SlotIndex = slotIndex;
        Recipe= recipe;
    }
}
[System.Serializable]
public struct CraftingProgressEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public float Progress;   
    public CraftingProgressEvent(CraftingType type, int slotIndex = 0, float progress = 0f)
    {
        this.Type = type;
        this.SlotIndex = slotIndex;
        this.Progress = progress;
    }
   
}
[System.Serializable]
public struct CraftingCompletedEvent
{
    public CraftingType Type;
    public int SlotIndex;
    public ItemEntry entry;
    public CraftingCompletedEvent(CraftingType type, ItemEntry entry,int  slotIndex= 0)
    {
        this.Type = type;
        this.SlotIndex = slotIndex;
        this.entry = entry;
    }
}

[System.Serializable]
public class CraftingRecipe
{
    public ItemEntry ResultItem;
    public float Time;
    public ItemEntry[] RequiredItems;
}
