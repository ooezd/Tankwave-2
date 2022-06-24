using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    #region Singleton
    public static Container instance;
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

    [SerializeField] public List<TankCard> tankCards;

    [SerializeField] public List<Sprite> tracks;
    [SerializeField] public List<Sprite> bodies;
    [SerializeField] public List<Sprite> guns;
    [SerializeField] public List<Sprite> tankImages;

}
