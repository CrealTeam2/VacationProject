using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataUnit
{
    public SerializableDictionary<string, int> Int = new();
    public SerializableDictionary<string, string> String = new();
    public SerializableDictionary<string, float> Float = new();
    public SerializableDictionary<string, Vector3> Vector3 = new();
    public SerializableDictionary<string, bool> Bool = new();
}
