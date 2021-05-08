using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this script is for the mini boss enemy
 * enemy game object needs two game objects attached to it for wall and edge detection
 * there has to be a layer for the ground or whatever the characters are standing on
 * there has to be a layer for the player character
*/
public class MiniBossEnemy : MonoBehaviour
{
    // sprite handling
    [Header("For Flipping Sprite")]
    [SerializeField] private bool _moveRight = true;
    [SerializeField] private float _localScaleX;
    [SerializeField] private float _localScaleY;
    [SerializeField] private float _localScaleZ;

    // wall detection
    [Header("Wall Checks")]
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private LayerMask _whatIsWall;
    [SerializeField] private bool _hittingWall;

    // edge detection
    [Header("Edge Checks")]
    [SerializeField] private Transform _edgeCheck;
    [SerializeField] private bool _notAtEdge;

    // player detection
    [Header("Player Check")]
    [SerializeField] private Transform _playerCheck;
    [SerializeField] private bool _hitPlayer;
    [SerializeField] private float _playerCheckRadius;
    [SerializeField] private LayerMask _whatIsPlayer;

    // ignore for now
    [Header("Sound")]
    [SerializeField] private AudioClip _attackSound;
    public AudioSource _myAudioSource;

    // components for enemy and what is player
    [HideInInspector] public Rigidbody2D _enemyRb;
    [HideInInspector] public Animator _anim;
    [HideInInspector] public SpriteRenderer _spriteRenderer;
    [HideInInspector] public Transform _player;

    // movement
    [Header("Enemy Move to Player")]
    public float _moveSpeed;
    public float _range;
    public float _stopDistance;

    // attacking player
    [Header("Attack")]
    public float _damageDealt;
    public float _attackSpeed;
    private float _timeBetweenAttacks;
    public GameObject _laser;
    public Transform _firePoint;

    void Start()
    {
        _enemyRb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _myAudioSource = GetComponent<AudioSource>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // checking if _hittingWall, _notAtEdge, _hitPlayer are true or not
        _hittingWall = Physics2D.OverlapCircle(_wallCheck.position, _wallCheckRadius, _whatIsWall);
        _notAtEdge = Physics2D.OverlapCircle(_edgeCheck.position, _wallCheckRadius, _whatIsWall);
        _hitPlayer = Physics2D.OverlapCircle(_playerCheck.position, _playerCheckRadius, _whatIsPlayer);

        // there has to be a player for the enemy to move at all
        if (_player != null && gameObject.GetComponent<Enemy>().Health > 0)
        {
            _anim.SetBool("Idle", false);
            _anim.SetBool("Attack Melee", false);
            _anim.SetBool("Attack Range Move", false);
            _anim.SetBool("Attack Range Idle", false);
            if (_enemyRb.velocity.x > 0.1f || _enemyRb.velocity.x < 0.1f)
            {
                _anim.SetBool("Move", true);
            }
            else
            {
                _anim.SetBool("Move", false);
            }

            // if player is outside of range and not touching a wall or an edge, enemy will patrol normally
            if (Vector2.Distance(transform.position, _player.position) > _range)
            {

                if (_hittingWall || !_notAtEdge)
                {
                    _moveRight = !_moveRight;
                }

                if (_moveRight)
                {
                    transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    _enemyRb.velocity = new Vector2(_moveSpeed, _enemyRb.velocity.y);
                }
                else
                {
                    transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    _enemyRb.velocity = new Vector2(-_moveSpeed, _enemyRb.velocity.y);
                }
            }
            // if player is inside of range, enemy will chase player
            if (Vector2.Distance(transform.position, _player.position) <= _range &&
                    Mathf.Abs(transform.position.x - _player.position.x) > _stopDistance && _notAtEdge)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack Melee", false);
                _anim.SetBool("Attack Range Move", true);
                _anim.SetBool("Attack Range Idle", false);

                // enemy to the right of player
                if (transform.position.x > _player.position.x)
                {
                    transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    _enemyRb.velocity = new Vector2(-_moveSpeed, _enemyRb.velocity.y);
                    _moveRight = false;
                }
                // enemy to the left of player
                else if (transform.position.x < _player.position.x)
                {
                    transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    _enemyRb.velocity = new Vector2(_moveSpeed, _enemyRb.velocity.y);
                    _moveRight = true;
                }
            }
            // enemy stops moving if enemy gets too close to player based on _stopDistance
            else if (Mathf.Abs(transform.position.x - _player.position.x) < _stopDistance && Vector2.Distance(transform.position, _player.position) <= _range)
            {
                _anim.SetBool("Attack Melee", true);
                _anim.SetBool("Move", false);
                _anim.SetBool("Idle", false);
                _anim.SetBool("Attack Range Idle", false);
                _anim.SetBool("Attack Range Move", false);

                _enemyRb.velocity = new Vector2(0, 0);

                // enemy to the right of player
                if (transform.position.x > _player.position.x)
                {
                    transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    _moveRight = false;
                }
                // enemy to the left of player
                else if (transform.position.x < _player.position.x)
                {
                    transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    _moveRight = true;
                }
            }

            /* this is when the player is still in range but there is a gap or a wall in the way
             * enemy will stop moving but will still face the player until player is out of range
            */
            if (Vector2.Distance(transform.position, _player.position) <= _range && !_notAtEdge)
            {
                _enemyRb.velocity = new Vector2(0, 0);

                // enemy to the right of player
                if (transform.position.x > _player.position.x)
                {
                    transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    _moveRight = true;

                    // might change this to compare _hitPlayer also
                    if (_player.position.x < _edgeCheck.position.x)
                    {
                        _anim.SetBool("Idle", false);
                        _anim.SetBool("Move", false);
                        _anim.SetBool("Attack Melee", false);
                        _anim.SetBool("Attack Range Idle", true);
                        _anim.SetBool("Attack Range Move", false);
                    }
                    else
                    {
                        _anim.SetBool("Idle", false);
                        _anim.SetBool("Move", false);
                        _anim.SetBool("Attack Melee", true);
                        _anim.SetBool("Attack Range Idle", false);
                        _anim.SetBool("Attack Range Move", false);
                    }
                }
                // enemy to the left of player
                else if (transform.position.x < _player.position.x)
                {
                    transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    _moveRight = false;

                    if (_player.position.x > _edgeCheck.position.x)
                    {
                        _anim.SetBool("Idle", false);
                        _anim.SetBool("Move", false);
                        _anim.SetBool("Attack Melee", false);
                        _anim.SetBool("Attack Range Idle", true);
                        _anim.SetBool("Attack Range Move", false);
                    }
                    else
                    {
                        _anim.SetBool("Idle", false);
                        _anim.SetBool("Move", false);
                        _anim.SetBool("Attack Melee", true);
                        _anim.SetBool("Attack Range Idle", false);
                        _anim.SetBool("Attack Range Move", false);
                    }
                }
            }
        }
        // if enemy has no health left it dies
        else if (gameObject.GetComponent<Enemy>().Health <= 0)
        {
            _hitPlayer = false;
            _anim.SetBool("Death", true);
            _enemyRb.velocity = new Vector2(0, 0);
            Destroy(gameObject, 1);
        }

        // enemy attacking player (melee)
        if (_hitPlayer && Mathf.Abs(transform.position.x - _player.position.x) <= _stopDistance)
        {
            // setting wait amount
            if (_timeBetweenAttacks <= 0f)
            {
                _timeBetweenAttacks = _attackSpeed;
            }

            // waiting 
            if (_timeBetweenAttacks > 0f)
            {
                _timeBetweenAttacks -= Time.deltaTime;

                // attack
                if (_timeBetweenAttacks <= 0f)
                {
                    _anim.SetBool("Attack Melee", true);
                    GameObject.Find("Player").GetComponent<Player>().TakeDamage(_damageDealt);
                    _timeBetweenAttacks = 0f;
                }
            }
        }
        else if (!_hitPlayer)
        {
            _anim.SetBool("Attack Melee", false);
        }

        // enemy attacking player (range)
        if (_hitPlayer && Mathf.Abs(transform.position.x - _player.position.x) > _stopDistance && _notAtEdge == true)
        {
            // setting wait amount
            if (_timeBetweenAttacks <= 0f)
            {
                _timeBetweenAttacks = _attackSpeed;
            }

            // waiting 
            if (_timeBetweenAttacks > 0f)
            {
                _timeBetweenAttacks -= Time.deltaTime;

                // attack
                if (_timeBetweenAttacks <= 0f)
                {
                    _anim.SetBool("Attack Range Move", true);
                    Instantiate(_laser, _firePoint.position, Quaternion.identity);
                    //GameObject.Find("Player").GetComponent<Player>().TakeDamage(_damageDealt);
                    _timeBetweenAttacks = 0f;
                }
            }
        }
        else if (!_hitPlayer)
        {
            _anim.SetBool("Attack Range Move", false);
        }

        if (_hitPlayer && Mathf.Abs(transform.position.x - _player.position.x) > _stopDistance && _notAtEdge == false)
        {
            // setting wait amount
            if (_timeBetweenAttacks <= 0f)
            {
                _timeBetweenAttacks = _attackSpeed;
            }

            // waiting 
            if (_timeBetweenAttacks > 0f)
            {
                _timeBetweenAttacks -= Time.deltaTime;

                // attack
                if (_timeBetweenAttacks <= 0f)
                {
                    _anim.SetBool("Attack Range Idle", true);
                    Instantiate(_laser, _firePoint.position, Quaternion.identity);
                    //GameObject.Find("Player").GetComponent<Player>().TakeDamage(_damageDealt);
                    _timeBetweenAttacks = 0f;
                }
            }
        }
        else if (!_hitPlayer)
        {
            _anim.SetBool("Attack Range Idle", false);
        }
    }
}

