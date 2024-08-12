using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Database
{
    public int count;
    public TestClass testClass;

    public SerializableDIctionary<string, SaveZombieData> zombieData;







    public Database()
    {
        count = 0;
        zombieData = new();
    }
}
