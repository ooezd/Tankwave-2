using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSVReader : MonoBehaviour
{
    #region Singleton
    public static CSVReader instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    #endregion

    public TextAsset[] levelsData;
    public Level currentLevel;
    public TankCard selectedTank;
    public Level[] levelInfos;

    public Map ReadLevel(int _levelNumber)
    {
        //Read desired level's data
        string _levelData = levelsData[_levelNumber].text;
        string[] _rows = _levelData.Split('\n');
        string[] _levelInfo = _rows[0].Split(',');
        Vector2 _mapSize = new Vector2(_rows.Length - 1, _rows[1].Split(',').Length);
        int[] _enemyTankCounts = new int[3]; 

        if (_levelInfo.Length == 3)
        {
            int.TryParse(_levelInfo[0], out int _purpleCount);
            int.TryParse(_levelInfo[1], out int _yellowCount);
            int.TryParse(_levelInfo[2], out int _blackCount);
            _enemyTankCounts[0] = _purpleCount;
            _enemyTankCounts[1] = _yellowCount;
            _enemyTankCounts[2] = _blackCount;
        }
        else
        {
            Debug.Log("Level features couldn't be read from the level file.");
            return null;
        }

        int[,] _mapMatrix = new int[(int)_mapSize.x, (int)_mapSize.y];
        for (int x = 0; x < _mapSize.x; x++)
        {
            string[] _rowInfo = _rows[x + 1].Split(',');
            for (int y = 0; y < _mapSize.y; y++)
            {
                int.TryParse(_rowInfo[y], out _mapMatrix[y, x]);
            }
        }

        return new Map(_mapSize, _mapMatrix, _enemyTankCounts);
    }
    public Level[] GetLevelDatas()
    {
        Level[] _levels = new Level[levelsData.Length];
        for(int i = 0; i < levelsData.Length; i++)
        {
            int _highScore = PlayerPrefs.GetInt($"HighScore{i + 1}", 0);
            _levels[i] = new Level(i, _highScore);
        }
        levelInfos = _levels;
        return _levels;
    }
    public Level GetLevelData(int _levelNumber)
    {
        return levelInfos[_levelNumber - 1];
    }
}
