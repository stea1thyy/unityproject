using UnityEngine;
using TMPro;

public class QuickbarUI : MonoBehaviour
{
    public PlayerInventory inventory;

    public TMP_Text slot1Text;
    public TMP_Text slot2Text;
    public TMP_Text slot3Text;

    void Update()
    {
        slot1Text.text = inventory.GetSlotItem(0);
        slot2Text.text = inventory.GetSlotItem(1);
        slot3Text.text = inventory.GetSlotItem(2);
    }
}
