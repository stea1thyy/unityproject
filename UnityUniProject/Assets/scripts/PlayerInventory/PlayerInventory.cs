using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Money")]
    public int money = 0;

    [Header("Ore Chunks (stored by name)")]
    public Dictionary<string, int> oreChunks = new Dictionary<string, int>();

    [Header("Pickaxe")]
    public bool hasPickaxe = false;
    public GameObject pickaxeModel;

    // =============================
    // QUICKBAR (3 SLOTS)
    // =============================
    [Header("Quickbar Slots")]
    public string[] quickSlots = new string[3];

    [Header("Selected Slot")]
    public int selectedSlot = 0;   // 0 = Slot1, 1 = Slot2, 2 = Slot3


    // =============================
    // QUICKBAR FUNCTIONS
    // =============================

    // Put an item in a slot
    public void AssignToSlot(int slot, string item)
    {
        if (slot < 0 || slot >= quickSlots.Length)
            return;

        quickSlots[slot] = item;
    }

    // Get the item name from a slot
    public string GetSlotItem(int slot)
    {
        if (slot < 0 || slot >= quickSlots.Length)
            return "";

        return quickSlots[slot];
    }

    // Select a slot (used by hotkeys 1/2/3)
    public void SelectSlot(int slot)
    {
        if (slot < 0 || slot >= quickSlots.Length)
            return;

        selectedSlot = slot;
        Debug.Log("Selected slot: " + slot + " (" + GetSlotItem(slot) + ")");
    }


    // =============================
    // MONEY
    // =============================
    public void AddMoney(int amount)
    {
        money += amount;
    }


    // =============================
    // PICKAXE
    // =============================
    public void GivePickaxe()
    {
        hasPickaxe = true;

        // Automatically place pickaxe into slot 0 if empty
        if (string.IsNullOrEmpty(quickSlots[0]))
            AssignToSlot(0, "Pickaxe");

        if (pickaxeModel != null)
            pickaxeModel.SetActive(true);

        Debug.Log("Player received pickaxe.");
    }


    // =============================
    // ORE FUNCTIONS
    // =============================
    public void AddOreChunk(string oreName, int amount)
    {
        if (!oreChunks.ContainsKey(oreName))
            oreChunks[oreName] = 0;

        oreChunks[oreName] += amount;

        Debug.Log("Picked up " + amount + " " + oreName + " chunk(s). Total: " + oreChunks[oreName]);
    }

    public int GetOreCount(string oreName)
    {
        if (oreChunks.ContainsKey(oreName))
            return oreChunks[oreName];

        return 0;
    }

    public void RemoveOre(string oreName, int amount)
    {
        if (!oreChunks.ContainsKey(oreName))
            return;

        oreChunks[oreName] -= amount;

        if (oreChunks[oreName] < 0)
            oreChunks[oreName] = 0;
    }


    // =============================
    // SELLING
    // =============================
    public void SellOre(string oreName, int amount, int pricePerChunk)
    {
        if (!oreChunks.ContainsKey(oreName) || oreChunks[oreName] < amount)
            return;

        oreChunks[oreName] -= amount;
        money += amount * pricePerChunk;

        Debug.Log("Sold " + amount + " " + oreName + " chunk(s) for $" + (amount * pricePerChunk));
    }
}
