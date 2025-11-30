using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactUI;   // “Press E to talk”
    public GameObject npcMenu;      // NPC shop/dialog UI

    private bool playerNear = false;
    private PlayerInventory playerInventory;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered NPC trigger.");

            playerNear = true;

            playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory == null)
                Debug.LogWarning("Player does NOT have PlayerInventory!");

            if (interactUI != null)
                interactUI.SetActive(true);
            else
                Debug.LogWarning("InteractUI is NOT assigned!");
        }
        else
        {
            Debug.LogWarning("Object entered trigger but tag is NOT Player. Tag = " + other.tag);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited NPC trigger.");

            playerNear = false;

            if (interactUI != null)
                interactUI.SetActive(false);

            if (npcMenu != null)
                npcMenu.SetActive(false);

            // Lock the cursor back when leaving
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Opening NPC menu.");

            if (npcMenu != null)
            {
                npcMenu.SetActive(true);

                // Unlock cursor so player can click UI
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Debug.LogWarning("NPCMenu is NOT assigned!");
            }
        }
    }

    // Called by UI Button
    public void GivePickaxeToPlayer()
    {
        if (playerInventory != null)
        {
            playerInventory.GivePickaxe();
            Debug.Log("NPC gave pickaxe to player.");
        }
        else
        {
            Debug.LogWarning("PlayerInventory is NULL — cannot give pickaxe!");
        }
    }

    // Called by a “Close” button
    public void CloseMenu()
    {
        if (npcMenu != null)
            npcMenu.SetActive(false);

        Debug.Log("Closed NPC menu.");

        // Lock cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
