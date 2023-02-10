using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] Map map;
    public int Level { get; private set; } = 1;
    public int MaxLevel { get; private set; } = 2;


    private void Start()
    {
        Application.targetFrameRate = 30;
        map.GenerateNewMap(Level);
    }

    public void LoadNextLevel()
    {
        Level++;
        map.GenerateNewMap(Level);
    }

    public void LoadNewGame()
    {
        Level = 1;
        map.GenerateNewMap(Level);
    }
}
