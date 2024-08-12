using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class TouchDamager : MonoBehaviour
{
    [SerializeField] float damage;
    List<EnemyTest> hit = new();
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
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            EnemyTest tmp = collision.GetComponent<EnemyTest>();
            if (!hit.Contains(tmp))
            {
                OnHit(tmp);
                hit.Add(tmp);
            }
        }
    }
    protected virtual void OnHit(EnemyTest hitEnemy)
    {
        hitEnemy.GetDamage(damage);
    }
}
