using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descText;
    public TMP_Text levelText;
    public TMP_Text costText;
    public Button buyButton;

    UpgradeDefinitionSO def;
    UpgradeManager manager;
    GameDirector director;

    public void Bind(UpgradeDefinitionSO definition, UpgradeManager mgr, GameDirector dir)
    {
        def = definition;
        manager = mgr;
        director = dir;

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(Buy);

        Refresh();
    }

    public void Refresh()
    {
        if (!def || !manager) return;

        int lvl = manager.GetLevel(def);
        bool maxed = manager.IsMaxed(def);
        int cost = manager.GetCost(def);

        if (titleText) titleText.text = def.displayName;
        if (descText) descText.text = def.description;
        if (levelText) levelText.text = $"Lv {lvl}/{def.maxLevel}";
        if (costText) costText.text = maxed ? "MAX" : cost.ToString();

        bool canAfford = director && director.Data >= cost;
        buyButton.interactable = !maxed && canAfford;
    }

    void Buy()
    {
        if (manager.TryBuy(def, director))
            Refresh();
    }
}
