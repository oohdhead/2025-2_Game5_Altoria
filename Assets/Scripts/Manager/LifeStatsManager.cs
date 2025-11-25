using Common;
using GameData;
using GameInteract;
using GameUI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LifeStatsManager
{
    List<LifeStatData> lifeStats;
    const string reward = "10080072";

    readonly Dictionary<string, float> weights = new()
    {
        { nameof(CollectInteractComponent), 0.4f },
        { nameof(UpgradeInteractComponent), 0.3f },
        { nameof(CraftInteractComponent), 0.3f },
    };

    public LifeStatsManager()
    {
        lifeStats = Manager.UserData.GetUserData<UserLifeData>().GetUserLifeData();
        if (lifeStats == null)
        {
            lifeStats = new()
            {
                new(nameof(CollectInteractComponent), 0, 0),
                new(nameof(UpgradeInteractComponent), 0, 0),
                new(nameof(CraftInteractComponent), 0, 0),
                new(nameof(TotalLife), 0, 0),
            };
        }
    }

    public void AddExp<T>(int amount)
    {
        var type = typeof(T).Name;
        bool levelUp = false;

        for (int i = 0; i < lifeStats.Count; i++)
        {
            if (lifeStats[i].LifeType == type)
            {
                if (lifeStats[i].Level == 5) return;

                lifeStats[i].Exp += amount;

                if (lifeStats[i].Exp >= GameDB.GetLifeExpData(lifeStats[i].Level))
                {
                    lifeStats[i].Level++;
                    levelUp = true;

                    var listLevelCount = GameDB.GetAchieveData(type).Counts;

                    int count = 0;
                    for(int index = 0; index < 5; index++)
                    {
                        if (listLevelCount[index].Level == lifeStats[i].Level)
                        {
                            count = listLevelCount[index].Count;
                        }
                    }

                    Manager.UserData.GetUserData<UserPlayerData>().SetFirstGift();
                    var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
                    popUp.SetData("10080072", count);
                    switch (type)
                    {
                        case nameof(CollectInteractComponent):
                            popUp.SetEtcText($"채집 숙련도 {lifeStats[i].Level}레벨 달성 보상");
                            break;
                        case nameof(UpgradeInteractComponent):
                            popUp.SetEtcText($"강화 숙련도 {lifeStats[i].Level}레벨 달성 보상");
                            break;
                        case nameof(CraftInteractComponent):
                            popUp.SetEtcText($"제작 숙련도 {lifeStats[i].Level}레벨 달성 보상");
                            break;
                    }
                }

                break;
            }
        }

        SetTotalStat<T>(amount, levelUp);
    }

    public int GetLevel<T>()
    {
        var type = typeof(T).Name;


        for (int i = 0; i < lifeStats.Count; i++)
        {
            if (lifeStats[i].LifeType == type)
            {
                return lifeStats[i].Level;
            }
        }

        return -1;
    }

    public int GetEXP<T>()
    {
        var type = typeof(T).Name;

        for (int i = 0; i < lifeStats.Count; i++)
        {
            if (lifeStats[i].LifeType == type)
            {
                return lifeStats[i].Exp;
            }
        }

        return -1;
    }

    void SetTotalStat<T>(int amount, bool levelUp)
    {
        int totalLevel = GetLevel<TotalLife>();

        int addExp = Mathf.RoundToInt(weights[typeof(T).Name] * amount);

        for (int i = 0; i < lifeStats.Count; i++)
        {
            if (lifeStats[i].LifeType == typeof(TotalLife).Name)
            {
                lifeStats[i].Exp += addExp;

                if (lifeStats[i].Exp >= GameDB.GetLifeExpData(lifeStats[i].Level))
                {
                    lifeStats[i].Level++;

                    var listLevelCount = GameDB.GetAchieveData(typeof(TotalLife).Name).Counts;

                    int count = 0;
                    for (int index = 0; index < 5; index++)
                    {
                        if (listLevelCount[index].Level == lifeStats[i].Level)
                        {
                            count = listLevelCount[index].Count;
                        }
                    }

                    Manager.UserData.GetUserData<UserPlayerData>().SetFirstGift();
                    var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
                    popUp.SetData("10080072", count);
                    popUp.SetEtcText($"생활력 {lifeStats[i].Level}레벨 달성 보상");
                }
            }
        }

        Save();
    }

    void Save()
    {
        Manager.UserData.GetUserData<UserLifeData>().SetSaveDataAndSave(lifeStats);
        Manager.UserData.GetUserData<UserLifeData>().SaveData();
    }
}
