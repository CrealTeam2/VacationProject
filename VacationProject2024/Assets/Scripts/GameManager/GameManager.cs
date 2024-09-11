using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject player;
    public Action onGameOver;
    public Volume globalVolume;
    public Image fadeImage;
    [SerializeField] GameObject gameEnd;

    public bool ElectorcitySupply;
    public delegate void OnGeneratorOn();
    public OnGeneratorOn onGeneratorOn;

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
        fadeImage = GameObject.Find("Canvas").transform.Find("Fade").GetComponent<Image>();

        SoundManager.Instance.PlayBGM("MainBGM", 0.4f, 55);
        fadeImage.gameObject.SetActive(true);
        fadeImage.color = new Color(0, 0, 0, 0.8f);
    }

    public IEnumerator StartGame()
    {
        fadeImage.gameObject.SetActive(true);
        for (int i = 204; i > 0; i--)
        {
            fadeImage.color = new Color(0, 0, 0, i/255f);
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);
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
            SceneManager.LoadScene("InGameMap_RYU3");
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
    public IEnumerator GameOver()
    {
        onGameOver?.Invoke();
        fadeImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 100; i++)
        {
            fadeImage.color = new Color(0, 0, 0, 0.01f * i);
            yield return null;
        }
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("InGameMap");
    }
    public IEnumerator GameWin()
    {
        onGameOver?.Invoke();
        fadeImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < 100; i++)
        {
            fadeImage.color = new Color(0, 0, 0, 0.01f * i);
            yield return null;
        }
        gameEnd.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("InGameMap");
        Destroy(gameObject);
    }
}
