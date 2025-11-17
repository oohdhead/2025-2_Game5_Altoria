using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CharacterCustomizer : MonoBehaviour
{
    [Header("Set Root")]
    [SerializeField] Transform eyebrowRoot;
    [SerializeField] Transform eyeRoot;
    [SerializeField] Transform mouthRoot;
    [SerializeField] Transform facehairRoot;
    [SerializeField] Transform hairRoot;

    void Awake()
    {
        LoadParts(eyebrowRoot);
        LoadParts(eyeRoot);
        LoadParts(mouthRoot);
        LoadParts(facehairRoot);
        LoadParts(hairRoot);
    }

    void LoadParts(Transform root)
    {
        for (int i = 0; i < (int)Define.CustomizationType.COUNT; i++)
        {
            foreach (Transform child in root)
            {
                // TODO: 플레이어 데이터에 저장된 커스터마이징 정보로 파츠들 활성화하기
                // TODO: 이전에 플레이어 모델 프리펩의 모든 파츠들 비활성화 하기
                if (child.name == Manager.UserData.GetUserData<UserPlayerData>().GetID((CustomizationType)i))
                    child.gameObject.SetActive(true);
            }
        }
    }
}
