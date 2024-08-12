using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] float m_maxHp;
    public float maxHp { get { return m_maxHp; } }
    public float hp { get; private set; }
    private void Awake()
    {
        hp = maxHp;
    }
    public void GetDamage(float damage)
    {
        Debug.Log("oof");
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}