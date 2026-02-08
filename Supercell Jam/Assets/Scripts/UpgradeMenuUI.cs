using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuUI : MonoBehaviour
{
    public GameDirector director;
    public UpgradeManager upgradeManager;

    [Header("List")]
    public Transform content;
    public UpgradeItemUI itemPrefab;

    readonly List<UpgradeItemUI> items = new();

    void Start()
    {
        Build();
        if (upgradeManager) upgradeManager.OnUpgradesChanged += RefreshAll;
    }

    void OnDestroy()
    {
        if (upgradeManager) upgradeManager.OnUpgradesChanged -= RefreshAll;
    }

    void Build()
    {
        foreach (Transform child in content) Destroy(child.gameObject);
        items.Clear();

        foreach (var up in upgradeManager.upgrades)
        {
            var item = Instantiate(itemPrefab, content);
            item.Bind(up, upgradeManager, director);
            items.Add(item);
        }
    }

    public void RefreshAll()
    {
        foreach (var item in items) item.Refresh();
    }
}
