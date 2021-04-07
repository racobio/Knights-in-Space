using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 100;
    public HealthbarBehaviour Healthbar;
    public Animator animator;
    public float CurrentTime = 0f;

    void Start()
    {
        Health = MaxHealth;
        Healthbar.SetHealth(Health, MaxHealth);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(30);
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Healthbar.SetHealth(Health, MaxHealth);

    

        if (Health <= 0)
        {
           animator.SetBool("IsDead", true);
          // Player.GetComponent<PlayerMovement>().enabled = false; Add to TriggerEvent Script
          
            Invoke("Respawn", 1);
        }
          
    }

    public void Respawn()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
