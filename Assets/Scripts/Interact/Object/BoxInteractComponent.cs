using GameUI;
using System.Collections;
using UnityEngine;

namespace GameInteract
{
    public class BoxInteractComponent : InteractBaseComponent
    {
        Animation anim;

        void Start()
        {
            anim = GetComponent<Animation>();

            if(Manager.UserData.GetUserData<UserPlayerData>().GetRedTreasure())
                GetComponent<Collider>().enabled = false;
        }

        public override void Interact(IEntity entity)
        {
            base.Interact(entity);

            StartCoroutine(Open());
            Manager.UserData.GetUserData<UserPlayerData>().SetRedTreasure();
        }

        IEnumerator Open()
        {
            anim.Play();

            yield return new WaitForSeconds(anim.clip.length);

            var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
            popUp.SetData("10080072", 10);
            EndInteract();
            GetComponent<Collider>().enabled = false;
        }
    }
}

