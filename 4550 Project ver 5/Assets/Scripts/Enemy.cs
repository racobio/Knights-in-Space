using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float Health;
    public float MaxHealth=100;
    public HealthbarBehaviour Healthbar;

    void Start()
    {
        Health = MaxHealth;
        Healthbar.SetHealth(Health, MaxHealth);
    
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Healthbar.SetHealth(Health, MaxHealth);
        if (Health <= 0)
            Health = 0;
       // Destroy(gameObject);
    }

    

 
}
