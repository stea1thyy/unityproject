using UnityEngine;

public class SunOrbit : MonoBehaviour
{
    [Header("Orbit Settings")]
    public Transform orbitCenter;
    public float orbitSpeed = 5f;
    public Vector3 orbitAxis = Vector3.up;

    void Update()
    {
        // Nothing to orbit around yet
        if (orbitCenter == null)
            return;

        // Simple visual orbit around the current planet
        transform.RotateAround(
            orbitCenter.position,
            orbitAxis,
            orbitSpeed * Time.deltaTime
        );
    }

    // Called when the player switches planets
    public void SetOrbitCenter(Transform newCenter)
    {
        orbitCenter = newCenter;
    }
}
