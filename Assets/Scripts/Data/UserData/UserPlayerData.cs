using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using static Define;

[Serializable]
public class CustomData
{
    public int Type;
    public string Id;

    public CustomData(int type, string id)
    {
        this.Type = type;
        this.Id = id;
    }
}

[Serializable]
public class WrapperClassCustomDataList
{
    public List<CustomData> userCustomizingData;
}

[Serializable]
public class PlayerData
{
    public Vector3 PlayerTransform;
    public Quaternion Rotation;
    public float Time;
    public float Stemina;
    public int Gender;

    public PlayerData()
    {
        PlayerTransform = new Vector3(-35.0f, 4.0f, 10.01f);
        Rotation = Quaternion.identity;
        Time = 7f;
        Stemina = 100f;
        Gender = 0;
    }
}

public class UserPlayerData : Security, IUserData
{
    string path = Path.Combine(Application.dataPath, "playerData.json");
    string custom_path = Path.Combine(Application.dataPath, "customData.json");

    PlayerData userPlayerData;
    List<CustomData> userCustomizingData;

    #region player
    public PlayerData GetPlayerData() => userPlayerData;

    public void SetTime(float time) => userPlayerData.Time = time;
    public float GetTime() => userPlayerData.Time;

    public void SetPlayerPosRo(Vector3 position, Quaternion quaternion)
    {
        userPlayerData.PlayerTransform = position;
        userPlayerData.Rotation = quaternion;
    }
    public Vector3 GetPlayerPosition() => userPlayerData.PlayerTransform;
    public Quaternion GetPlayerQuaternion() => userPlayerData.Rotation;

    public void SetDataStemina(float stemina) => userPlayerData.Stemina = stemina;


    public void SetGender(int value) => userPlayerData.Gender = value;
    public int GetGender() => userPlayerData.Gender;
    #endregion

    #region Custom
    public string GetID(CustomizationType type)
    {
        for(int i = 0; i < userCustomizingData.Count; i++)
        {
            if(userCustomizingData[i].Type == (int)type)
                return userCustomizingData[i].Id;
        }
        return null;
    }
    public void SetID(CustomizationType type, string id)
    {
        bool found = false;

        for (int i = 0; i < userCustomizingData.Count; i++)
        {
            if (userCustomizingData[i].Type == (int)type)
            {
                userCustomizingData[i].Id = id;
                found = true;
                break;
            }
        }

        if (!found)
        {
            userCustomizingData.Add(new((int)type, id));
        }

        CustomSaveData();
    }
    #endregion

    #region All Data
    public void SetDefaultData()
    {
        PlayerSetDefaultData();
        CustomSetDefaultData();
    }

    public bool LoadData()
    {
        var pR = PlayerLoadData();
        var cR = CustomLoadData();

        return pR && cR;
    }

    public bool SaveData()
    {
        var pR = PlayerSaveData();
        var cR = CustomSaveData();

        return pR && cR;
    }
    #endregion

    #region Player Data
    public void PlayerSetDefaultData()
    {
        userPlayerData = new();
    }

    public bool PlayerLoadData()
    {
        bool result = false;

        try
        {
            if (!File.Exists(path)) // Create
            {
                PlayerSetDefaultData();
            }
            else // Load
            {
                string loadJson = File.ReadAllText(path);
                userPlayerData = JsonUtility.FromJson<PlayerData>(loadJson);
                //playerData = JsonUtility.FromJson<PlayerData>(Decrypt(loadJson, KEY));
            }

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Load failed ({e.Message})");
        }

        return result;
    }

    public bool PlayerSaveData()
    {
        bool result = false;

        try
        {
            string jsonData = JsonUtility.ToJson(userPlayerData);
            File.WriteAllText(path, jsonData);
            //File.WriteAllText(path, Encrypt(jsonData, KEY));

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Save failed ({e.Message})");
        }

        return result;
    }
    #endregion

    #region Custom Data
    public void CustomSetDefaultData()
    {
        userCustomizingData = new()
        {
            new (0, "1"),
            new (1, "1"),
            new (2, "1"),
            new (3, "1"),
            new (4, "1"),
        };
    }

    public bool CustomLoadData()
    {
        bool result = false;

        try
        {
            if (!File.Exists(custom_path)) // Create
            {
                CustomSetDefaultData();
            }
            else // Load
            {
                string loadJson = File.ReadAllText(custom_path);
                var wrapper = JsonUtility.FromJson<WrapperClassCustomDataList>(loadJson);
                userCustomizingData = wrapper.userCustomizingData;
            }

            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Load failed ({e.Message})");
        }

        return result;
    }

    public bool CustomSaveData()
    {
        bool result = false;

        try
        {
            WrapperClassCustomDataList wrapper = new();
            wrapper.userCustomizingData = userCustomizingData;
            string jsonData = JsonUtility.ToJson(wrapper);
            File.WriteAllText(custom_path, jsonData);
            result = true;
        }
        catch (Exception e)
        {
            Debug.Log($"{GetType()} : Save failed ({e.Message})");
        }

        return result;
    }
    #endregion
}
