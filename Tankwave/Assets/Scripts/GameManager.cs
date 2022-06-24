using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager instance;
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
    //start the game after initialize the map, player and two enemies
    //isPlaying = false; 
    //when an enemy dies, check enemy numbers if = 0 win
    //when the base is down, or the player dies lose the game
    //spawn enemies randomly

    public bool isPlaying = false;
    public float timer = 0;
    private float lastEnemySpawn;
    private List<GameObject> aliveEnemies = new List<GameObject>();
    Coroutine enemySpawner;

    private void Start()
    {
        StartGame();
    }
    private void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;
        }
    }
    public void StartGame()
    {
        isPlaying = true;
        enemySpawner = StartCoroutine(EnemySpawnerCo());
    }
    IEnumerator EnemySpawnerCo()
    {
        //check last spawn time, alive enemy count, then spawn if conditions are met
        if ((Time.time - lastEnemySpawn) > 4 || timer < 1)
        {
            if (aliveEnemies.Count < 5)
            {
                GameObject _enemyObject = MapManager.instance.SpawnEnemy();
                if (_enemyObject != null)
                {
                    aliveEnemies.Add(_enemyObject);
                    lastEnemySpawn = Time.time;

                }
            }
        }
        yield return new WaitForSeconds(4);
        enemySpawner = StartCoroutine(EnemySpawnerCo());
    }
    public void Won()
    {
        PauseGame();
        int _openedLevels = PlayerPrefs.GetInt("OpenedLevels");
        Debug.Log($"Op: {_openedLevels} Lv: {CSVReader.instance.currentLevel.levelNumber}");
        if (_openedLevels == 0) _openedLevels = 1;
        if (_openedLevels == CSVReader.instance.currentLevel.levelNumber)
        {
            PlayerPrefs.SetInt("OpenedLevels", (_openedLevels+1));
            PlayerPrefs.Save();
        }
        GameUIManager.instance.Win();
        PointManager.instance.AddTimePoint(timer);

    }
    public void BaseDown()
    {
        PauseGame();
        GameUIManager.instance.Lose();
    }
    public void EnemyDown(GameObject _enemyObject)
    {
        aliveEnemies.Remove(_enemyObject);
        Debug.Log("enemy down, current enemy count: " + aliveEnemies.Count);
        if (MapManager.instance.CheckAllDead() && aliveEnemies.Count == 0)
        {
            Won();
        }
    }
    public void PlayerDown()
    {
        PauseGame();
        GameUIManager.instance.Lose();
    }
    private void PauseGame()
    {
        isPlaying = false;
        foreach (Projectile _projectileObject in FindObjectsOfType<Projectile>())
        {
            Destroy(_projectileObject);
        }
        foreach (Tank _tank in FindObjectsOfType<Tank>())
        {
            _tank.Freeze();
        }
        StopCoroutine(enemySpawner);
    }
}
