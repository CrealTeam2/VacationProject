using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

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
    public void GiveDebuff()
    {
        Player tmp = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //tmp.AddDebuff(new Grabbed(10.0f, this));
    }
}
[CustomEditor(typeof(EnemyTest))]
public class EnemyTest_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Give Debuff"))
        {
            (target as EnemyTest).GiveDebuff();
        }
    }
}