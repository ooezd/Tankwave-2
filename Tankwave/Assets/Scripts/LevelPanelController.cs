using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LevelPanelController : MonoBehaviour
{
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private GameObject contentObject;
    [SerializeField] private TextMeshProUGUI warningText;
    private Vector2 defaultWarningAnchorMin;
    private Vector2 defaultWarningAnchorMax;


    private List<GameObject> levelRows = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        defaultWarningAnchorMin = warningText.rectTransform.anchorMin;
        defaultWarningAnchorMax = warningText.rectTransform.anchorMax;
    }
    void OnEnable()
    {
        ClearAll();
        foreach (Level _level in CSVReader.instance.GetLevelDatas())
        {
            GameObject _levelButtonObject = Instantiate(levelButtonPrefab, contentObject.transform);
            LevelButton _levelButton = _levelButtonObject.GetComponent<LevelButton>();
            _levelButton.Initialize(_level);
            levelRows.Add(_levelButtonObject);
        }
    }
    void ClearAll()
    {
        foreach (GameObject _rowObject in levelRows)
        {
            Destroy(_rowObject);
        }
        levelRows.Clear();
    }
    public void LockedLevel(int _levelNumber)
    {
        warningText.text = $"Level {_levelNumber} is locked!";
        warningText.rectTransform.anchorMin = defaultWarningAnchorMin;
        warningText.rectTransform.anchorMax = defaultWarningAnchorMax;
        warningText.DOFade(1, .5f);
        warningText.rectTransform.DOAnchorMin(new Vector2(0.4f, 0.75f), .5f);
        warningText.rectTransform.DOAnchorMax(new Vector2(0.6f, 0.85f), .5f)
            .OnComplete(()=>
            {
                warningText.rectTransform.DOAnchorMin(new Vector2(1f, 0.75f), .5f);
                warningText.rectTransform.DOAnchorMax(new Vector2(1.2f, 0.85f), .5f);
            });
    }
}
