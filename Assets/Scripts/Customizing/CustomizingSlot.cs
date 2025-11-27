using System;
using TMPro;
using UnityEngine;
using static Define;

namespace GameUI
{
    public class CustomizingSlot : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI buttonTxt;
        CustomizationType type;
        string id;

        public Action OnClickAction;

        public void SlotInit(CustomizationType type, string id, int index)
        {
            this.type = type;
            this.id = id;
            buttonTxt.text = $"¿É¼Ç{index}";
        }

        public void OnClick()
        {
            Manager.UserData.GetUserData<UserPlayerData>().SetID(type, id);
            OnClickAction?.Invoke();
        }
    }
}