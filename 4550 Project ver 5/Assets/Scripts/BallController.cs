using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody2D bod;
    public float spd;
    public float randomUpSpd;
 //reference
    CharacterController2D charCont;
    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
        charCont = GameObject.Find("Player").GetComponent<CharacterController2D>();
        //Destroy(gameObject, 2f);
    }
    private void OnEnable()
    {
        int dir = Random.Range(0, 2);
        bod.AddForce(Vector2.up * Random.Range(-randomUpSpd, randomUpSpd));
        if(charCont.m_FacingRight == true)
        {
            bod.AddForce(Vector2.right * spd);
        }
        else
        {
            bod.AddForce(Vector2.left * spd);
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
