using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class UIFade : MonoBehaviour
{
    [SerializeField] float fadeTime = 0.2f;
    CanvasGroup group;
    Coroutine routine;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        StartFade(1f);
    }

    public void FadeOut()
    {
        StartFade(0f);
    }

    void StartFade(float target)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeRoutine(target));
    }

    IEnumerator FadeRoutine(float target)
    {
        float start = group.alpha;
        float time = 0f;

        while (time < fadeTime)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(start, target, time / fadeTime);
            yield return null;
        }

        group.alpha = target;

        if (Mathf.Approximately(target, 0f))
            gameObject.SetActive(false);
    }
}
