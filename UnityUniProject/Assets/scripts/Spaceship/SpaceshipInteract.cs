using UnityEngine;

public class SpaceshipInteract : MonoBehaviour
{
    public SpaceshipUIController spaceshipUI;
    public GameObject pressEPrompt;

    // Tracks whether the player is inside the trigger
    bool playerInRange = false;

    void Start()
    {
        // Hide the prompt by default
        if (pressEPrompt != null)
            pressEPrompt.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Player entered the spaceship area
        playerInRange = true;

        if (pressEPrompt != null)
            pressEPrompt.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Player left the spaceship area
        playerInRange = false;

        if (pressEPrompt != null)
            pressEPrompt.SetActive(false);
    }

    void Update()
    {
        // Don't do anything if the player isn't nearby
        if (!playerInRange)
            return;

        // If the prompt isn't visible, this interact shouldn't fire
        if (pressEPrompt == null || !pressEPrompt.activeSelf)
            return;

        // Block interaction if another UI is already open
        if (PopUpManager.IsAnyUIAcive)
            return;

        // Open the spaceship UI
        if (Input.GetKeyDown(KeyCode.E))
        {
            pressEPrompt.SetActive(false);
            spaceshipUI.OpenUI();
        }
    }
}
