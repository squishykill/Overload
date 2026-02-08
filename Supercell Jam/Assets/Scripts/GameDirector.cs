using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameDirector : MonoBehaviour
{
    [Header("Alien")]
    public Transform alienTarget;
    public float overloadRequired = 1f;

    [Header("Rewards per Robot")]
    public int dataPerRobot = 5;
    public float overloadPerRobot = 10f;

    [Header("UI")]
    public TMP_Text dataText;
    public Slider overloadSlider;

    public UpgradeManagerStatic upgrades; // assign in inspector
    public int baseDataPerRobot = 5;
    public float baseOverloadPerRobot = 10f;

    public int Data { get; private set; }
    public float Overload { get; private set; }

    public event Action<int> OnDataChanged;

    public GameObject explosion;
    public FadeAlienAndSwitchScene fadeAndSwitch;


    void Start()
    {
        RefreshUI();
    }

    public void SpendData(int amount)
    {
        SetData(Data - amount);
    }
    public void OnRobotSacrificed()
    {
        int dataGain = baseDataPerRobot + (upgrades ? upgrades.DataBonus() : 0);
        float overloadGain = baseOverloadPerRobot + (upgrades ? upgrades.OverloadBonus() : 0f);

        SetData(Data + dataGain);
        Overload += overloadGain;

        if (Overload >= overloadRequired) KillAlien();
        RefreshUI();
    }

    void KillAlien()
    {
        Overload = overloadRequired;
        RefreshUI();

        explosion.SetActive(true);
 
        if (fadeAndSwitch)
            fadeAndSwitch.Play();
    }

    void RefreshUI()
    {
        if (dataText)
            dataText.text = $"{Data}";

        if (overloadSlider)
        {
            float percent = (Overload / overloadRequired) * 100f;
            overloadSlider.value = Mathf.Clamp(percent, 0f, 100f);
        }
    }

    void SetData(int newValue)
    {
        newValue = Mathf.Max(0, newValue);
        if (Data == newValue) return;

        Data = newValue;
        OnDataChanged?.Invoke(Data);
        RefreshUI();
    }
}
