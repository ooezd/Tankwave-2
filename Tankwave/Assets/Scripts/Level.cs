using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public int levelNumber;
    public int highScore;
    public bool isLocked;

    public Level(int _levelNumber, int _highScore)
    {
        levelNumber = _levelNumber+1;
        highScore = _highScore;
        if (PlayerPrefs.GetInt("OpenedLevels") < 1)
        {
            PlayerPrefs.SetInt("OpenedLevels", 1);
            PlayerPrefs.Save();
        }
        int _openedLevels = PlayerPrefs.GetInt("OpenedLevels");
        if (levelNumber> _openedLevels) isLocked = true;
        else isLocked = false;

    }
}
