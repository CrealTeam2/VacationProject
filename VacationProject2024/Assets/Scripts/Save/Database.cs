using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Database
{
    public SerializableDictionary<Vector3, bool> savePointsDict;
    public Vector3 savePoint;

    public SerializableDictionary<string, SaveZombieData> zombieData;
    
    public SerializableDictionary<string, DataUnit> interactionDatas;






    public Database()
    {
        InitDatabase();
    }

    public void InitDatabase()
    {
        savePointsDict = new SerializableDictionary<Vector3, bool>();
        interactionDatas = new SerializableDictionary<string, DataUnit>();
        savePoint = new Vector3();
        zombieData = new();
    }
}
