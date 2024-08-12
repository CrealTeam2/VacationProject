using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : Singleton<ZombieManager>, ISavable
{
    [SerializeField] Dictionary<string, Zombie> zombieDict = new();
    public Dictionary<string, Zombie> ZombieDict => zombieDict;
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
        zombieDict.Add(id, zombie);
    }

    public void LoadData(Database data)
    {
        foreach (KeyValuePair<string, SaveZombieData> item in data.zombieData)
        {
            var zombie = zombieDict[item.Key];
            zombie.transform.position = new Vector3(item.Value.x, item.Value.y, item.Value.z);
            zombie.Health = item.Value.health;
            zombie.Activation = item.Value.activation;
            zombie.IsEnabled = item.Value.isEnabled;
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
    public bool isEnabled;
}
