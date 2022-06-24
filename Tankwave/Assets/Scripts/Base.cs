using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    private int health = 5;

    public void GetDamage()
    {
        if (--health == 0)
        {
            GameManager.instance.BaseDown();
            Destroy(gameObject);
        }
        else
        {
            PointManager.instance.AddPoints(-200);
        }
    }
}
