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
            // TODO : 버튼 클릭을 통해 PlayeData에 CustomData를 저장한다. 
            Manager.UserData.GetUserData<UserPlayerData>().SetID(type, id);
            OnClickAction?.Invoke();
        }
    }
}