using UnityEngine;
using UnityEngine.UI;
using GameUI;
using TMPro;
using GameInteract;
using Common;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class Stat
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI statText;
    public Slider slider;
}

public class MainMenuPopUp : UIPopUp
{
    [SerializeField] Stat[] stats = new Stat[3];

    readonly System.Type[] lifeTypes =
    {
        typeof(TotalLife),
        typeof(CollectInteractComponent),
        typeof(UpgradeInteractComponent)
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        UpdateStats();
    }

    private void OnEnable()
    {
        UpdateStats();
    }

    public override bool Init()
    {
        return base.Init();
    }

    void UpdateStats()
    {
        for (int i = 0; i < stats.Length; i++)
        {
            UpdateStatByIndex(i);
        }
    }

    void UpdateStatByIndex(int index)
    {
        var stat = stats[index];
        var type = lifeTypes[index];

        // 레벨
        stat.levelText.text = "LV. " + GetLevel(type);

        // 경험치
        int exp = GetEXP(type);

        stat.slider.minValue = 0;
        stat.slider.maxValue = 250;
        stat.slider.value = exp;

        stat.statText.text = $"{exp} / {stat.slider.maxValue}";
    }
    int GetLevel(System.Type t)
    {
        if (t == typeof(TotalLife))
            return GameSystem.Life.GetLevel<TotalLife>();
        if (t == typeof(CollectInteractComponent))
            return GameSystem.Life.GetLevel<CollectInteractComponent>();
        if (t == typeof(UpgradeInteractComponent))
            return GameSystem.Life.GetLevel<UpgradeInteractComponent>();

        return 1;
    }

    int GetEXP(System.Type t)
    {
        if (t == typeof(TotalLife))
            return GameSystem.Life.GetEXP<TotalLife>();
        if (t == typeof(CollectInteractComponent))
            return GameSystem.Life.GetEXP<CollectInteractComponent>();
        if (t == typeof(UpgradeInteractComponent))
            return GameSystem.Life.GetEXP<UpgradeInteractComponent>();

        return 0;
    }

    public void OnClickInventory()
    {
        Manager.UI.ShowPopup<InventoryUI>();
    }
    public void OnClickCraft()
    {
        Manager.UI.ShowPopup<CraftPopUp>();
    }

    public void OnClickUpgrade()
    {
        Manager.UI.ShowPopup<UpgradePopUp>();
    }

    public void OnClickSetting()
    {
        Manager.UI.ShowPopup<SettingPopUp>();
        Debug.Log("[MainMenuPopUp] : 설정창");
    }
    public void OnClickMain()
    {
        Manager.UI.ShowPopup<ExitPopUp>().SetPopUpType(ExitPopUpType.GoToMainMenu);
        Debug.Log("[MainMenuPopUp] : 메인메뉴로");
    }

    public void OnClickExit()
    {
        Manager.UI.ShowPopup<ExitPopUp>().SetPopUpType(ExitPopUpType.ExitGame);
        Debug.Log("[MainMenuPopUp] : 종료창");
    }

    public void ClosePopUp()
    {
        Manager.UI.ClosePopup();
    }
}
