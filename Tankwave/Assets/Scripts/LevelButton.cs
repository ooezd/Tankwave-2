using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject lockObject;
    [SerializeField] private GameObject highScoreObject;
    private bool isLocked;
    private Level level;
    public void Initialize(Level _level)
    {
        level = _level;
        levelText.text = $"Level {_level.levelNumber}";
        
        if (_level.highScore> 0)
        {
            highScoreObject.gameObject.SetActive(true);
            highScoreText.text = _level.highScore.ToString();
            Debug.Log(_level.highScore);
        }
        else
        {
            highScoreObject.gameObject.SetActive(false);
        }
        if (_level.isLocked) Lock();
        else Unlock();
    }
    private void Lock()
    {
        isLocked = true;
        lockObject.gameObject.SetActive(true);
        highScoreObject.gameObject.SetActive(false);
    }
    private void Unlock()
    {
        isLocked = false;
        lockObject.gameObject.SetActive(false);
        highScoreObject.gameObject.SetActive(true);
    }
    public void OnClick()
    {
        if (isLocked)
        {
            FindObjectOfType<LevelPanelController>().LockedLevel(level.levelNumber);
        }
        else
        {
            CSVReader.instance.currentLevel = level;
            MenuUIManager.instance.LevelSelected();
        }
    }
}
