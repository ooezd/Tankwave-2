using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    private int hitpoint;
    [SerializeField] private SpriteRenderer woodImage;
    public void Initialize(int _hitpoint)
    {
        hitpoint = _hitpoint;
        SetBrightness();
    }

    public void GetDamage()
    {
        if(--hitpoint == 0)
        {
            Destroy(gameObject);
        }
        SetBrightness();
    }
    private void SetBrightness()
    {
        Color _color = woodImage.color;
        woodImage.color = new Color(_color.r, _color.g, _color.b, (float)((float)1 / 4) * (hitpoint + 1));
    }
}
