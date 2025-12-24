using UnityEngine;

public class OreData : MonoBehaviour
{
    [Header("Mining")]
    public int maxHealth = 3;
    public GameObject miningFX;

    [Header("Info UI")]
    public GameObject infoCanvas;   // Drag ore info canvas here

    int currentHealth;
    bool infoShown = false;

    void Awake()
    {
        currentHealth = maxHealth;

        // Info UI should start hidden
        if (infoCanvas != null)
            infoCanvas.SetActive(false);
    }

    // Called by RaycastMiner when player mines
    public void Mine()
    {
        currentHealth--;

        if (miningFX != null)
            Instantiate(miningFX, transform.position, Quaternion.identity);

        // First time the player actually mines this ore, show info
        if (!infoShown && infoCanvas != null)
        {
            infoShown = true;
            OpenInfo();
        }

        if (currentHealth <= 0)
            Destroy(gameObject);
    }

    void OpenInfo()
    {
        infoCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PopUpManager.IsAnyUIAcive = true;
    }

    // Hook this to the X button on the ore info canvas
    public void CloseInfo()
    {
        if (infoCanvas != null)
            infoCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PopUpManager.IsAnyUIAcive = false;
    }
}
