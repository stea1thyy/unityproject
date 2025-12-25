using UnityEngine;

public class OreJournalUI : MonoBehaviour
{
    public static OreJournalUI Instance;

    [Header("UI")]
    public GameObject oreJournalCanvas;
    public OreJournalButton[] journalButtons;

    private bool isOpen = false;

    void Awake()
    {
        // Simple singleton so other scripts can access the journal
        Instance = this;

        // Journal starts closed
        oreJournalCanvas.SetActive(false);
    }

    void Update()
    {
        // Keep the cursor free while the journal is open
        if (isOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Called when an ore gets discovered in-game
    public void DiscoverOre(OreType type)
    {
        foreach (OreJournalButton button in journalButtons)
        {
            if (button.oreType == type)
            {
                button.Discover();
                return;
            }
        }
    }

    // Opens the journal UI
    public void Open()
    {
        oreJournalCanvas.SetActive(true);
        isOpen = true;

        PopUpManager.IsAnyUIAcive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Closes the journal (X button)
    public void Close()
    {
        oreJournalCanvas.SetActive(false);
        isOpen = false;

        PopUpManager.IsAnyUIAcive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
