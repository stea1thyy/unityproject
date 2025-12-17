using UnityEngine;

public class SpaceshipInteract : MonoBehaviour
{
    public SpaceshipUIController spaceshipUI;
    public GameObject pressEPrompt;

    private bool playerInRange = false;

    void Start()
    {
        pressEPrompt.SetActive(false);
    }

// The code bellow is used to trigger a panel thats in the game.
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        pressEPrompt.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        pressEPrompt.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            pressEPrompt.SetActive(false);
            spaceshipUI.OpenUI();
        }
    }
}
