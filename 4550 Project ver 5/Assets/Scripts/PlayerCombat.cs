using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    //Sword
    public Animator animator;
    public Transform swordAttPoint;
    public float swordAttRange;
    public LayerMask enemyLayers;
    public float SwordAttRate = 2f;
    float SwordNextAttTime = 0f;
    public GameObject SelectSword;
    public GameObject SwordAudio;

    //Laser
    public Transform LaserfirePoint;
    public LineRenderer lineRenderer;
    public GameObject Fxef;
    public GameObject Laserlight;
    public GameObject SelectLasergun;
    public float LaserBullet;
    public Text LaserBulletCount;

    //Shotgun
    public GameObject ball;
    public GameObject FireEffect;
    public Transform ShotgunfirePoint;
    public float ShotgunFireRate = 2f;
    float ShotgunNextFireTime = 0f;
    public float BallNumber;
    public GameObject SelectShotgun;
    public float ShotgunBullet;
    public Text ShotgunBulletCount;

    private void Awake()
    {
        SelectSword.SetActive(false);
        SelectLasergun.SetActive(false);
        SelectShotgun.SetActive(false);
    }

    void Update()
    {
        selectweapon();
        //three attack modes of the Sword
        if (SelectSword.activeInHierarchy && Time.time >= SwordNextAttTime)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                SwordAudio.SetActive(false);
                SwordAttackStab();
                SwordNextAttTime = Time.time + 1f / SwordAttRate;
                SwordAudio.SetActive(true);

            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                SwordAudio.SetActive(false);
                SwordAttackChop();
                SwordNextAttTime = Time.time + 2f / SwordAttRate;
                SwordAudio.SetActive(true);

            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SwordAudio.SetActive(false);
                SwordAttackUpward();
                SwordNextAttTime = Time.time + 3f / SwordAttRate;
                SwordAudio.SetActive(true);

            }
        }

        //Laser
        if (SelectLasergun.activeInHierarchy && Input.GetKeyDown(KeyCode.J))
        {
            if(LaserBullet > 0)
            {
                Laserlight.SetActive(true);
                lineRenderer.enabled = true;
                shotline();
                LaserBullet -= 1;
            }

        }
        if (SelectLasergun.activeInHierarchy && Input.GetKeyUp(KeyCode.J))
        {
            Laserlight.SetActive(false);
            lineRenderer.enabled = false;
            Fxef.SetActive(false);
        }
        LaserBulletCount.text = LaserBullet.ToString();

        //shotgun
        if (SelectShotgun.activeInHierarchy && Input.GetKeyDown(KeyCode.J))
        {
            if (Time.time >= ShotgunNextFireTime && ShotgunBullet >= 6f)
            {
                shotgun();
                FireEffect.transform.position = ShotgunfirePoint.position;
                FireEffect.SetActive(true);
                ShotgunNextFireTime = Time.time + 1f / ShotgunFireRate;
                ShotgunBullet -= BallNumber;
            }
        }
        if (SelectShotgun.activeInHierarchy && Input.GetKeyUp(KeyCode.J))
        {
            FireEffect.SetActive(false);
        }
        ShotgunBulletCount.text = ShotgunBullet.ToString();
    }

    void selectweapon()
    {
        //select Sword
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetTrigger("SelectSword");
            animator.SetBool("IsUnarmed", false);
            animator.SetBool("IsOnSword", true);
            animator.SetBool("IsOnLaser", false);
            animator.SetBool("IsOnShotgun", false);
            SelectSword.SetActive(true);
            SelectLasergun.SetActive(false);
            SelectShotgun.SetActive(false);
        }
        //select laser gun
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetTrigger("SelectLasergun");
            animator.SetBool("IsUnarmed", false);
            animator.SetBool("IsOnSword", false);
            animator.SetBool("IsOnLaser", true);
            animator.SetBool("IsOnShotgun", false);
            SelectSword.SetActive(false);
            SelectLasergun.SetActive(true);
            SelectShotgun.SetActive(false);
        }
        //select shotgun
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            animator.SetTrigger("SelectShotgun");
            animator.SetBool("IsUnarmed", false);
            animator.SetBool("IsOnSword", false);
            animator.SetBool("IsOnLaser", false);
            animator.SetBool("IsOnShotgun", true);
            SelectSword.SetActive(false);
            SelectLasergun.SetActive(false);
            SelectShotgun.SetActive(true);
        }
    }

    void SwordAttackStab()
    {
        animator.SetTrigger("SwordStab");
        animator.SetBool("IsUnarmed", false);
        animator.SetBool("IsOnSword", true);
        animator.SetBool("IsOnLaser", false);
        animator.SetBool("IsOnShotgun", false);

        Collider2D[] hitInfo = Physics2D.OverlapCircleAll(swordAttPoint.position, swordAttRange, enemyLayers);
        foreach (Collider2D enemy in hitInfo)
        {
            enemy.GetComponent<Enemy>().TakeDamage(30);
        }
    }
    void SwordAttackChop()
    {
        animator.SetTrigger("SwordChop");
        animator.SetBool("IsUnarmed", false);
        animator.SetBool("IsOnSword", true);
        animator.SetBool("IsOnLaser", false);
        animator.SetBool("IsOnShotgun", false);

        Collider2D[] hitInfo = Physics2D.OverlapCircleAll(swordAttPoint.position, swordAttRange, enemyLayers);
        foreach (Collider2D enemy in hitInfo)
        {
            enemy.GetComponent<Enemy>().TakeDamage(40);
        }
    }
    void SwordAttackUpward()
    {
        animator.SetTrigger("SwordUpward");
        animator.SetBool("IsUnarmed", false);
        animator.SetBool("IsOnSword", true);
        animator.SetBool("IsOnLaser", false);
        animator.SetBool("IsOnShotgun", false);

        Collider2D[] hitInfo = Physics2D.OverlapCircleAll(swordAttPoint.position, swordAttRange, enemyLayers);
        foreach (Collider2D enemy in hitInfo)
        {
            enemy.GetComponent<Enemy>().TakeDamage(50);
        }
    }
    //Draw Sword Range
    void OnDrawGizmosSelected()
    {
        if (swordAttPoint == null) return;

        Gizmos.DrawWireSphere(swordAttPoint.position, swordAttRange);
    }

    void shotline()
    {
        animator.SetTrigger("LaserFire");
        animator.SetBool("IsUnarmed", false);
        animator.SetBool("IsOnSword", false);
        animator.SetBool("IsOnLaser", true);
        animator.SetBool("IsOnShotgun", false);

        RaycastHit2D hit;
        hit = Physics2D.Raycast(LaserfirePoint.position, LaserfirePoint.right);
        //Hitting the target generates damage
        if (hit && lineRenderer.enabled == true)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(10);
            }
            //Laser startposition
            lineRenderer.SetPosition(0, LaserfirePoint.position);
            //Laser endposition
            lineRenderer.SetPosition(1, hit.point);
            Fxef.transform.position = hit.point;
            Fxef.SetActive(true);
        }
        //Failure to hit generates a ray of fixed length
        else
        {
            lineRenderer.SetPosition(0, LaserfirePoint.position);
            lineRenderer.SetPosition(1, LaserfirePoint.position + LaserfirePoint.right * 100);
        }
    }


    void shotgun()
    {
        animator.SetTrigger("ShotgunFire");
        animator.SetBool("IsUnarmed", false);
        animator.SetBool("IsOnSword", false);
        animator.SetBool("IsOnLaser", false);
        animator.SetBool("IsOnShotgun", true);

        for (int i = 0; i < BallNumber; i++)
        {
            Instantiate(ball, ShotgunfirePoint.position, Quaternion.identity);
        }
    }
}
