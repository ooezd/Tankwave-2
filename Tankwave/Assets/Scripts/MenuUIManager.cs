using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class MenuUIManager : MonoBehaviour
{
    #region Singleton
    public static MenuUIManager instance;
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

    [SerializeField] private TextMeshProUGUI welcomeText;
    [SerializeField] private TextMeshProUGUI cybertankText;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject tanksPanel;

    void Start()
    {
        //PlayerPrefs.DeleteAll();
    }
    public void OnLevelsButtonClick()
    {
        levelsPanel.gameObject.SetActive(true);
        buttonsPanel.gameObject.SetActive(false);
    }
    public void OnLevelsCloseButtonClick()
    {
        levelsPanel.gameObject.SetActive(false);
        buttonsPanel.gameObject.SetActive(true);
    }
    public void LevelSelected()
    {
        HideAll();
        tanksPanel.SetActive(true);
    }
    public void TankSelected()
    {
        SceneLoader.instance.LoadLevelScene();
    }
    public void HideAll()
    {
        tanksPanel.SetActive(false);
        buttonsPanel.SetActive(false);
        levelsPanel.SetActive(false);
    }
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }
}
