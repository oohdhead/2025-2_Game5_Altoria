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
        [SerializeField] GameObject interaction;
        [SerializeField] TextMeshProUGUI interactText;

        public void OnClickMenu()
        {
            Manager.UI.ShowPopup<MainMenuPopUp>();
        }

        // 상호작용 문구 보이기
        public void ShowInteractText()
        {
            interaction.SetActive(true);
            // interactText.text = "";  //상호작용 종류
        }

        public void HideInteractText()
        {
            interaction.SetActive(false);
        }
    }
}
