using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
[RequireComponent(typeof (Rigidbody2D))]
public class HarmfulSpike : MonoBehaviour
{
    BoxCollider2D box;
    Rigidbody2D rb;

    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        box.isTrigger = true;

        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().TakeDamage(1);
        }
    }
}
