using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this script is for the range enemy
 * enemy game object needs four game objects attached to it for wall and edge detection (back and front)
 * there has to be a layer for the ground or whatever the characters are standing on
 * there has to be a layer for the player character
*/
public class PatrolEnemyRange : MonoBehaviour
{
    [Header("For Flipping Sprite")]
    [SerializeField] private bool _moveRight = true;
    [SerializeField] private float _localScaleX;
    [SerializeField] private float _localScaleY;
    [SerializeField] private float _localScaleZ;


    [Header("Wall Checks")]
    [SerializeField] private Transform _wallCheckFront;
    [SerializeField] private Transform _wallCheckBack;
    [SerializeField] private float _wallCheckRadius;
    [SerializeField] private LayerMask _whatIsWall;
    [SerializeField] private bool _hittingWallFront;
    [SerializeField] private bool _hittingWallBack;


    [Header("Edge Checks")]
    [SerializeField] private Transform _edgeCheckFront;
    [SerializeField] private Transform _edgeCheckBack;
    [SerializeField] private bool _notAtEdgeFront;
    [SerializeField] private bool _notAtEdgeBack;


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


    [Header("Enemy Move to Player")]
    public float _moveSpeed;
    public float _farRange;
    public float _middleRange;
    public float _shortRange;
    public float _stopDistance;

    // ignore for now
    [Header("Attack")]
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
        _hittingWallFront = Physics2D.OverlapCircle(_wallCheckFront.position, _wallCheckRadius, _whatIsWall);
        _hittingWallBack = Physics2D.OverlapCircle(_wallCheckBack.position, _wallCheckRadius, _whatIsWall);
        _notAtEdgeFront = Physics2D.OverlapCircle(_edgeCheckFront.position, _wallCheckRadius, _whatIsWall);
        _notAtEdgeBack = Physics2D.OverlapCircle(_edgeCheckBack.position, _wallCheckRadius, _whatIsWall);

        _hitPlayer = Physics2D.OverlapCircle(_playerCheck.position, _playerCheckRadius, _whatIsPlayer);

        // there has to be a player for the enemy to move at all
        if (_player != null && gameObject.GetComponent<Enemy>().Health > 0)
        {
            _anim.SetBool("Idle", false);
            _anim.SetBool("Attack", false);
            _anim.SetBool("Move", true);

            // if player is outside of range and not touching a wall or an edge, enemy will patrol normally
            if (Vector2.Distance(transform.position, _player.position) > _farRange)
            {
                if (_hittingWallFront || !_notAtEdgeFront)
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
            if (Vector2.Distance(transform.position, _player.position) <= _farRange &&
                Mathf.Abs(transform.position.x - _player.position.x) > _stopDistance &&
                Vector2.Distance(transform.position, _player.position) > _middleRange && _notAtEdgeFront)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack", true);

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.position.x, transform.position.y, transform.position.z), _moveSpeed * Time.deltaTime);

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
            // enemy stays still if it's in middle range
            else if (Vector2.Distance(transform.position, _player.position) < _middleRange &&
                Vector2.Distance(transform.position, _player.position) > _shortRange)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack", true);

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
            // enemy moves away if enemy gets too close to player based on _middleRange
            else if (Vector2.Distance(transform.position, _player.position) < _shortRange && _notAtEdgeBack)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack", true);

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(_player.position.x, transform.position.y, transform.position.z), -1 * _moveSpeed * Time.deltaTime);

                // enemy to the right of player
                if (transform.position.x > _player.position.x)
                {
                    transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    _enemyRb.velocity = new Vector2(_moveSpeed, _enemyRb.velocity.y);
                    _moveRight = false;
                }
                // enemy to the left of player
                else if (transform.position.x < _player.position.x)
                {
                    transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    _enemyRb.velocity = new Vector2(-_moveSpeed, _enemyRb.velocity.y);
                    _moveRight = true;
                }
            }
            // if enemy reach a wall or an edge while in short range, enemy stops
            else if (Vector2.Distance(transform.position, _player.position) < _shortRange && !_notAtEdgeBack)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack", true);

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
            if (Vector2.Distance(transform.position, _player.position) <= _farRange && !_notAtEdgeFront)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack", true);

                _enemyRb.velocity = new Vector2(0, 0);

                // enemy to the right of player
                if (transform.position.x > _player.position.x)
                {
                    transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    _moveRight = true;
                }
                // enemy to the left of player
                else if (transform.position.x < _player.position.x)
                {
                    transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    _moveRight = false;
                }
            }
        }
        else if (gameObject.GetComponent<Enemy>().Health <= 0)
        {
            _hitPlayer = false;
            _anim.SetBool("Death", true);
            _enemyRb.velocity = new Vector2(0, 0);
            Destroy(gameObject, 1);
        }

        // enemy attacking player
        if (_hitPlayer)
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
                    _anim.SetBool("Attack", true);
                    Instantiate(_laser, _firePoint.position, Quaternion.identity);
                    //GameObject.Find("Player").GetComponent<Player>().TakeDamage(_damageDealt);
                    _timeBetweenAttacks = 0f;
                }
            }
        }
        else if (!_hitPlayer)
        {
            _anim.SetBool("Attack", false);
        }
    }
}

