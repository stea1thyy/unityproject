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
        // Locked by default
        discovered = false;
        label.text = "?????";
        button.interactable = false;

        // Make sure no old listeners are hanging around
        button.onClick.RemoveAllListeners();
    }

    // Called once when the ore is first discovered
    public void Discover()
    {
        if (discovered)
            return;

        discovered = true;
        label.text = oreType.ToString();
        button.interactable = true;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OpenInfoFromJournal);
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
