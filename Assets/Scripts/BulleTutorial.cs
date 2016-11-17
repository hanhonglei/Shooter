using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulleTutorial : MonoBehaviour
{

    private Rigidbody rb;
    private bool bMove = true;

    public float speed = 50.0f;
    public float damage = 5.0f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (bMove)
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case Tags.obstacle:
                bMove = false;
                break;
            case Tags.enemy:
                break;
            default:
                return;
        }
        BeginDamage(other);
    }

    private void BeginDamage(Collider other)
    {
        GetComponent<Renderer>().enabled = false;

        EnemyTutorial otherActor = other.GetComponent<EnemyTutorial>();
        if (otherActor)
            otherActor.Damage(damage);
        GameObject.Destroy(gameObject);
    }
}
