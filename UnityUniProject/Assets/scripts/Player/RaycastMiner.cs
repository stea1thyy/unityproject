using UnityEngine;

public class RaycastMiner : MonoBehaviour
{
    [Header("Raycast Settings")]
    public float maxDistance = 5f; 
    public LayerMask mineableLayers; 

    [Header("Ore Info UI")]
    public GameObject oreInfoPanel; 

    private OreData currentOreTarget;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);

        // UI Block Check
        if (PopUpManager.IsAnyUIAcive)
        {
            if (oreInfoPanel != null)
            {
                oreInfoPanel.SetActive(false);
            }
            return; 
        }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        // Reset target and hide UI before checking
        currentOreTarget = null;
        if (oreInfoPanel != null)
        {
            oreInfoPanel.SetActive(false); 
        }

        // Raycast Check
        if (Physics.Raycast(ray, out hit, maxDistance, mineableLayers))
        {
            // Check if the hit object has the OreData component
            currentOreTarget = hit.collider.GetComponent<OreData>();

            if (currentOreTarget != null)
            {
                // turn the panel ON.
                // The text (IronNameText, IronText) will display whatever was set in the Editor.
                if (oreInfoPanel != null)
                {
                    oreInfoPanel.SetActive(true);
                }
                
                //  Handle Mining Input
                if (Input.GetMouseButtonDown(0)) 
                {
                    currentOreTarget.MineOre();
                }
            }
        }
    }
}