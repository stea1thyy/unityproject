using UnityEngine;

[System.Serializable]
public class PlanetData
{
    public string planetName;      // Button name match
    public Transform planet;       // Planet transform
    public Transform spawnPoint;   // Player spawn point

    // Info UI shown only on first visit
    public GameObject infoCanvas;
    [HideInInspector] public int visitCount = 0;
}

public class SpaceshipUIController : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;

    [Header("Player")]
    public Transform player;
    public PlayerMovementController playerMovement;

    [Header("Sun")]
    public SunOrbit sunOrbit;

    [Header("Planets")]
    public PlanetData[] planets;

    bool planetInfoOpen = false;
    GameObject currentInfoCanvas = null;

    void Start()
    {
        panel.SetActive(false);

        // Ensure all planet info UIs start hidden
        foreach (PlanetData p in planets)
        {
            if (p.infoCanvas != null)
                p.infoCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // ESC key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // close planet info
            if (planetInfoOpen && currentInfoCanvas != null)
            {
                ClosePlanetInfo(currentInfoCanvas);
                return;
            }

            // close spaceship UI
            if (panel.activeSelf)
            {
                CloseUI();
                return;
            }
        }
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

        if (!planetInfoOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PopUpManager.IsAnyUIAcive = false;
        }
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

        Debug.LogError("Planet not found: " + planetName);
    }

    void TeleportToPlanet(PlanetData data)
    {
        if (data.planet == null || data.spawnPoint == null)
        {
            Debug.LogError("Planet or spawn point missing for " + data.planetName);
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();

        cc.enabled = false;
        player.position = data.spawnPoint.position;
        player.rotation = data.spawnPoint.rotation;
        cc.enabled = true;

        playerMovement.planet = data.planet;

        if (sunOrbit != null)
            sunOrbit.SetOrbitCenter(data.planet);

        data.visitCount++;

        // First visit show info UI
        if (data.visitCount == 1 && data.infoCanvas != null)
        {
            data.infoCanvas.SetActive(true);
            currentInfoCanvas = data.infoCanvas;
            planetInfoOpen = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PopUpManager.IsAnyUIAcive = true;
        }
        else
        {
            // Revisi. This ensures gameplay is unfrozen
            planetInfoOpen = false;
            currentInfoCanvas = null;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PopUpManager.IsAnyUIAcive = false;
        }
    }

    // Called by X button OR Escape
    public void ClosePlanetInfo(GameObject canvas)
    {
        if (canvas == null) return;

        canvas.SetActive(false);
        planetInfoOpen = false;
        currentInfoCanvas = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PopUpManager.IsAnyUIAcive = false;
    }
}
