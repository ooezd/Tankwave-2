using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameUIManager : MonoBehaviour
{
    #region Singleton
    public static GameUIManager instance;
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

    private Player player;
    [SerializeField] private GameObject background;
    [SerializeField] private LevelEnd levelEnd;
    void Start()
    {
        StartCoroutine(FindPlayerCo());
    }
    IEnumerator FindPlayerCo()
    {
        if (FindObjectOfType<Player>() != null)
        {
            player = FindObjectOfType<Player>();
        }
        else
        {
            yield return new WaitForSeconds(.1f);
            StartCoroutine(FindPlayerCo());
        }
    }
    public void OnFireButtonClicked()
    {
        player.Fire();
    }
    public void CentralizeBackground(Vector2 _position)
    {
        background.transform.position = _position;
    }
    public void Win()
    {
        levelEnd.gameObject.SetActive(true);
        levelEnd.Initialize(true);
    }
    public void Lose()
    {
        levelEnd.gameObject.SetActive(true);
        levelEnd.Initialize(false);
    }
}

