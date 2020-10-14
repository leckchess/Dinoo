using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance;
    public Level CurrentLevel { get; set; }
    public Level[] levels;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    public void LoadLevels(UnityAction callback)
    {
        levels = Resources.LoadAll<Level>("Levels");
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].ID = i;
        }
        callback.Invoke();
    }
}
