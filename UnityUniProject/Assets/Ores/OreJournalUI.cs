using UnityEngine;

public class OreJournalUI : MonoBehaviour
{
    public static OreJournalUI Instance;

    public GameObject oreJournalCanvas;

    private void Awake()
    {
        Instance = this;
        oreJournalCanvas.SetActive(false);
    }

    // Called from PlayerMovementController (I key)
    public void Open()
    {
        oreJournalCanvas.SetActive(true);
        PopUpManager.IsAnyUIAcive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Hook this to the X button
    public void Close()
    {
        oreJournalCanvas.SetActive(false);
        PopUpManager.IsAnyUIAcive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
