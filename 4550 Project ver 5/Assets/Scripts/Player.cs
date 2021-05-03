using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 100;
    public HealthbarBehaviour Healthbar;
    [HideInInspector] public Animator anim;

    void Start()
    {
        Health = MaxHealth;
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        Healthbar.SetHealth(Health, MaxHealth);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
        }
    }
}
