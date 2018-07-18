using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ConstantManager : MonoBehaviour {

    public static ConstantManager Manager;
    private int difficulty;
    
    [Serializable]
    class GameData
    {
        public int progress;
    }

    private int progress = 0;

    public int GetProgress()
    {
        return progress;
    }

    public int GetDifficulty()
    {
        return difficulty;
    }

    public void SetProgress(int newprog)
    {
        if (newprog > progress)
        {
            Debug.Log("Progress from " + progress + " to now " + newprog);
            progress = newprog;
            Save();
        }
        else
           Debug.Log("Something Wrong? tried to set prog to " + newprog + " when current prog is " + progress);
    }


    void Awake()
    {
        if (Manager == null)
        {
            DontDestroyOnLoad(gameObject);
            Manager = this;
        }
        else if (Manager != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        difficulty = 10;
        Debug.Log("Loading");
        Load();
    }

	public void StartGame (int selected_difficulty)
    {
        //StartCoroutine(StartEnum(selected_difficulty));
        difficulty = selected_difficulty;
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

    }
    
    public void SelectionScreen(bool FirstTime)
    {
        if (FirstTime)
            playSong();
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Single);
    }

    public void playSong()
    {
        gameObject.GetComponent<AudioSource>().Play(0);
        Invoke("playSong", gameObject.GetComponent<AudioSource>().clip.length);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");

        GameData data = new GameData();
        data.progress = progress;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            GameData data = (GameData)bf.Deserialize(file);
            file.Close();
            Debug.Log("Loaded Successfully from " + Application.persistentDataPath + "/gameInfo.dat");
            progress = data.progress;
        }
        else
            Debug.Log("Nothing Loaded");
    }

}
