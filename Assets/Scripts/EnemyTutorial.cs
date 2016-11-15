using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorial : MonoBehaviour {
    public float health = 100.0f;

    protected void Die()
    {
        GameObject.Destroy(gameObject);
    }


    public void Damage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0f;
            Die();
        }
    }
}
