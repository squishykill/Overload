using UnityEngine;

public enum UpgradeType
{
    RobotSpeed,
    DataPerRobot,
    OverloadPerRobot,
    SpawnChargeSpeed // reduces secondsToCharge
}

[CreateAssetMenu(menuName = "Game/Upgrade Definition")]
public class UpgradeDefinitionSO : ScriptableObject
{
    public string id;                 // unique key, e.g. "robot_speed"
    public string displayName;        // "Robot Speed"
    [TextArea] public string description;

    public UpgradeType type;

    [Header("Progression")]
    public int maxLevel = 25;

    [Tooltip("Cost at level 1")]
    public int baseCost = 10;

    [Tooltip("Higher = steeper cost curve")]
    public float costGrowth = 1.25f;

    [Header("Effect per level")]
    public float valuePerLevel = 0.2f; // meaning depends on type
}
