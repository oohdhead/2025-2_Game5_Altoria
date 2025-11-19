using System;
using UnityEngine;
using static Define;

namespace GameUI
{
    public class CustomizingSlot : MonoBehaviour
    {
        CustomizationType type;
        string id;

        public Action OnClickAction;

        public void SlotInit(CustomizationType type, string id)
        {
            this.type = type;
            this.id = id;
        }

        public void OnClick()
        {
            Manager.UserData.GetUserData<UserPlayerData>().SetID(type, id);
            OnClickAction?.Invoke();
        }
    }
}