using System.Collections.Generic;
using UnityEngine;
using GameInteract;
using Unity.VisualScripting;
using GameData;

public class LifeStatsManager
{
    List<LifeStatData> lifeStats;

    readonly Dictionary<string, float> weights = new()
    {
        { nameof(CollectInteractComponent), 0.7f },
        { nameof(UpgradeInteractComponent), 0.3f },
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

        if (totalLevel != GetLevel<T>() && !(levelUp && (totalLevel == GetLevel<T>() - 1)))
            return;

        int addExp = Mathf.RoundToInt(weights[typeof(T).Name] * amount);

        for (int i = 0; i < lifeStats.Count; i++)
        {
            if (lifeStats[i].LifeType == typeof(TotalLife).Name)
            {
                lifeStats[i].Exp += addExp;
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
