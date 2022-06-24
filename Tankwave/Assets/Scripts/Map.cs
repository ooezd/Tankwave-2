using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public Vector2 mapSize;
    public int[,] mapMatrix;
    public int[] enemyTankCounts;

    public Map(Vector2 _mapSize, int[,] _mapMatrix, int[] _enemyTankCounts)
    {
        this.mapSize = _mapSize;
        this.mapMatrix = _mapMatrix;
        this.enemyTankCounts = _enemyTankCounts;
    }
}
