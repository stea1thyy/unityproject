using UnityEngine;

public class RaycastMiner : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f;
    public LayerMask mineableLayers;

    [Header("Ore Hint UI (small prompt only)")]
    public GameObject oreInfoPanel;

    [Header("Player Reference")]
    public PlayerInventory playerInventory;

    private OreData currentOreTarget;

    void Start()
    {
        // Auto-grab inventory if not set
        if (playerInventory == null)
            playerInventory = FindFirstObjectByType<PlayerInventory>();

        if (oreInfoPanel != null)
            oreInfoPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInventory == null)
            return;

        // Stop all mining logic while any UI is open
        if (PopUpManager.IsAnyUIAcive)
        {
            if (oreInfoPanel != null)
                oreInfoPanel.SetActive(false);
            return;
        }

        currentOreTarget = null;

        if (oreInfoPanel != null)
            oreInfoPanel.SetActive(false);

        // Check for mineable ore in front of the player
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxDistance, mineableLayers))
        {
            currentOreTarget = hit.collider.GetComponentInParent<OreData>();

            if (currentOreTarget != null)
            {
                if (oreInfoPanel != null)
                    oreInfoPanel.SetActive(true);

                // Can’t mine without a pickaxe
                if (!playerInventory.hasPickaxe)
                    return;

                if (Input.GetMouseButtonDown(0))
                    currentOreTarget.Mine();
            }
        }
    }
}
