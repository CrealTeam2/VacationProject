using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Activator_Trigger : Activator
{
    bool t = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && t == false)
        {
            Activate();
            SoundManager.Instance.PlaySound(gameObject, "Scream", 1, 1);
            SoundManager.Instance.PlaySound(GameManager.Instance.player.transform.Find("Rotator").gameObject, "Chase", 1, 1);
            t = true;
        }
    }
}
