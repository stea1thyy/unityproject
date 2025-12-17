using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactUI;    // “Press E to talk”
    public GameObject npcMenu;       // NPC shop/dialog UI

    private bool playerNear = false;
    private PlayerInventory playerInventory;

    void OnTriggerEnter(Collider other)
    {
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
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited NPC trigger.");

            playerNear = false;

            if (interactUI != null)
                interactUI.SetActive(false);

            if (npcMenu != null && npcMenu.activeSelf)
            {
                // Force close and re-lock cursor if player walks away
                CloseMenu(); 
            }
        }
    }

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (npcMenu != null && !npcMenu.activeSelf)
            {
                OpenMenu();
            }
        }
        
        // Allow closing menu with Escape key
        if (npcMenu != null && npcMenu.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    // Menu control functions 

    public void OpenMenu()
    {
        Debug.Log("Opening NPC menu.");

        if (npcMenu != null)
        {
            npcMenu.SetActive(true);
            
            // Set the global flag to STOP player movement
            PopUpManager.IsAnyUIAcive = true;

            // Unlock cursor for UI interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Debug.LogWarning("NPCMenu is NOT assigned!");
        }
    }

    // Called by a Close button OR the Update function
    public void CloseMenu()
    {
        if (npcMenu != null)
            npcMenu.SetActive(false);

        Debug.Log("Closed NPC menu.");

        // Set the global flag back to allow player movement
        PopUpManager.IsAnyUIAcive = false;

        // Lock cursor back for game control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}