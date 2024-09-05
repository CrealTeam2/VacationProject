using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieManager : Singleton<ZombieManager>, ISavable
{
    [SerializeField] Dictionary<string, Zombie> zombieDict = new();
    public Dictionary<string, Zombie> ZombieDict => zombieDict;
    private void Awake()
    {
/*        zombieDict = new Dictionary<string, Zombie>();*/
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterZombie(string id, Zombie zombie)
    {
        zombieDict[id] = zombie;
    }
    public void LoadData(Database data)
    {
        List<string> removeZomdataId = new();
        foreach (KeyValuePair<string, SaveZombieData> item in data.zombieData)
        {
            Zombie zombie;
            zombieDict.TryGetValue(item.Key, out zombie);
            if (zombie == null)
            {
                removeZomdataId.Add(item.Key);
                continue;
            }
            zombie.GetComponent<NavMeshAgent>().Warp(new Vector3(item.Value.x, item.Value.y, item.Value.z));
            zombie.Health = item.Value.health;
            zombie.Activation = item.Value.activation;
            zombie.IsEnabled = item.Value.isEnabled;
            if (item.Value.isDead || item.Value.health <= 0)
            {
                zombie.anim.Play("Death", 0, 1);
                zombie.Die();
            }
            zombie.gameObject.SetActive(item.Value.isEnabled);
        }

        while(removeZomdataId.Count > 0)
        {
            data.zombieData.Remove(removeZomdataId[0]);
            removeZomdataId.RemoveAt(0);
        }
    }

    public void SaveData(ref Database data)
    {
        foreach (KeyValuePair<string, Zombie> item in zombieDict)
        {
            data.zombieData[item.Key] = new SaveZombieData()
            {
                x = item.Value.transform.position.x,
                y = item.Value.transform.position.y,
                z = item.Value.transform.position.z,
                health = item.Value.Health,
                activation = item.Value.Activation,
                isEnabled = item.Value.IsEnabled,
                isDead = item.Value.isDead
            };
        }
    }
}

[Serializable]
public class SaveZombieData
{
    public float x, y, z;
    public float activation;
    public float health;
    public bool isEnabled, isDead;
}
