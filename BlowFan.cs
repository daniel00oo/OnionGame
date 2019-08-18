using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider2D))]
[RequireComponent(typeof (Rigidbody2D))]
public class BlowFan : MonoBehaviour
{
    public Player player;
    public float blowPower = 20;
    public bool upsideDown = false;

    private float originalFallSpeed;
    private PlayerHover ph;
    private BoxCollider2D box;
    private Rigidbody2D rb;

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        ph = player.GetComponent<PlayerHover>();

        blowPower = upsideDown ? -blowPower : blowPower;
        originalFallSpeed = ph.fallingSpeed;
        box.isTrigger = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && HasGlidePower())
        {
            ph.externalCanGlide = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && HasGlidePower())
        {
            ph.fallingSpeed = blowPower;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && HasGlidePower())
        {
            ph.fallingSpeed = originalFallSpeed;
            ph.externalCanGlide = false;
        }
    }

    private bool HasGlidePower()
    {
        if (player.staminaMax > 0 && ph != null)
            return true;

        return false;
    }
}
