using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tank Card", menuName = "Tanks")]
public class TankCard : ScriptableObject
{
    public int tankId;
    public int speed;
    public int bulletSpeed;
    public int fireRate;
    public int health;
    public int bodyId;
    public int trackId;
    public int gunId;
    public Color color;
    public int worthPoints;
}
