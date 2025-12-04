using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Money")]
    public int money = 0;

    [Header("Pickaxe")]
    public bool hasPickaxe = false;
    public GameObject pickaxeModel;   // Assign the pickaxe model in the inspector

    void Start()
    {
        // Hide pickaxe at start
        if (pickaxeModel != null)
            pickaxeModel.SetActive(false);
    }

    // Called by NPC when giving the player a pickaxe
    public void GivePickaxe()
    {
        hasPickaxe = true;

        if (pickaxeModel != null)
            pickaxeModel.SetActive(true);

        Debug.Log("Player received pickaxe.");
    }

    // Optional — add money
    public void AddMoney(int amount)
    {
        money += amount;
    }
}
