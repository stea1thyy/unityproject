using UnityEngine;

public class OreData : MonoBehaviour
{
    [Header("Ore Properties")]
    public string oreName; 
    public string oreDescription; 
    
    public int health = 3;
    public GameObject miningFX; 

    // Public method called by the Player's RaycastMiner
    public void MineOre()
    {
        health--;

        if (miningFX != null)
        {
            // Instantiate particles at the block's position
            Instantiate(miningFX, transform.position, Quaternion.identity);
        }

        if (health <= 0)
        {
            Debug.Log($"Mined {oreName}!");
            Destroy(gameObject); 
        } 
        else
        {
            Debug.Log($"Hitting {oreName}. Remaining health: {health}");
        }
    }
}