using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OreJournalButton : MonoBehaviour
{
    [Header("Ore")]
    public OreType oreType;

    [Header("UI")]
    public Button button;
    public TextMeshProUGUI label;

    [Header("Info Panel")]
    [Tooltip("The same info panel used when mining the ore")]
    public GameObject infoPanel;

    private bool discovered = false;

    void Awake()
    {
        // Make sure no old listeners are hanging around
        if (button != null)
            button.onClick.RemoveAllListeners();

        // If this ore was discovered before this button's Awake ran don't reset it. (AI was used her to figure out an issue. i had an issue where whenever i mined the first ore it wouldnt pop up in the info page)
        if (discovered)
        {
            // Re-apply discovered state visuals + click
            if (label != null)
                label.text = oreType.ToString();

            if (button != null)
            {
                button.interactable = true;
                button.onClick.AddListener(OpenInfoFromJournal);
            }

            return;
        }

        // Locked by default
        discovered = false;

        if (label != null)
            label.text = "?????";

        if (button != null)
            button.interactable = false;
    }

    // Called once when the ore is first discovered
    public void Discover()
    {
        if (discovered)
            return;

        discovered = true;

        if (label != null)
            label.text = oreType.ToString();

        if (button != null)
        {
            button.interactable = true;

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenInfoFromJournal);
        }
    }

    // Opens the ore info panel from the journal
    void OpenInfoFromJournal()
    {
        // Close the journal first
        OreJournalUI.Instance.Close();

        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            infoPanel.transform.SetAsLastSibling();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PopUpManager.IsAnyUIAcive = true;
    }
}
