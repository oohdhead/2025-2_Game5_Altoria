using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameUI
{
    public class UI_LobbyScene : UIHUD
    {
        public void OnClickDataResetButton()
        {
            Manager.UserData.SetAllDefaultUserData();
        }
    }
}

