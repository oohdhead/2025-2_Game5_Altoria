using Common;
using GameUI;
using SceneLoad;
using SceneLoade;
using UnityEngine;
using UnityEngine.AI;

public class GameScene : BaseScene
{
    protected int task = 15;

    const string PlayerKey = "Player";
    const string NavMeshKey = "NavMesh/GameScene";

    NavMeshDataInstance navMeshInstance;
    bool isInit;
    protected override void Init()
    {
        if (isInit) return;
        isInit = true;
        base.Init();

        GameSystem.Init();
        sceneType = Define.SceneType.GameScene;
        SceneLoad();
        CreatDayNight();
    }

    void SceneLoad()
    {
        var loadingUI = Manager.UI.ShowPopup<LoadingUI>();


        Manager.Resource.LoadAsync<NavMeshData>(NavMeshKey, data =>
        {
            if (data != null)
            {
                navMeshInstance = NavMesh.AddNavMeshData(data);
                Debug.Log(" NavMesh loaded before map.");
            }
            else
            {
                Debug.LogError("Failed to load NavMeshData Addressable!");
            }


            var loader = new JsonMapLoader($"{GetType()}", task);
            loader.Load();

            loadingUI.StartLoding(loader);
            loadingUI.OnClosed += PlayerLoad;
        });
    }

    void PlayerLoad()
    {
        Manager.Resource.Instantiate(PlayerKey,
            new InstantiateOptions { Position = Manager.UserData.GetUserData<UserPlayerData>().GetPlayerPosition()
            , Rotation = Manager.UserData.GetUserData<UserPlayerData>().GetPlayerQuaternion() },
            obj =>
            {
                Manager.UI.ShowHUD<UI_GameScene>();
            });
    }

    void CreatDayNight()
    {
        var go = new GameObject("DayNight");
        var script = go.AddComponent<DayNightCycle>();
        go.transform.SetParent(this.transform);
        script.InitTime(Manager.UserData.GetUserData<UserPlayerData>().GetTime());
    }

    protected virtual void OnDestroy()
    {
     
        if (navMeshInstance.valid)
        {
            NavMesh.RemoveNavMeshData(navMeshInstance);
            Debug.Log(" NavMesh Unloaded.");
        }
    }
}
