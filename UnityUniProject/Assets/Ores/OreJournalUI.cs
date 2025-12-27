using UnityEngine;

public class OreJournalUI : MonoBehaviour
{
    public static OreJournalUI Instance;

    [Header("UI")]
    public GameObject oreJournalCanvas;
    public OreJournalButton[] journalButtons;

    [Header("Audio")]
    public AudioClip openSound;     // played when journal opens
    public AudioClip closeSound;    // played when journal closes
    public AudioClip discoverSound; // played when an ore is discovered

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

                // Play discovery sound
                if (discoverSound != null)
                    AudioSource.PlayClipAtPoint(discoverSound, Camera.main.transform.position);

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

        // Play open sound
        if (openSound != null)
            AudioSource.PlayClipAtPoint(openSound, Camera.main.transform.position);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Closes the journal (X button)
    public void Close()
    {
        oreJournalCanvas.SetActive(false);
        isOpen = false;

        PopUpManager.IsAnyUIAcive = false;

        // Play close sound
        if (closeSound != null)
            AudioSource.PlayClipAtPoint(closeSound, Camera.main.transform.position);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
