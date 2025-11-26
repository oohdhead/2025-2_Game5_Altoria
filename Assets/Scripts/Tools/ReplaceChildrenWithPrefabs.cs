#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceChildrenWithPrefabs : EditorWindow
{
    GameObject rootObject;
    bool includeInactive = true;
    Dictionary<string, GameObject> prefabByName;

    [SerializeField] DefaultAsset prefabFolder;


    [MenuItem("Tools/Prefab/Replace Children With Prefabs")]
    public static void ShowWindow()
    {
        var window = GetWindow<ReplaceChildrenWithPrefabs>();
        window.titleContent = new GUIContent("Replace Children With Prefabs");
        window.Show();
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("자식 오브젝트를 동일 이름 프리팹으로 교체", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox(
            "1) Hierarchy에서 부모 오브젝트를 드래그해 넣고\n" +
            "2) Project 뷰에서 프리팹이 들어있는 폴더를 드래그해 넣으세요.\n" +
            "→ 해당 폴더(및 하위 폴더) 내 프리팹들만 사용해서 자식 오브젝트를 교체합니다.",
            MessageType.Info);

        EditorGUILayout.Space();

        rootObject = (GameObject)EditorGUILayout.ObjectField("부모 오브젝트", rootObject, typeof(GameObject), true);
        includeInactive = EditorGUILayout.Toggle("비활성 자식 포함", includeInactive);

        EditorGUILayout.Space();

        prefabFolder = (DefaultAsset)EditorGUILayout.ObjectField(
            "프리팹 폴더",
            prefabFolder,
            typeof(DefaultAsset),
            false
        );

        if (prefabFolder == null)
        {
            EditorGUILayout.HelpBox("프리팹이 들어있는 폴더를 반드시 지정해야 합니다.", MessageType.Warning);
        }

        EditorGUILayout.Space();

        EditorGUI.BeginDisabledGroup(rootObject == null || prefabFolder == null);
        if (GUILayout.Button("자식 오브젝트 프리팹으로 교체"))
        {
            if (EditorUtility.DisplayDialog(
                "자식 오브젝트 교체",
                "모든 자식 오브젝트를 (지정한 폴더 내) 동일 이름의 프리팹으로 교체합니다.\n" +
                "Undo로 되돌릴 수 있지만, 저장 전에는 주의해서 사용하세요.",
                "진행", "취소"))
            {
                ReplaceChildren();
            }
        }
        EditorGUI.EndDisabledGroup();
    }

    void ReplaceChildren()
    {
        if (rootObject == null)
        {
            Debug.LogError("[ReplaceChildrenWithPrefabs] 부모 오브젝트가 설정되지 않았습니다.");
            return;
        }

        if (prefabFolder == null)
        {
            Debug.LogError("[ReplaceChildrenWithPrefabs] 프리팹 폴더가 설정되지 않았습니다.");
            return;
        }

        BuildPrefabDictionaryFromFolder();

        if (prefabByName == null || prefabByName.Count == 0)
        {
            Debug.LogWarning("[ReplaceChildrenWithPrefabs] 지정된 폴더 내에서 프리팹을 찾지 못했습니다.");
            return;
        }

        Undo.RegisterFullObjectHierarchyUndo(rootObject, "Replace Children With Prefabs");

        var children = rootObject.GetComponentsInChildren<Transform>(includeInactive);
        int replaceCount = 0;
        int skipNoPrefab = 0;

        foreach (var t in children)
        {
            if (t == rootObject.transform)
                continue;

            string objName = t.name;

            if (!prefabByName.TryGetValue(objName, out GameObject prefab))
            {
                skipNoPrefab++;
                continue;
            }

            Transform parent = t.parent;
            int siblingIndex = t.GetSiblingIndex();
            Vector3 localPos = t.localPosition;
            Quaternion localRot = t.localRotation;
            Vector3 localScale = t.localScale;
            bool wasActive = t.gameObject.activeSelf;

            Undo.DestroyObjectImmediate(t.gameObject);

            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);

            if (newObj != null)
            {
                Transform nt = newObj.transform;
                nt.SetSiblingIndex(siblingIndex);
                nt.localPosition = localPos;
                nt.localRotation = localRot;
                nt.localScale = localScale;
                newObj.name = objName;
                newObj.SetActive(wasActive);

                Undo.RegisterCreatedObjectUndo(newObj, "Create Replaced Prefab");
                replaceCount++;
            }
        }

        Debug.Log($"[ReplaceChildrenWithPrefabs] 교체 완료 - 교체된 오브젝트: {replaceCount}, " +
                  $"프리팹 미존재로 스킵: {skipNoPrefab}");
    }

    void BuildPrefabDictionaryFromFolder()
    {
        prefabByName = new Dictionary<string, GameObject>();

        string folderPath = AssetDatabase.GetAssetPath(prefabFolder);
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("[ReplaceChildrenWithPrefabs] 폴더 경로를 찾을 수 없습니다.");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            string name = prefab.name;

            if (prefabByName.ContainsKey(name))
            {
                Debug.LogWarning($"[ReplaceChildrenWithPrefabs] 같은 이름의 프리팹이 여러 개 있습니다: {name}\n" +
                                 $"기존 경로: {AssetDatabase.GetAssetPath(prefabByName[name])}, " +
                                 $"무시된 경로: {path}");
                continue;
            }

            prefabByName.Add(name, prefab);
        }

        Debug.Log($"[ReplaceChildrenWithPrefabs] 폴더 '{folderPath}'에서 프리팹 {prefabByName.Count}개 로드됨.");
    }
}
#endif

