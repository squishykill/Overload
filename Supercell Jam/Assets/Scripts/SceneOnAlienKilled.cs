using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOnAlienKilled : MonoBehaviour
{
    [Tooltip("Scene name as listed in Build Settings (File > Build Settings).")]
    public string sceneName = "Ending";

    [Tooltip("Optional delay before switching scenes.")]
    public float delaySeconds = 0f;

    bool triggered;

    public void GoToScene()
    {
        if (triggered) return;
        triggered = true;

        if (delaySeconds <= 0f)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Invoke(nameof(Load), delaySeconds);
        }
    }

    void Load()
    {
        SceneManager.LoadScene(sceneName);
    }
}
