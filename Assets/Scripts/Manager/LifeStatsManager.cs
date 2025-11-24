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
                    var count = GameDB.GetAchieveData(type).Counts[lifeStats[i].Level]; 
                    GameSystem.Inventory.AddItem(reward, count);

                    lifeStats[i].Level++;
                    levelUp = true;
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
                    var count = GameDB.GetAchieveData(typeof(TotalLife).Name).Counts[lifeStats[i].Level];
                    GameSystem.Inventory.AddItem(reward, count);

                    lifeStats[i].Level++;
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
