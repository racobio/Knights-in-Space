using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D _enemyProjectileRb;
    [HideInInspector] public Transform _player;

    public float _damageDealt;
    public float _projectileSpeed;
    private float _xDirection;

    // Start is called before the first frame update
    void Start()
    {
        _enemyProjectileRb = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        // laser points the correct way
        if (transform.position.x < _player.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            _xDirection = 1;
        }
        else if (transform.position.x > _player.position.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
            _xDirection = -1;
        }

        // laser travels
        if (_xDirection == 1)
        {
            _enemyProjectileRb.velocity = transform.right * _projectileSpeed * _xDirection;
        }
        else if (_xDirection == -1)
        {
            _enemyProjectileRb.velocity = transform.right * _projectileSpeed * _xDirection;
        }
    }

    // disappears after 2 seconds if it doesn't hit player
    private void Awake()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // check is trigger on box collider 2d
    // player takes damage if it collides with laser
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<Player>().TakeDamage(_damageDealt);
            Destroy(gameObject);
        }
    }

    // if you don't check is trigger, then you can do this
    // just note that there is some knockback when colliding
    /*
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            GameObject.Find("Player").GetComponent<Player>().TakeDamage(_damageDealt);
            Destroy(gameObject);
        }
    }
    */
}
