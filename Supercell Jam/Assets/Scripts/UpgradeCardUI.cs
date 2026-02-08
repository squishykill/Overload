using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCardUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text levelText;
    public TMP_Text costText;
    public Button upgradeButton;

    GameDirector director;
    UpgradeManagerStatic manager;
    UpgradeManagerStatic.Upgrade upgrade;

    public void Bind(GameDirector dir, UpgradeManagerStatic mgr, UpgradeManagerStatic.Upgrade up)
    {
        director = dir;
        manager = mgr;
        upgrade = up;

        if (upgradeButton)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() =>
            {
                if (manager.TryBuy(upgrade, director))
                    Refresh();
            });
        }

        Refresh();
    }

    public void Refresh()
    {
        if (upgrade == null) return;

        if (titleText) titleText.text = upgrade.title;
        if (levelText) levelText.text = $"Lv {upgrade.level}/{upgrade.maxLevel}";

        bool maxed = upgrade.IsMaxed;
        int cost = upgrade.NextCost;

        if (costText) costText.text = maxed ? "MAX" : cost.ToString();

        bool canAfford = director && director.Data >= cost;
        if (upgradeButton) upgradeButton.interactable = !maxed && canAfford;
    }
}
