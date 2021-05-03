using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float Health;
    public float MaxHealth=100;
    public HealthbarBehaviour Healthbar;
    bool hashurt = false;
    PlayerCombat playerCom;
    PlayerExp playerExperience;

    void Start()
    {
        Health = MaxHealth;
        Healthbar.SetHealth(Health, MaxHealth);
        playerCom = GameObject.Find("Player").GetComponent<PlayerCombat>();
        playerExperience = GameObject.Find("Player").GetComponent<PlayerExp>();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Healthbar.SetHealth(Health, MaxHealth);
        if (Health <= 0)
        {
            if(!hashurt)
            {
                playerExperience.curExp += 20;
                playerCom.LaserBullet += 2;
                playerCom.ShotgunBullet += 3;
                hashurt = true;
            }
        }
       // Destroy(gameObject);
    }

    

 
}
