using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tank
{
    MapManager levelLoader;
    Vector2 mapSize;
    FixedJoystick joystick;
    Rigidbody2D rigidbody2d;
    Vector2 moveInput = Vector2.zero;
    private AudioSource audioSource;
    [SerializeField] private AudioClip laserSound;
    private bool isShielded;

    public override void Initialize(TankCard tankCard)
    {
        base.Initialize(tankCard);
    }

    void Start()
    {
        Initialize(CSVReader.instance.selectedTank);
        levelLoader = FindObjectOfType<MapManager>();
        mapSize = levelLoader.GetSize();
        joystick = FindObjectOfType<FixedJoystick>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {
        HandleInputs();
    }
    void FixedUpdate()
    {
        rigidbody2d.MovePosition(rigidbody2d.position + moveInput * speed * Time.fixedDeltaTime);
    }
    void HandleInputs()
    {
        if (joystick.Direction != Vector2.zero)
        {
            if (Mathf.Abs(joystick.Horizontal) > Mathf.Abs(joystick.Vertical))
            {
                if (joystick.Horizontal > 0) moveInput = Vector2.right;
                else moveInput = Vector2.left;
            }
            else if (Mathf.Abs(joystick.Horizontal) < Mathf.Abs(joystick.Vertical))
            {
                if (joystick.Vertical > 0) moveInput = Vector2.up;
                else moveInput = Vector2.down;
            }
            else
            {
                moveInput = Vector2.zero;
            }
            Quaternion _inputRotation = Quaternion.LookRotation(Vector3.forward, moveInput);
            rigidbody2d.rotation = _inputRotation.eulerAngles.z;
        }
        else
        {
            moveInput = Vector2.zero;
        }
    }
    public override void GetDamage()
    {
        if (!isShielded)
        {
            base.GetDamage();
            if (health <= 0)
            {
                GameManager.instance.PlayerDown();
                Destroy(gameObject);
            }
            else
            {
                PointManager.instance.AddPoints(-worthPoints / 5);
            }
        }
    }
    public override void Fire()
    {
        if (canFire && health > 0)
        {
            audioSource.PlayOneShot(laserSound,.5f);
        }
        base.Fire();
    }
    public void Shield(float _seconds)
    {
        StartCoroutine(ShieldCountdownCo(_seconds));
    }
    IEnumerator ShieldCountdownCo(float _seconds)
    {
        Debug.Log("Player shielded");
        isShielded = true;
        yield return new WaitForSeconds(_seconds);
        isShielded = false;
    }
}

