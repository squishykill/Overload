using System;
using UnityEngine;

public class UpgradeManagerStatic : MonoBehaviour
{
    [Serializable]
    public class Upgrade
    {
        public string title;

        [Header("Progression")]
        public int level = 0;
        public int maxLevel = 25;
        public int baseCost = 10;
        public float costGrowth = 1.25f;

        [Header("Effect")]
        public float valuePerLevel = 1f;

        public bool IsMaxed => level >= maxLevel;

        public int NextCost
        {
            get
            {
                if (IsMaxed) return int.MaxValue;
                float cost = baseCost * Mathf.Pow(costGrowth, level);
                return Mathf.Max(1, Mathf.RoundToInt(cost));
            }
        }

        public float TotalValue => level * valuePerLevel;
    }

    [Header("Four Static Upgrades")]
    public Upgrade robotSpeed = new Upgrade { title = "Robot Speed", valuePerLevel = 0.25f, baseCost = 10, costGrowth = 1.22f, maxLevel = 50 };
    public Upgrade dataGain = new Upgrade { title = "Data Gain", valuePerLevel = 1f, baseCost = 10, costGrowth = 1.25f, maxLevel = 50 };
    public Upgrade overload = new Upgrade { title = "Overload Gain", valuePerLevel = 1f, baseCost = 10, costGrowth = 1.25f, maxLevel = 50 };
    public Upgrade chargeRate = new Upgrade { title = "Charge Rate", valuePerLevel = 0.05f, baseCost = 15, costGrowth = 1.28f, maxLevel = 30 };
    // chargeRate interpreted as "faster charge %" per level. Example: 0.05 = 5% faster per level.

    public event Action OnChanged;

    public bool TryBuy(Upgrade up, GameDirector director)
    {
        if (up == null || director == null) return false;
        if (up.IsMaxed) return false;

        int cost = up.NextCost;
        if (director.Data < cost) return false;

        director.SpendData(cost);
        up.level++;

        OnChanged?.Invoke();
        return true;
    }

    // Convenience getters used by gameplay:
    public float RobotSpeedBonus() => robotSpeed.TotalValue;         // +speed
    public int DataBonus() => Mathf.RoundToInt(dataGain.TotalValue); // +data
    public float OverloadBonus() => overload.TotalValue;             // +overload

    public float ChargeSecondsMultiplier()
    {
        // Faster charge reduces secondsToCharge
        // totalFaster = 0.05 * level => 5% faster per level
        float totalFaster = chargeRate.TotalValue;
        return Mathf.Clamp(1f - totalFaster, 0.2f, 1f); // don’t go below 20% of base
    }
}
