using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperController : MonoBehaviour
{
    public float attackRange;
    public float FireRate = 2f;
    float NextFireTime = 0f;
    public GameObject fire;
    public GameObject firepoint;
    public LayerMask enemyLayers;

    private void Update()
    {
        if (Time.time >= NextFireTime)
        {
            shootFire();
            NextFireTime = Time.time + 1f / FireRate;
        }

    }

    void shootFire()
    {
        Collider2D[] hitInfo = Physics2D.OverlapCircleAll(firepoint.transform.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitInfo)
        {
            Vector3 dir = enemy.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            Instantiate(fire, firepoint.transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        }
    }
}
