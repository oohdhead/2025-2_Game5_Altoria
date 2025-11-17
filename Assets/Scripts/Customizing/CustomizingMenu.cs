using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

namespace GameUI
{
    public class CustomizingMenu : MonoBehaviour
    {
        [Header("Set UI Slot")]
        [SerializeField] List<Transform> slotRoots;

        [Header("Set Gender")]
        [SerializeField] TMP_Dropdown genderDropDown;
        [SerializeField] GameObject maleModel;
        [SerializeField] GameObject femaleModel;

        [Header("Male Set Root")]
        [SerializeField] Transform maleEyebrowRoot;
        [SerializeField] Transform maleEyeRoot;
        [SerializeField] Transform maleMouthRoot;
        [SerializeField] Transform maleFacehairRoot;
        [SerializeField] Transform maleHairRoot;

        [Header("Female Set Root")]
        [SerializeField] Transform femaleEyebrowRoot;
        [SerializeField] Transform femaleEyeRoot;
        [SerializeField] Transform femaleMouthRoot;
        [SerializeField] Transform femaleHairRoot;

        Dictionary<string, GameObject> maleEyebrowParts = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> maleEyeParts = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> maleMouthParts = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> maleFacehairParts = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> maleHairParts = new Dictionary<string, GameObject>();

        Dictionary<string, GameObject> femaleEyebrowParts = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> femaleEyeParts = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> femaleMouthParts = new Dictionary<string, GameObject>();
        Dictionary<string, GameObject> femaleHairParts = new Dictionary<string, GameObject>();

        List<GameObject> slots = new();
        int gender;

        public void Awake()
        {
            Init();
            gender = 0;
            SetSlot();
        }

        void Init()
        {
            LoadParts(maleEyebrowRoot, maleEyebrowParts);
            LoadParts(maleEyeRoot, maleEyeParts);
            LoadParts(maleMouthRoot, maleMouthParts);
            LoadParts(maleFacehairRoot, maleFacehairParts);
            LoadParts(maleHairRoot, maleHairParts);

            LoadParts(femaleEyebrowRoot, femaleEyebrowParts);
            LoadParts(femaleEyeRoot, femaleEyeParts);
            LoadParts(femaleMouthRoot, femaleMouthParts);
            LoadParts(femaleHairRoot, femaleHairParts);
        }

        void LoadParts(Transform root, Dictionary<string, GameObject> dict)
        {
            foreach (Transform child in root)
            {
                dict.Add(child.name, child.gameObject);
            }
        }

        void SetSlot()
        {
            if (slots.Count != 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    Destroy(slots[i]);
                }
                slots.Clear();
            }

            for (int i = 0; i < (int)CustomizationType.COUNT; i++)
            {
                for (int j = 0; j < GetPartsCount((CustomizationType)i, gender); j++)
                {
                    GameObject CustomizingSlotPrefab = Resources.Load<GameObject>(nameof(CustomizingSlot));
                    var newGO = Instantiate(CustomizingSlotPrefab, slotRoots[i]);
                    if (newGO.TryGetComponent<CustomizingSlot>(out var slot))
                    {
                        var type = (CustomizationType)i;
                        var id = (j + 1).ToString();
                        slot.SlotInit(type, id);
                        if(gender == 0)
                            slot.OnClickAction = () => SelectOptionMale(type, id);
                        else
                            slot.OnClickAction = () => SelectOptionFemale(type, id);
                    }
                    slots.Add(newGO);
                }
            }
        }

        void SelectOptionMale(CustomizationType type, string id)
        {
            switch (type)
            {
                case CustomizationType.EyeBrow:
                    ApplyMaleEyeBrow(id);
                    break;
                case CustomizationType.Eye:
                    ApplyMaleEye(id);
                    break;
                case CustomizationType.Mouth:
                    ApplyMaleMouth(id);
                    break;
                case CustomizationType.FaceHair:
                    ApplyMaleFaceHair(id);
                    break;
                case CustomizationType.Hair:
                    ApplyMaleHair(id);
                    break;
            }
        }

        void SelectOptionFemale(CustomizationType type, string id)
        {
            switch (type)
            {
                case CustomizationType.EyeBrow:
                    ApplyFemaleEyeBrow(id);
                    break;
                case CustomizationType.Eye:
                    ApplyFemaleEye(id);
                    break;
                case CustomizationType.Mouth:
                    ApplyFemaleMouth(id);
                    break;
                case CustomizationType.Hair:
                    ApplyFemaleHair(id);
                    break;
            }
        }

        #region Apply
        void ApplyMaleEyeBrow(string id) => SetActiveOnly(id, maleEyebrowParts);
        void ApplyMaleEye(string id) => SetActiveOnly(id, maleEyeParts);
        void ApplyMaleMouth(string id) => SetActiveOnly(id, maleMouthParts);
        void ApplyMaleFaceHair(string id) => SetActiveOnly(id, maleFacehairParts);
        void ApplyMaleHair(string id) => SetActiveOnly(id, maleHairParts);

        void ApplyFemaleEyeBrow(string id) => SetActiveOnly(id, femaleEyebrowParts);
        void ApplyFemaleEye(string id) => SetActiveOnly(id, femaleEyeParts);
        void ApplyFemaleMouth(string id) => SetActiveOnly(id, femaleMouthParts);
        void ApplyFemaleHair(string id) => SetActiveOnly(id, femaleHairParts);
        #endregion

        void SetActiveOnly(string id, Dictionary<string, GameObject> dict)
        {
            foreach (var kv in dict)
            {
                kv.Value.SetActive(kv.Key == id);
            }
        }

        int GetPartsCount(CustomizationType type, int gender)
        {
            if (gender == 0)
            {
                switch (type)
                {
                    case CustomizationType.EyeBrow:
                        return maleEyebrowParts.Count;
                    case CustomizationType.Eye:
                        return maleEyeParts.Count;
                    case CustomizationType.Mouth:
                        return maleMouthParts.Count;
                    case CustomizationType.FaceHair:
                        return maleFacehairParts.Count;
                    case CustomizationType.Hair:
                        return maleHairParts.Count;
                    default:
                        return -1;
                }
            }
            else
            {
                switch (type)
                {
                    case CustomizationType.EyeBrow:
                        return femaleEyebrowParts.Count;
                    case CustomizationType.Eye:
                        return femaleEyeParts.Count;
                    case CustomizationType.Mouth:
                        return femaleMouthParts.Count;
                    case CustomizationType.Hair:
                        return femaleHairParts.Count;
                    default:
                        return -1;
                }
            }
        }

        public void ChangedGender()
        {
            gender = genderDropDown.value;
            maleModel.SetActive(gender == 0);
            femaleModel.SetActive(gender == 1);
            Manager.UserData.GetUserData<UserPlayerData>().SetGender(gender);

            SetSlot();
        }
    }
}