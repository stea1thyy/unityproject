using UnityEngine;

public class RaycastMiner : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f;
    public LayerMask mineableLayers;

    [Header("Ore Hint UI")]
    public GameObject oreInfoPanel;    // optional small "aiming at ore" hint

    [Header("Player Reference")]
    public PlayerInventory playerInventory;

    OreData currentOreTarget;

    void Start()
    {
        if (playerInventory == null)
            playerInventory = FindFirstObjectByType<PlayerInventory>();

        if (oreInfoPanel != null)
            oreInfoPanel.SetActive(false);
    }

    void Update()
    {
        if (playerInventory == null)
            return;

        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        // If any UI is open (like ore info), don't mine
        if (PopUpManager.IsAnyUIAcive)
        {
            if (oreInfoPanel != null)
                oreInfoPanel.SetActive(false);
            return;
        }

        currentOreTarget = null;

        if (oreInfoPanel != null)
            oreInfoPanel.SetActive(false);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, mineableLayers))
        {
            currentOreTarget = hit.collider.GetComponent<OreData>();

            if (currentOreTarget != null)
            {
                if (oreInfoPanel != null)
                    oreInfoPanel.SetActive(true);

                if (!playerInventory.hasPickaxe)
                    return;

                if (Input.GetMouseButtonDown(0))
                {
                    currentOreTarget.Mine();
                }
            }
        }
    }
}
