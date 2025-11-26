using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3(0, 20, 0); 
    Transform player;
    const string playerTag = "Player";

    void Start()
    {
        StartCoroutine(FindPlayerCoroutine());
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = targetPosition;

            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
    }

    System.Collections.IEnumerator FindPlayerCoroutine()
    {
        while (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag(playerTag);
            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
            yield return new WaitForSeconds(0.5f); 
        }
    }
}
