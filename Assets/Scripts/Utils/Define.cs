using UnityEngine;

public class Define
{
    public enum SceneType
    {
        None,
        Lobby,
        GameScene,
        TestFC_1,
        TestMap,
    };
    public enum ItemType
    {
        None,
        Weapon,
        Tool,
        Consume,
        Material,
        Additive,// ��Ÿ 

    }
    public enum ItemGrade
    {
        None,
        Normal,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    public enum ContentType
    {
        None,
        Farm,
        Fish,
        Fell,
        Animal,
        Mining,
        Plant,
        Upgrade,
        Craft,
    }
    public enum AreaType
    {
        None,
        A, 
        B, 
        C, 
        D, 
        E,
        COUNT
    }
    public enum PlayerState
    {
        Idle=0,
        Move=1<<0,
        Jump=1<<1,
        Interacting=1<<2,
        Riding=1<<3,
        Attack=1<<4,
        Run=1<<5,
        Die=1<<6,

    }
    public enum CustomizationType
    {
        eyebrows,
        eyes,
        mouth,
        facialHair_,
        hair_,
        COUNT,
    }
}
