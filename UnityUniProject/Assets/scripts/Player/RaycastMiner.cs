using UnityEngine;

public class RaycastMiner : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f;        // How far we can mine
    public LayerMask mineableLayers;       // What layers count as ore

    [Header("Ore Info UI")]
    public GameObject oreInfoPanel;        // UI shown when aiming at ore

    [Header("Player Reference")]
    public PlayerInventory playerInventory;

    private OreData currentOreTarget;      // Ore currently hit by ray

    void Start()
    {
        // Auto-find inventory if not set
        if (playerInventory == null)
            playerInventory = FindFirstObjectByType<PlayerInventory>();

        if (playerInventory == null)
            Debug.LogError("RaycastMiner: PlayerInventory NOT found in scene!");

        // Hide UI on start
        if (oreInfoPanel != null)
            oreInfoPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInventory == null)
            return;

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        // Don't mine while other UI is open
        if (PopUpManager.IsAnyUIAcive)
        {
            if (oreInfoPanel != null)
                oreInfoPanel.SetActive(false);
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        currentOreTarget = null;

        if (oreInfoPanel != null)
            oreInfoPanel.SetActive(false);

        // Check for ore in front of player
        if (Physics.Raycast(ray, out hit, maxDistance, mineableLayers))
        {
            currentOreTarget = hit.collider.GetComponent<OreData>();

            if (currentOreTarget != null)
            {
                if (oreInfoPanel != null)
                    oreInfoPanel.SetActive(true);

                // Need pickaxe to mine
                if (!playerInventory.hasPickaxe)
                    return;

                if (Input.GetMouseButtonDown(0))
                    currentOreTarget.MineOre();
            }
        }
    }
}
