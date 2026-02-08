using System.Collections;
using UnityEngine;

public class GameIntroSequence : MonoBehaviour
{
    [Header("References")]
    public Transform alien;
    public Transform alienStartPoint;   // positioned above screen
    public Transform alienFinalPoint;   // can be an empty, OR just use alien's initial scene position
    public GameObject uiRoot;           // parent object of all UI (disable at start)
    public GameObject spawner;

    [Header("Timing")]
    public float preDelay = 0.25f;
    public float alienLowerDuration = 1.2f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public float postDelayBeforeUI = 0.15f;

    [Header("Optional - disable gameplay input until intro ends")]
    public Behaviour[] disableDuringIntro; // e.g., TapSpawner

    void Start()
    {
        StartCoroutine(RunIntro());
    }

    IEnumerator RunIntro()
    {
        // Hide UI
        if (uiRoot) uiRoot.SetActive(false);

        // Disable gameplay scripts during intro
        SetBehavioursEnabled(false);

        // Cache final position
        Vector3 finalPos = alienFinalPoint ? alienFinalPoint.position : alien.position;

        // Move alien to start position (above screen)
        if (alien && alienStartPoint)
            alien.position = alienStartPoint.position;

        yield return new WaitForSeconds(preDelay);

        // Lower alien down
        if (alien)
        {
            Vector3 startPos = alien.position;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / Mathf.Max(0.01f, alienLowerDuration);
                float e = ease.Evaluate(Mathf.Clamp01(t));
                alien.position = Vector3.LerpUnclamped(startPos, finalPos, e);
                yield return null;
            }

            alien.position = finalPos;
        }

        yield return new WaitForSeconds(postDelayBeforeUI);

        // Show UI
        if (uiRoot) uiRoot.SetActive(true);
        if (spawner) spawner.SetActive(true);

        // Enable gameplay
        SetBehavioursEnabled(true);
    }

    void SetBehavioursEnabled(bool enabled)
    {
        if (disableDuringIntro == null) return;
        foreach (var b in disableDuringIntro)
        {
            if (b) b.enabled = enabled;
        }
    }




}
