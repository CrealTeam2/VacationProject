using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyDetector : MonoBehaviour
{
    public Action<EnemyTest> onHit;
    List<Collider> hit = new();
    Collider m_collider;
    Collider _collider { get { if (m_collider == null) m_collider = GetComponent<Collider>(); return m_collider; } }
    private void OnEnable()
    {
        hit.Clear();
        _collider.enabled = true;
    }
    private void OnDisable()
    {
        _collider.enabled = false;
    }
    private void OnTriggerStay(Collider collision)
    {
        if (!hit.Contains(collision))
        {
            hit.Add(collision);
            if (collision.gameObject.CompareTag("Enemy"))
            {
                EnemyTest tmp = collision.GetComponent<EnemyTest>();
                onHit.Invoke(tmp);
            }
        }
    }
}