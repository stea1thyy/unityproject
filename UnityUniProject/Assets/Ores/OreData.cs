using UnityEngine;

public class OreData : MonoBehaviour
{
    [Header("Ore")]
    public OreType oreType;

    [Header("Mining")]
    public int maxHealth = 3;
    public GameObject miningFX;

    [Header("Info UI")]
    [Tooltip("Info panel for this ore (panel object, not the canvas)")]
    public GameObject infoPanel;

    private int currentHealth;
    private bool infoShown = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        // Make sure the info panel starts hidden
        if (infoPanel != null)
            infoPanel.SetActive(false);

        PopUpManager.IsAnyUIAcive = false;
    }

    // Called when the player mines this ore
    public void Mine()
    {
        currentHealth--;

        if (miningFX != null)
            Instantiate(miningFX, transform.position, Quaternion.identity);

        // First time this ore is mined
        if (!infoShown)
        {
            infoShown = true;

            if (OreJournalUI.Instance != null)
                OreJournalUI.Instance.DiscoverOre(oreType);

            OpenInfo();
        }

        // Remove the ore once it's depleted
        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    void OpenInfo()
    {
        if (infoPanel == null)
            return;

        infoPanel.SetActive(true);
        infoPanel.transform.SetAsLastSibling();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PopUpManager.IsAnyUIAcive = true;
    }

    // Assigned to the X button on the info panel
    public void CloseInfo()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PopUpManager.IsAnyUIAcive = false;
    }
}
