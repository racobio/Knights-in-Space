using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* this script is for the range enemy
 * enemy game object needs four game objects attached to it for wall and edge detection
 * there has to be a layer for the ground or whatever the characters are standing on
 * there has to be a layer for the player character
*/
public class RoamEnemyRange : MonoBehaviour
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


    [Header("Enemy Move")]
    public float _moveSpeed;
    public float _stopDistance;
    public float _range;

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

    // NOTE: Need to fix animation and check animation for the others as well
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
            if (_enemyRb.velocity.x > 0.1f || _enemyRb.velocity.x < 0.1f)
            {
                _anim.SetBool("Move", true);
            }
            else
            {
                _anim.SetBool("Move", false);
            }

            if (_hittingWallFront || !_notAtEdgeFront) // _hittingWallBack || !_notAtEdgeBack
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

            // within range the enemy will always face the player as the enemy moves
            if (Vector2.Distance(transform.position, _player.position) < _range && Vector2.Distance(transform.position, _player.position) > _stopDistance && _notAtEdgeBack && _notAtEdgeFront)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack", true);

                // enemy to the right of player 
                if (transform.position.x >= _player.position.x)
                {
                    if (_moveRight == true)
                    {
                        transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    }
                    else
                    {
                        transform.localScale = new Vector3(-_localScaleX, _localScaleY, _localScaleZ);
                    }
                }
                // enemy to the left of player
                else if (transform.position.x < _player.position.x)
                {
                    if (_moveRight == false)
                    {
                        transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    }
                    else
                    {
                        transform.localScale = new Vector3(_localScaleX, _localScaleY, _localScaleZ);
                    }
                }
            }

            // enemy stops moving if enemy gets too close to player based on _stopDistance
            if (Vector2.Distance(transform.position, _player.position) <= _stopDistance)
            {
                _anim.SetBool("Idle", false);
                _anim.SetBool("Move", false);
                _anim.SetBool("Attack", true);

                _enemyRb.velocity = new Vector2(0, 0);

                // enemy to the right of player
                if (transform.position.x >= _player.position.x)
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

