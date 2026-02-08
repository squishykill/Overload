using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeAlienAndSwitchScene : MonoBehaviour
{
    [Header("Alien")]
    public SpriteRenderer alienSprite;

    [Header("Scene")]
    public string sceneName = "WinScene";
    public float fadeDuration = 0.5f;

    bool triggered;

    public void Play()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        if (!alienSprite)
        {
            SceneManager.LoadScene(sceneName);
            yield break;
        }

        Color start = alienSprite.color;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, fadeDuration);
            Color c = start;
            c.a = Mathf.Lerp(1f, 0f, t);
            alienSprite.color = c;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}

