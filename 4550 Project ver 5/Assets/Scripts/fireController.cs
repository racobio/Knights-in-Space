using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireController : MonoBehaviour
{
    public float spd;
    Rigidbody2D bod;
    Enemy enemy;

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
        enemy = FindObjectOfType<Enemy>();
    }

    private void OnEnable()
    {
        bod.AddForce(transform.up * spd);
        Invoke("Disable", 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(20);
            Invoke("Disable", 0.001f);
        }
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
