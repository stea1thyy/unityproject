using UnityEngine;

public class QuickbarHotkey : MonoBehaviour
{
    public PlayerInventory inventory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            inventory.SelectSlot(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            inventory.SelectSlot(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            inventory.SelectSlot(2);
    }
}
