using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    #region Singleton
    public static PointManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }
    #endregion

    private int currentPoint;
    private int highScore;

    void Start()
    {
        currentPoint = 0;
        highScore = PlayerPrefs.GetInt($"HighScore{CSVReader.instance.currentLevel.levelNumber}");
    }
    public void AddPoints(int _amount)
    {
        currentPoint += _amount;
    }
    public int GetPoint()
    {
        return currentPoint;
    }
    public void AddTimePoint(float _time)
    {
        AddPoints((int)(5000f / _time));
    }
    public bool isHighScore()
    {
        if (currentPoint > highScore)
        {
            PlayerPrefs.SetInt($"HighScore{CSVReader.instance.currentLevel.levelNumber}",currentPoint);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
}
