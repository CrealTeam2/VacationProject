using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;

public class GameManager : MonoBehaviour, ISavable
{
    public Transform startTransform;
    public List<Transform> savePoints = new List<Transform>();
    Dictionary<Vector3, bool> savePointsDict = new Dictionary<Vector3, bool>();
    Vector3 currentSavePoint;
    GameObject player;

    private void Awake()
    {
        InitSavePoint();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.transform.position = currentSavePoint;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckIfOnSavePoint();
    }

    void InitSavePoint()
    {
        foreach (Transform t in savePoints)
        {
            savePointsDict.Add(t.position, false);
        }
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
    }

    public void LoadData(Database data)
    {
        foreach(KeyValuePair<Vector3, bool> item in data.savePointsDict)
        {
            if (!savePointsDict.ContainsKey(item.Key))
            {
                data.savePointsDict.Remove(item.Key);
            }
            savePointsDict[item.Key] = item.Value;
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
}
