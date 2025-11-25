using GameUI;
using UnityEngine;

namespace GameInteract
{
    public class BoxInteractComponent : InteractBaseComponent
    {
        Animation anim;

        void Start()
        {
            anim = GetComponent<Animation>();
        }

        public override void Interact(IEntity entity)
        {
            base.Interact(entity);

            OnAnimation();
        }

        void OnAnimation()
        {
            anim.Play();
            EndInteract();

            var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
            popUp.SetData("10080072", 10);
        }
    }
}

