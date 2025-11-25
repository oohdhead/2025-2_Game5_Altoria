using Common;
using GameData;
using GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameInteract
{
    public class CollectInteractComponent : InteractBaseComponent
    {
        [SerializeField] string objectID;

        bool interactCollTime = false;

        [Header("Respawn")]
        float returnDuration = 20.0f;
        Vector3 orignScale;

        public override void Interact(IEntity entity)
        {
            base.Interact(entity);
            if (interactCollTime)
                return;

            interactCollTime = true;
            CollectTimer timer = new(2);
            orignScale = transform.localScale;
            timer.OnFinished += EndCollect;
        }

        void EndCollect(ITimer timer)
        {
            GetComponent<Collider>().enabled = false;
            transform.localScale = Vector3.zero;

            StartCoroutine("Respawn");

            List<(CollectGroup, float)> probList = new List<(CollectGroup, float)>();
            var dic = GameDB.GetCollectData(objectID).Value;
            var data = dic[objectID];

            for (int i = 0; i < data.CollectGroup.Count; i++)
                probList.Add((data.CollectGroup[i], data.CollectGroup[i].Probability));

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            var equipData = GameSystem.Inventory.GetEquipItem(Type);
            int bous = 0;
            if (equipData != null)
            {
                bous = GameDB.GetUpgradeData(equipData.Level).Bous;
            }
            var item = GameSystem.Random.Pick(probList, bous);

            var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
            popUp.SetData(objectID, item.Count);

            EndInteract();
        }

        public void SetObjectID(string id)
        {
            objectID = id;
        }

        IEnumerator Respawn()
        {
            float elapsed = 0f;

            while (elapsed < returnDuration)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localScale = orignScale;
            GetComponent<Collider>().enabled = true;
            interactCollTime = false;
        }
    }
}