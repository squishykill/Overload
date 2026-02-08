using UnityEngine;

public class UpgradeScreenUI : MonoBehaviour
{
    public GameDirector director;
    public UpgradeManagerStatic upgrades;

    public UpgradeCardUI robotSpeedCard;
    public UpgradeCardUI dataGainCard;
    public UpgradeCardUI overloadCard;
    public UpgradeCardUI chargeRateCard;

    void Start()
    {
        robotSpeedCard.Bind(director, upgrades, upgrades.robotSpeed);
        dataGainCard.Bind(director, upgrades, upgrades.dataGain);
        overloadCard.Bind(director, upgrades, upgrades.overload);
        chargeRateCard.Bind(director, upgrades, upgrades.chargeRate);

        // Refresh when upgrades change (purchase)
        if (upgrades) upgrades.OnChanged += RefreshAll;

        // Refresh when Data changes (affordability)
        if (director) director.OnDataChanged += OnDataChanged;

        RefreshAll();
    }

    void OnDestroy()
    {
        if (upgrades) upgrades.OnChanged -= RefreshAll;
        if (director) director.OnDataChanged -= OnDataChanged;
    }

    void OnDataChanged(int newData)
    {
        RefreshAll();
    }

    public void RefreshAll()
    {
        robotSpeedCard.Refresh();
        dataGainCard.Refresh();
        overloadCard.Refresh();
        chargeRateCard.Refresh();
    }
}

