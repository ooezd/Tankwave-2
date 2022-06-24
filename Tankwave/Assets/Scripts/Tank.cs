using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tank : MonoBehaviour
{
    protected int tankId;
    protected int speed;
    protected int bulletSpeed;
    protected int fireRate;
    protected int health;
    protected int worthPoints;
    protected bool canFire = true;
    [SerializeField] protected SpriteRenderer bodyImage;
    [SerializeField] protected SpriteRenderer[] trackImages;
    [SerializeField] protected SpriteRenderer gunImage;
    public GameObject projectilePrefab;


    public virtual void Fire()
    {
        if (canFire && health > 0)
        {
            StartCoroutine(FireRateCo());
            GameObject projectileObject = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectileObject.GetComponent<Projectile>().Initialize(bulletSpeed, transform.up, gameObject.tag);
        }
        
    }
    IEnumerator FireRateCo()
    {
        canFire = false;
        yield return new WaitForSeconds((float)1 / (float)fireRate);
        canFire = true;
    }
    public virtual void Initialize(TankCard _tankCard)
    {
        tankId = _tankCard.tankId;
        speed = 1 +_tankCard.speed /5;
        bulletSpeed = _tankCard.bulletSpeed;
        fireRate = _tankCard.fireRate;
        health = _tankCard.health;
        worthPoints = _tankCard.worthPoints;
        bodyImage.sprite = Container.instance.bodies[_tankCard.bodyId];
        trackImages[0].sprite = Container.instance.tracks[_tankCard.trackId];
        trackImages[1].sprite = Container.instance.tracks[_tankCard.trackId];
        gunImage.sprite = Container.instance.guns[_tankCard.gunId];

        SpriteRenderer _sr = GetComponentInChildren<SpriteRenderer>();
        _sr.color = _tankCard.color;
        bodyImage.color = _tankCard.color;

    }
    public virtual void GetDamage()
    {
        health--;
    }
    public void Freeze()
    {
        GetComponent<Rigidbody2D>().simulated=false;
        canFire = false;
        StopAllCoroutines();
    }
}
