using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image fadeImage;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI winLoseText;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI newHighText;
    [SerializeField] private CanvasGroup buttons;
    [SerializeField] private Button nextLevelButton;

    public void Initialize(bool _isWin)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        highScoreText.text = "High score: " + PlayerPrefs.GetInt($"HighScore{CSVReader.instance.currentLevel.levelNumber}").ToString();
        levelText.text = "Level " + CSVReader.instance.currentLevel.levelNumber;
        backgroundImage.DOFade(.5f, 1f);
        fadeImage.DOFade(1, 1f).OnComplete(
            ()=> {
                if (_isWin) WinText();
                else LoseText();
            });
        levelText.DOFade(1, 1f);
        highScoreText.DOFade(1, 1f);
        
    }
    public void WinText()
    {
        winLoseText.text = "YOU WON";
        winLoseText.color = new Color(0, 220, 0, 0);
        winLoseText.DOFade(1, .3f).SetLoops(5, LoopType.Yoyo).SetEase(Ease.InOutBounce).OnComplete(()=>PointText());
    }
    public void LoseText()
    {
        winLoseText.text = "YOU LOST";
        winLoseText.color = new Color(220, 0, 0, 0);
        winLoseText.DOFade(1, 3).OnComplete(() => ShowButtons());
    }
    public void PointText()
    {
        pointText.DOFade(1, .5f);
        float point = 0;
        float target = PointManager.instance.GetPoint();
        DOTween.To(
            x => point = x,
            point,
            target,
            5).OnUpdate(() => { pointText.text = ((int)point).ToString(); }).SetEase(Ease.OutExpo).OnComplete(()=>CheckHighScore());
    }
    private void CheckHighScore()
    {
        if (PointManager.instance.isHighScore())
        {
            newHighText.gameObject.SetActive(true);
            newHighText.DOFade(1, .5f);
            newHighText.transform.DOPunchScale(new Vector3(.1f, .1f), 2, 10, 1).OnComplete(()=>ShowButtons());
        }
        else
        {
            ShowButtons();
        }
    }
    private void ShowButtons()
    {
        int _levelNumber = CSVReader.instance.currentLevel.levelNumber;
        if (PlayerPrefs.GetInt("OpenedLevels") >= _levelNumber + 1 && CSVReader.instance.levelInfos.Length >= _levelNumber + 1)
        {
            nextLevelButton.interactable = true;
        }
        else
        {
            nextLevelButton.interactable = false;
        }
        buttons.DOFade(1, 1).OnComplete(() => buttons.interactable = true);
    }

    public void RestartLevel()
    {
        SceneLoader.instance.LoadLevelScene();
    }
    public void MainMenu()
    {
        SceneLoader.instance.LoadMenuScene();
    }
    public void NextLevel()
    {
        int _openedLevels = PlayerPrefs.GetInt("OpenedLevels");
        int _currentLevel = CSVReader.instance.currentLevel.levelNumber;
        if (_openedLevels >= _currentLevel + 1)
        {
            CSVReader.instance.currentLevel = CSVReader.instance.levelInfos[_currentLevel];
            SceneLoader.instance.LoadLevelScene();
        }
    }
}
