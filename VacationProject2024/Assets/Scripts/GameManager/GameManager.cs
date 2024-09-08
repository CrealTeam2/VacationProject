using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Rendering;

public class GameManager : Singleton<GameManager>, ISavable, ISingletonStart
{
    Transform startTransform;
    Transform endTransform;
    List<Transform> savePoints;
    Dictionary<Vector3, bool> savePointsDict;
    List<Vector3> removeList;
    Vector3 currentSavePoint;
    GameObject player;
    public Action onGameOver;
    public Volume globalVolume;

    private void Awake()
    {
        Init();
        Application.targetFrameRate = 60;

    }
    void Init()
    {
        savePoints = new();
        savePointsDict = new();
        removeList = new();
        

    }

    public void IStart()
    {
        startTransform = GameObject.FindWithTag("StartPosition").transform;
        endTransform = GameObject.FindWithTag("EndPosition").transform;
        var gameObjects = GameObject.FindGameObjectsWithTag("SavePoints");
        savePoints.Clear();
        foreach (GameObject t in gameObjects)
            savePoints.Add(t.transform);

        savePointsDict.Clear();
        foreach (Transform t in savePoints)
        {
            savePointsDict.Add(t.position, false);
        }

        player = GameObject.FindWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckIfOnSavePoint();
    }

    void CheckIfOnSavePoint()
    {
        foreach (Vector3 key in savePointsDict.Keys)
        {
            if(Mathf.Sqrt(Mathf.Pow(player.transform.position.x - key.x, 2) + Mathf.Pow(player.transform.position.z - key.z, 2)) < 1
                && Mathf.Abs(player.transform.position.y - key.y) < 5 && savePointsDict[key] == false)
            {
                currentSavePoint = key;
                savePointsDict[key] = true;
                DataManager.Instance.SaveGame();
                return;
            }
        }

        if(Mathf.Sqrt(Mathf.Pow(player.transform.position.x - endTransform.position.x, 2) + Mathf.Pow(player.transform.position.z - endTransform.position.z, 2)) < 1
                && Mathf.Abs(player.transform.position.y - endTransform.position.y) < 5)
        {
            DataManager.Instance.ResetData();
            SceneManager.LoadScene("RyuScene2");
        }
    }

    public void LoadData(Database data)
    {
        foreach(KeyValuePair<Vector3, bool> item in data.savePointsDict)
        {
            if (!savePointsDict.ContainsKey(item.Key))
            {
                removeList.Add(item.Key);
            }
            savePointsDict[item.Key] = item.Value;
        }
        while(removeList.Count > 0)
        {
            data.savePointsDict.Remove(removeList[0]);
            removeList.RemoveAt(0);
        }
        if (savePointsDict.ContainsKey(data.savePoint))
            currentSavePoint = data.savePoint;
        else currentSavePoint = startTransform.position;
    }

    public void SaveData(ref Database data)
    {
        foreach (KeyValuePair<Vector3, bool> item in savePointsDict)
        {
            data.savePointsDict[item.Key] = item.Value;
        }
        data.savePoint = currentSavePoint;
    }
    public void GameOver()
    {
        onGameOver?.Invoke();
    }
}
