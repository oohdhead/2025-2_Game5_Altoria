using Common;
using GameItem;
using System.Collections.Generic;
using TMPro;
using Unity.AppUI.Core;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.ShaderKeywordFilter;
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

        PlayerInteractComponent playerInteract;
        public override bool Init()
        {
            if (!base.Init()) return false;

            InitializePrompt();
            return true;
        }

        public void OnClickMenu()
        {
            Manager.UI.ShowPopup<MainMenuPopUp>();
        }
        void InitializePrompt()
        {
            SetPromptActive(false);
            playerInteract = GameObject.FindAnyObjectByType<PlayerInteractComponent>();

            if (playerInteract == null) return;

            playerInteract.OnInteractableNearby += SetPromptActive;
            SetPromptActive(playerInteract.HasNearbyInteractable);
        }

        // 상호작용 문구 보이기
        void SetPromptActive(bool active)
        {
            if (interaction != null)
                interaction.SetActive(active);
            //interactText.text = "";  상호작용 종류 연결 
        }
    }
}
