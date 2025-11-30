using UnityEngine;

public class Ore : MonoBehaviour
{
    public string oreName;
    public int chunkAmount = 1;    // how many chunks you get
    public GameObject chunkPrefab; // the chunk to give the player

    public void Mine(PlayerInventory player)
    {
        // Give chunks to player
        player.AddOreChunk(oreName, chunkAmount);

        // Destroy ore in world
        Destroy(gameObject);
    }
}
