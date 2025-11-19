using GameUI;
using UnityEngine;
using UnityEngine.Analytics;

public class CharacterCustomizer : MonoBehaviour
{
    [Header("Set Model")]
    [SerializeField] SkinnedMeshRenderer hed;
    [SerializeField] SkinnedMeshRenderer topBody;
    [SerializeField] SkinnedMeshRenderer bottomBody;
    [SerializeField] SkinnedMeshRenderer[] partsModels;

    void Awake()
    {
        SetModel();
    }

    void SetModel()
    {
        var gender = ((EGender)Manager.UserData.GetUserData<UserPlayerData>().GetGender()).ToString();

        hed.sharedMesh = Manager.Resource.Load<Mesh>($"{gender}[{gender[0]}_Head]");
        topBody.sharedMesh = Manager.Resource.Load<Mesh>($"{gender}[{gender[0]}_TopBody]");
        bottomBody.sharedMesh = Manager.Resource.Load<Mesh>($"{gender}[{gender[0]}_BottomBody]");

        for (int i = 0; i < (int)Define.CustomizationType.COUNT; i++)
        {
            if (gender == "Female" && i == 3)
            {
                partsModels[i].enabled = false;
                return;
            }

            partsModels[i].sharedMesh = Manager.Resource.Load<Mesh>(
                Manager.UserData.GetUserData<UserPlayerData>().GetID((Define.CustomizationType)i));

        }
    }
}
