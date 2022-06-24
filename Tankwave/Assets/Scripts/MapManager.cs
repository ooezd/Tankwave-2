using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Singleton
    public static MapManager instance;
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

    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private GameObject steelPrefab;
    [SerializeField] private GameObject spacePrefab;
    [SerializeField] private GameObject tankPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float cellSize;
    
    private int width;
    private int height;
    private int[,] mapMatrix;
    private int[] enemyCounts;
    List<int> enemyList = new List<int>();
    List<Vector2> baseSquarePositions = new List<Vector2>();
    List<Vector2> enemySpawnerPositions = new List<Vector2>();
    Vector2 playerSpawnerPosition;

    private void Start()
    {
        CreateGrid(GetLevelData(CSVReader.instance.currentLevel.levelNumber-1));
    }
    public void CreateGrid(Map _map)
    {
        this.width = (int)_map.mapSize.x;
        this.height = (int)_map.mapSize.y;
        this.mapMatrix = _map.mapMatrix;
        this.enemyCounts = _map.enemyTankCounts;
        for (int i = 0; i < _map.enemyTankCounts.Length; i++)
        {
            for (int j = 0; j < _map.enemyTankCounts[i]; j++)
            {
                enemyList.Add(i);
            }
        }
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                switch (mapMatrix[x, y])
                {
                    case 0:
                        break;
                    case 101:
                        SpawnWood(GetWorldPosition(x, y),1);
                        break;
                    case 102:
                        SpawnWood(GetWorldPosition(x, y),2);
                        break;
                    case 103:
                        SpawnWood(GetWorldPosition(x, y),3);
                        break;
                    case 2:
                        SpawnSteel(GetWorldPosition(x, y));
                        break;
                    case 3:
                        baseSquarePositions.Add(GetWorldPosition(x, y));
                        break;
                    case 4:
                        enemySpawnerPositions.Add(GetWorldPosition(x, y));
                        break;
                    case 5:
                        playerSpawnerPosition = GetWorldPosition(x, y);
                        break;
                    default:
                        Debug.Log("Unexpected map item id!");
                        break;
                }
            }
        }

        //spawn one base at _baseSquare's center position
        //use enemySpawnerPositions in EnemySpawner
        //spawn player at playerSpawnerPosition

        SetCameraPosition();
        SpawnPlayer();
        CreateBase();
        CreatePeripheralCollider();
        StartCoroutine(PowerUpSpawnerCo());
    }
    private void SpawnPlayer()
    {
        Instantiate(tankPrefab, playerSpawnerPosition, Quaternion.identity);
    }
    private void SetCameraPosition()
    {
        Camera.main.transform.position = (Vector3)(GetWorldPosition(0, 0) + GetWorldPosition(width - 1, height - 1)) / 2 + new Vector3(0, 0, -10);
        Camera.main.orthographicSize = Vector2.Distance(GetWorldPosition(0, 0), GetWorldPosition(0, width - 1)) / 1.5f;
        GameUIManager.instance.CentralizeBackground(GetCenterPosition());
    }
    private void CreatePeripheralCollider()
    {
        Vector2 _corner1 = GetWorldPosition(0, 0) + new Vector2(-cellSize / 2, cellSize / 2);
        Vector2 _corner2 = GetWorldPosition(0, 7) + new Vector2(-cellSize / 2, -cellSize / 2);
        Vector2 _corner3 = GetWorldPosition(7, 7) + new Vector2(cellSize / 2, -cellSize / 2);
        Vector2 _corner4 = GetWorldPosition(7, 0) + new Vector2(cellSize / 2, cellSize / 2);

        EdgeCollider2D _edge1 = Instantiate(new GameObject(), Vector2.zero, Quaternion.identity).AddComponent<EdgeCollider2D>();
        _edge1.points = new Vector2[] { _corner1, _corner2 };
        EdgeCollider2D _edge2 = Instantiate(new GameObject(), Vector2.zero, Quaternion.identity).AddComponent<EdgeCollider2D>();
        _edge2.points = new Vector2[] { _corner2, _corner3 };
        EdgeCollider2D _edge3 = Instantiate(new GameObject(), Vector2.zero, Quaternion.identity).AddComponent<EdgeCollider2D>();
        _edge3.points = new Vector2[] { _corner3, _corner4 };
        EdgeCollider2D _edge4 = Instantiate(new GameObject(), Vector2.zero, Quaternion.identity).AddComponent<EdgeCollider2D>();
        _edge4.points = new Vector2[] { _corner4, _corner1 };
    }
    private void SpawnWood(Vector2 _position, int _hitpoint)
    {
        GameObject a = Instantiate(woodPrefab,_position, Quaternion.identity);
        a.GetComponent<Wood>().Initialize(_hitpoint);
    }
    private void SpawnSteel(Vector2 _position)
    {
        GameObject a = Instantiate(steelPrefab, _position, Quaternion.identity);
    }
    private void CreateBase()
    {
        Vector2 _centerPosition = Vector2.zero;
        foreach (Vector2 _basePosition in baseSquarePositions)
        {
            _centerPosition += _basePosition;
        }
        _centerPosition /= baseSquarePositions.Count;

        Instantiate(basePrefab, _centerPosition, Quaternion.identity);
    }
    IEnumerator PowerUpSpawnerCo()
    {
        yield return new WaitForSeconds(5 + Random.Range(0, 20));
        SpawnPowerUp();
        yield return new WaitForSeconds(5 + Random.Range(0, 15));
        SpawnPowerUp();
        yield return new WaitForSeconds(5 + Random.Range(0, 10));
        SpawnPowerUp();
    }
    private void SpawnPowerUp()
    {
        int _randomX = Random.Range(0, width);
        int _randomY = Random.Range(0, height);
        float _randomAddX = Random.Range(-cellSize / 2, cellSize / 2);
        float _randomAddY = Random.Range(-cellSize / 2, cellSize / 2);
        Vector2 _finalPosition = GetWorldPosition(_randomX, _randomY) + new Vector2(_randomAddX,_randomAddY);
        Instantiate(powerUpPrefab, _finalPosition, Quaternion.identity);
    }
    public Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2(x, -y) * cellSize;
    }
    private Map GetLevelData(int _levelNumber)
    {
        return FindObjectOfType<CSVReader>().ReadLevel(_levelNumber);
    }
    private Vector2 GetCenterPosition()
    {
        return (GetWorldPosition(0,0) + GetWorldPosition(width-1,height-1))/ 2;
    }
    public Vector2 GetSize()
    {
        return new Vector2(width, height);
    }
    public GameObject SpawnEnemy()
    {
        GameObject _enemy = null;
        
        if (enemyList.Count > 0)
        {
            int _randomIndex = Random.Range(0, enemyList.Count);
            int _randomTank = enemyList[_randomIndex];
            enemyList.RemoveAt(_randomIndex);

            _enemy = Instantiate(enemyPrefab, enemySpawnerPositions[Random.Range(0, enemySpawnerPositions.Count)], Quaternion.identity);
            _enemy.GetComponent<EnemyTank>().Initialize(Container.instance.tankCards[_randomTank + 3]);
        }
        
        return _enemy;
    }
    public bool CheckAllDead()
    {
        return enemyList.Count == 0;
    }
}
