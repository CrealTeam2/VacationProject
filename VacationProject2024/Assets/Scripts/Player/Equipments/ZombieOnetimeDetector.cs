using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ZombieOnetimeDetector : MonoBehaviour
{
    public Action<Zombie> onHit;
    bool hit = false;
    Collider m_collider;
    Collider _collider { get { if (m_collider == null) m_collider = GetComponent<Collider>(); return m_collider; } }
    private void OnEnable()
    {
        hit = false;
        _collider.enabled = true;
    }
    private void OnDisable()
    {
        _collider.enabled = false;
    }
    public void Refresh() => hit = false;
    private void OnTriggerStay(Collider collision)
    {
        if (hit) return;
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Zombie tmp = collision.GetComponent<Zombie>();
            onHit?.Invoke(tmp);
            hit = true;
        }
    }
}