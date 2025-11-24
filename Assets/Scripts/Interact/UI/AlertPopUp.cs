using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameUI;

namespace GameUI
{
    public class AlertPopUp : UIPopUp
    {
        [SerializeField] TMP_Text messageText;
        public void SetMessage(string message) => messageText.text = message;

    }
}