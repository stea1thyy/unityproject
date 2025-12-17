using UnityEngine;

[System.Serializable]
public class PlanetData
{
    public string planetName;      // Earth,Mars, etc.
    public Transform planet;       // Planet transform
    public Transform spawnPoint;   // Where player spawns
}

public class SpaceshipUIController : MonoBehaviour
{
    public GameObject panel;

    [Header("Player")]
    public Transform player;
    public PlayerMovementController playerMovement;

    [Header("Planets")]
    public PlanetData[] planets;   // All of the planets go here

    void Start()
    {
        panel.SetActive(false);
    }

    public void OpenUI()
    {
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PopUpManager.IsAnyUIAcive = true;
    }

    public void CloseUI()
    {
        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PopUpManager.IsAnyUIAcive = false;
    }

    public void SelectPlanet(string planetName)
    {
        for (int i = 0; i < planets.Length; i++)
        {
            if (planets[i].planetName == planetName)
            {
                TeleportToPlanet(planets[i]);
                CloseUI();
                return;
            }
        }

        Debug.LogError("Planet not found: " + planetName); // Debug here was used to figure out an issue with planets not being located
    }

    void TeleportToPlanet(PlanetData data)
    {
        if (data.planet == null || data.spawnPoint == null)
        {
            Debug.LogError("Planet or spawn point missing for " + data.planetName);
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();

        // Safe teleport
        cc.enabled = false;
        player.position = data.spawnPoint.position;
        player.rotation = data.spawnPoint.rotation;
        cc.enabled = true;

        // Switch gravity
        playerMovement.planet = data.planet;
    }
}
