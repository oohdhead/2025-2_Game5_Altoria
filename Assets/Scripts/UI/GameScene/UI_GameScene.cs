using Common;
using GameItem;
using System.Collections.Generic;
using TMPro;
using Unity.AppUI.Core;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

namespace GameUI
{
    public class UI_GameScene : UIHUD
    {
        [SerializeField] UIWidget staminaBar;

        public void OnClickMenu()
        {
            Manager.UI.ShowPopup<MainMenuPopUp>();
        }
    }
}
