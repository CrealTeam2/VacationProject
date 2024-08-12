using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Data;
using System;

public class DataManager : Singleton<DataManager>
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public static FileDataHandler fileDataHandler;



    Database data = new Database();
    public List<ISavable> savableObjects = new List<ISavable>();

    private void Start()
    {
        fileDataHandler = new(Application.persistentDataPath, fileName);
        savableObjects = FindAllSavableObjects();
        LoadGame();
    }

    public void InitGame()
    {
        data = new Database();
    }

    public void SaveGame()
    {
        foreach (ISavable savableObject in savableObjects)
        {
            savableObject.SaveData(ref data);
        }

        fileDataHandler.Save(data);
    }

    public void LoadGame() 
    {
        data = fileDataHandler.Load();

        if (data == null)
        {
            Debug.Log("No Data Initialized. Initialzing the data");
            InitGame();
        }

        foreach (ISavable savableObject in savableObjects)
        {
            savableObject.LoadData(data);
        }
        print(data.count);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISavable> FindAllSavableObjects()
    {
        IEnumerable<ISavable> savables = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();
        return new List<ISavable>(savables);
    }
}

public class FileDataHandler
{
    private string dataDirectoryPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirectoryPath, string dataFileName)
    {
        this.dataDirectoryPath = dataDirectoryPath;
        this.dataFileName = dataFileName;
    }

    public Database Load()
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        Database loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<Database>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("Error Occured When Trying to Load Data from File" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(Database data)
    {
        string fullPath = Path.Combine(dataDirectoryPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error Occured When Trying to Save Data to File" + fullPath + "\n" + e);
        }
    }
}
