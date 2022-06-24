using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType { slow, shield };
public class PowerUp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite slowPowerUpImage;
    [SerializeField] private Sprite shieldPowerUpImage;
    private PowerUpType powerUpType;

    void Start()
    {
        powerUpType = Random.Range(0, 2) == 0 ? PowerUpType.slow : PowerUpType.shield;
        Initialize();
    }
    public void Initialize()
    {
        spriteRenderer.sprite = powerUpType == PowerUpType.slow ? slowPowerUpImage : shieldPowerUpImage;
        StartCoroutine(DisappearCo());
    }
    IEnumerator DisappearCo()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
    public void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision player");
            if (powerUpType == PowerUpType.slow)
            {
                foreach (EnemyTank _enemyTank in FindObjectsOfType<EnemyTank>())
                {
                    _enemyTank.TakeSlowPowerUp();
                }
            }
            else
            {
                FindObjectOfType<Player>().Shield(5);
            }
            Destroy(gameObject);
        }
    }
}
