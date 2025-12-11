using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class PopUpManager : MonoBehaviour
{
    // This static flag controls whether ANY UI is blocking player input
    public static bool IsAnyUIAcive = false; 

    [Header("Spawn Pop-up References")]
    public GameObject EarthInfoPanel; 
    private Image earthInfoImage; 

    void Awake() 
    {
        if (EarthInfoPanel != null)
        {
            earthInfoImage = EarthInfoPanel.GetComponent<Image>();
        }
        
        if (earthInfoImage == null)
            Debug.LogError("The EarthInfoPanel GameObject is missing an Image component!");
    }

    void Start()
    {
        // Start the process of showing the pop up after all other Start
        StartCoroutine(DelayedShowPopUp());
    }

    // Executes the ShowPopUp function 
    IEnumerator DelayedShowPopUp()
    {
        yield return null; 
        
        ShowPopUp();
    }

    // Public method called by the Close Button.
    public void ClosePopUp()
    {
        if (EarthInfoPanel != null)
            EarthInfoPanel.SetActive(false);
        
        if (earthInfoImage != null)
            earthInfoImage.raycastTarget = false;
        
        IsAnyUIAcive = false; 
        Debug.Log("Spawn Pop-up closed. Locking cursor for gameplay.");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Utility method to show the pop-up.
    public void ShowPopUp()
    {
        if (EarthInfoPanel != null)
            EarthInfoPanel.SetActive(true);
        
        if (earthInfoImage != null)
            earthInfoImage.raycastTarget = true;
        
        IsAnyUIAcive = true; 
        Debug.Log("Spawn Pop up displayed. Unlocking cursor for interaction.");
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}