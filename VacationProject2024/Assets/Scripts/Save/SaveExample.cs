using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveExample : MonoBehaviour, ISavable
{
    public int count = 0;
    float curTime = 0;

    public TestClass testClass = new TestClass();

    private void Update()
    {
        curTime += Time.deltaTime;
        if (curTime > 1)
        {
            curTime = 0;
            count++;
            testClass.num += 10;
            testClass.name = "ABC" + count.ToString();
        }
    }

    public void LoadData(Database data)
    {
        count = data.count;
        testClass = data.testClass;
    }

    public void SaveData(ref Database data)
    {
        data.count = count;
        data.testClass = testClass;
    }
}

[Serializable]
public class TestClass
{
    public int num;
    public string name;
}
