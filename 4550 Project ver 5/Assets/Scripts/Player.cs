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
        Healthbar.SetHealth(Health, MaxHealth);
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Healthbar.SetHealth(Health, MaxHealth);
        if (Health <= 0)
        {
            Health = 0;
        }
    }
}
