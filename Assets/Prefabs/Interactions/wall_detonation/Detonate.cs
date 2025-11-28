using System.Threading;
using UnityEngine;

public class Detonate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Delay for 3 seconds before detonating
        Thread.Sleep(3000);
        // Check for items in blast radius
        // Play explosion animation
        // Remove TNT-item itself
        Destroy(gameObject);
        // Destroy items in blast radius
        
        // Find all Objects in 3m radius
    }
}
