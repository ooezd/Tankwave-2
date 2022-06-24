using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rbody;
    private string shooterTag;

    public void Initialize(int _speed, Vector2 _direction, string _shooterTag)
    {
        shooterTag = _shooterTag;
        rbody.velocity = _speed * _direction * 2;
        Quaternion _inputRotation = Quaternion.LookRotation(Vector3.forward, _direction);
        rbody.rotation = _inputRotation.eulerAngles.z;
        gameObject.layer = _shooterTag == "Player" ? 6 : 7;

    }
    public void OnCollisionEnter2D(Collision2D _collision)
    {
            
        Destroy(this.gameObject);
        if(_collision.gameObject.CompareTag("Wood"))
        {
            _collision.gameObject.GetComponent<Wood>().GetDamage();
            if (shooterTag == "Player")
            {
                PointManager.instance.AddPoints(25);
            }
        }
        if (!_collision.gameObject.CompareTag(shooterTag))
        {
            if (_collision.gameObject.GetComponent<Tank>() != null)
            {
                _collision.gameObject.GetComponent<Tank>().GetDamage();
            }
        }
        if (_collision.gameObject.tag == "Base")
        {
            _collision.gameObject.GetComponent<Base>().GetDamage();
        }
    }
}
