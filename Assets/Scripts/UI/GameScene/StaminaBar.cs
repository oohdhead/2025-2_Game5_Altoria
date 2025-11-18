using GameUI;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : UIWidget
{
    [SerializeField] Image staminaBG;
    [SerializeField] Image stamina;
    void Start()
    {
        SetStamina();     //실시간 갱신 필요
    }
    public void SetStamina()
    {
        float value = Manager.UserData.GetUserData<UserPlayerData>().GetPlayerData().Stemina;
        //Debug.Log("Stamina Value: " + value);
        stamina.fillAmount = value;
    }
}