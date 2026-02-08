using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapSpawner : MonoBehaviour
{
    public Camera cam;
    public GameDirector director;
    public RobotUnit robotPrefab;

    [Header("Spawn Zone")]
    BoxCollider2D spawnZone;

    public TMP_Text loadingText;

    [Header("Charge")]
    public float secondsToCharge = 1.0f;   // time to refill from 0 -> full
    public Slider chargeSlider;            // Slider with min 0, max 100

    float charge01 = 1f; // 0..1 (start full)
    public float baseSecondsToCharge = 1f;

    void Awake()
    {
        spawnZone = GetComponent<BoxCollider2D>();
        if (!cam) cam = Camera.main;

        // If slider exists, initialize it
        RefreshChargeUI();
    }

    void Update()
    {
        // Fill charge over time
        float secondsToChargeNow = secondsToCharge;

        if (director && director.upgrades)
            secondsToChargeNow *= director.upgrades.ChargeSecondsMultiplier();

        secondsToChargeNow = Mathf.Max(0.01f, secondsToChargeNow);
        charge01 = Mathf.Clamp01(charge01 + (Time.deltaTime / secondsToChargeNow));
        RefreshChargeUI();

        // Only allow spawn when fully charged
        if (charge01 < 1f) return;

        loadingText.color = Color.black;
        loadingText.text = "READY";

        if (!WasTappedThisFrame(out Vector2 screenPos)) return;

        Vector3 world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
        world.z = 0f;

        // Only allow spawning inside the SpawnZone collider
        if (spawnZone && !spawnZone.OverlapPoint(world)) return;

        SpawnRobot(world);

        // Reset charge after spawning
        charge01 = 0f;
        loadingText.text = "Loading...";
        loadingText.color = Color.white;
        RefreshChargeUI();
    }

    void SpawnRobot(Vector3 position)
    {
        float speed = robotPrefab.moveSpeed;
        if (director && director.upgrades)
            speed += director.upgrades.RobotSpeedBonus();

        var robot = Instantiate(robotPrefab, position, Quaternion.identity);
        robot.Init(director, speed);
    }

    void RefreshChargeUI()
    {
        if (!chargeSlider) return;
        chargeSlider.value = charge01 * 100f; // slider range 0..100
    }

    bool WasTappedThisFrame(out Vector2 screenPos)
    {
        screenPos = default;

        // Mobile
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
            {
                screenPos = t.position;
                return true;
            }
        }

        // Editor/PC
        if (Input.GetMouseButtonDown(0))
        {
            screenPos = Input.mousePosition;
            return true;
        }

        return false;
    }

}
