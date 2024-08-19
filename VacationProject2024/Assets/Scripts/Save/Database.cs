using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Database
{
    public SerializableDIctionary<Vector3, bool> savePointsDict;
    public Vector3 savePoint;

    public SerializableDIctionary<string, SaveZombieData> zombieData;







    public Database()
    {
        savePointsDict = new SerializableDIctionary<Vector3, bool>();
        savePoint = new Vector3();
        zombieData = new();
    }
}
