using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyTank : Tank
{
    private Rigidbody2D rbody;
    private Vector2 moveDirection;
    private float moveTimer;
    private AudioSource audioSource;
    [SerializeField] private AudioClip laserSound;

    public override void Initialize(TankCard tankCard)
    {
        base.Initialize(tankCard);
    }

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        moveDirection = Vector2.down;
        moveTimer = Random.Range(1,5);
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(RandomFireCo());
    }
    void Update()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            TurnRandom();
        }
    }
    void FixedUpdate()
    {
        rbody.MovePosition(rbody.position + moveDirection * speed * Time.fixedDeltaTime);
        RotateToMoveDirection();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Projectile"))
        {
            TurnRandom();
        }
    }
    void TurnRandom()
    {
        int randomNumber = Random.Range(0, 9);
        if (randomNumber % 3 == 0)
            moveDirection = transform.right;
        else if (randomNumber % 3 == 1)
            moveDirection = -transform.right;
        else
            moveDirection = -transform.up;
        moveTimer = Random.Range(1, 5) ;
    }
    void RotateToMoveDirection()
    {
        Quaternion _inputRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        rbody.rotation = _inputRotation.eulerAngles.z;
    }
    public override void GetDamage()
    {
        base.GetDamage();
        if (health <= 0)
        {
            canFire = false;
            GameManager.instance.EnemyDown(gameObject);
            transform.DOShakePosition(.5f, .1f, 100, 0, false, false).OnComplete(() => { Destroy(gameObject); }).SetEase(Ease.Flash);

            PointManager.instance.AddPoints(worthPoints);
        }
        else
        {
            PointManager.instance.AddPoints(worthPoints / 10);
        }
    }
    IEnumerator RandomFireCo()
    {
        yield return new WaitForSeconds(Random.Range(0,2));
        if (canFire && health > 0)
        {
            audioSource.PlayOneShot(laserSound, 0.5f);
        }
        Fire();
        StartCoroutine(RandomFireCo());
    }
    public void TakeSlowPowerUp()
    {
        Debug.Log("EnemySlow");
        speed = 1;
    }
}
