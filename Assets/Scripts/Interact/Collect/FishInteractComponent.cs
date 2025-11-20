using Common;
using GameData;
using GameUI;
using System.Collections.Generic;
using UnityEngine;
using static Define;

namespace GameInteract
{
    public class FishInteractComponent : InteractBaseComponent
    {
        [SerializeField] AreaType areaType;
        [Header("Setting of WorldUI")]
        const string path = "UI/";

        public override void EnterInteract()
        {
            base.EnterInteract();
        }

        public override void ExitInteract()
        {
            base.ExitInteract();
        }

        public override void Interact(IEntity entity)
        {
            CollectTimer timer = new(2);
            timer.OnFinished += EndCollect;
        }

        protected void EndCollect(ITimer timer)
        {
            
            List<(FishGroup, float)> probList = new();

            var fishDic = GameDB.GetFishData(areaType).Value;
            var data = fishDic[areaType.ToString()];

            for (int i = 0; i < data.FishGroups.Count; i++)
                probList.Add((data.FishGroups[i], data.FishGroups[i].Probability));

            GameSystem.Life.AddExp<CollectInteractComponent>(10);

            var equipData = GameSystem.Inventory.GetEquipItem(Type);
            int bous = 0;
            if (equipData != null)
            {
                bous = GameDB.GetUpgradeData(equipData.Level).Bous;
            }
            var item = GameSystem.Random.Pick(probList, bous);

            var popUp = Manager.UI.ShowPopup<GetItemPopUp>();
            popUp.SetData(item.ID, 1);

            EndInteract();
        }
    }

}