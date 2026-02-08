using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade Catalog")]
    public List<UpgradeDefinitionSO> upgrades = new();

    // id -> level
    Dictionary<string, int> levels = new();

    public event Action OnUpgradesChanged;

    void Awake()
    {
        // init levels
        foreach (var up in upgrades)
        {
            if (up == null || string.IsNullOrWhiteSpace(up.id)) continue;
            if (!levels.ContainsKey(up.id)) levels.Add(up.id, 0);
        }
    }

    public int GetLevel(UpgradeDefinitionSO up)
        => (up != null && levels.TryGetValue(up.id, out int lvl)) ? lvl : 0;

    public bool IsMaxed(UpgradeDefinitionSO up)
        => GetLevel(up) >= up.maxLevel;

    public int GetCost(UpgradeDefinitionSO up)
    {
        int lvl = GetLevel(up);
        if (lvl >= up.maxLevel) return int.MaxValue;

        // cost for next level (lvl starts at 0)
        float cost = up.baseCost * Mathf.Pow(up.costGrowth, lvl);
        return Mathf.Max(1, Mathf.RoundToInt(cost));
    }

    public bool TryBuy(UpgradeDefinitionSO up, GameDirector director)
    {
        if (up == null || director == null) return false;
        if (IsMaxed(up)) return false;

        int cost = GetCost(up);
        if (director.Data < cost) return false;

        director.SpendData(cost);
        levels[up.id] = GetLevel(up) + 1;

        OnUpgradesChanged?.Invoke();
        return true;
    }

    // ---- EFFECT QUERIES ----
    // These are the “real-time” values your game reads.

    public float GetRobotSpeedBonus()
        => GetTotal(UpgradeType.RobotSpeed);

    public int GetDataPerRobotBonus()
        => Mathf.RoundToInt(GetTotal(UpgradeType.DataPerRobot));

    public float GetOverloadPerRobotBonus()
        => GetTotal(UpgradeType.OverloadPerRobot);

    public float GetSpawnChargeSecondsMultiplier()
    {
        // valuePerLevel interpreted as % faster charge (e.g. 0.05 = 5% faster each level)
        // Multiplier reduces secondsToCharge.
        float faster = GetTotal(UpgradeType.SpawnChargeSpeed);
        return Mathf.Clamp(1f - faster, 0.2f, 1f); // don’t go below 20% of base
    }

    float GetTotal(UpgradeType type)
    {
        float total = 0f;
        foreach (var up in upgrades)
        {
            if (up == null || up.type != type) continue;
            total += GetLevel(up) * up.valuePerLevel;
        }
        return total;
    }
}
