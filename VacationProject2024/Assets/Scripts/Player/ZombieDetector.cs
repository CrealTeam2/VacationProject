using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZombieDetector : MonoBehaviour
{
    public List<Zombie> detected { get; } = new();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            detected.Add(other.GetComponent<Zombie>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            detected.Remove(other.GetComponent<Zombie>());
        }
    }
}
