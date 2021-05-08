using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float Health;
    public float MaxHealth = 100;
    public HealthbarBehaviour Healthbar;
    public Animator animator;


    void Start()
    {
        Health = MaxHealth;
        Healthbar.SetHealth(Health, MaxHealth);
        animator = GetComponent<Animator>();
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

    public void Falling()
    {
        TakeDamage(50);
    }

    public void Respawn()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
