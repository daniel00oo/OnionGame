using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2d;

    public float speed = 1.0f;
    public float jump = 20.0f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetAxis("Horizontal") > 0)
        {
            Debug.Log(Input.GetAxis("Horizontal"));
            rb2d.velocity.Set(speed, rb2d.velocity.y); 
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb2d.velocity.Set(-speed, rb2d.velocity.y);
        }
        else
        {
            rb2d.velocity.Set(0, rb2d.velocity.y);
        }
        */

        rb2d.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb2d.velocity.y);

        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Platform" && Input.GetAxis("Vertical") > 0)
        {
            rb2d.AddForce(new Vector2(0, jump));
        }
    }
}
