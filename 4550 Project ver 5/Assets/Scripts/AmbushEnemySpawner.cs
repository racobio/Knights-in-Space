using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushEnemySpawner : MonoBehaviour
{
    [HideInInspector] public Transform _player;
    public float _range;
    public GameObject _ambushEnemy;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < _player.position.x && Vector2.Distance(transform.position, _player.position) > _range)
        {
            // put the spawner directly on the ground
            Instantiate(_ambushEnemy, new Vector3(transform.position.x, transform.position.y + .6f, transform.position.z), Quaternion.identity);
            //Instantiate(_ambushEnemy, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
