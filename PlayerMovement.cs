using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb2d;

    private int currentJumps = 1;
    private int defaultJumps = 1;

    public float speed = 1.0f;
    public float jump = 20.0f;

    public Animator animator;

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
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y); 
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
        }
        else if (Input.GetAxis("Horizontal") < 1)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
        */
       
        rb2d.velocity = new Vector2(speed * Input.GetAxis("Horizontal"), rb2d.velocity.y);
        
        if (Mathf.Abs(rb2d.velocity.x) > 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(rb2d.velocity.x));
        }

        if (rb2d.velocity.normalized.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        if (rb2d.velocity.normalized.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);

        animator.SetFloat("VerticalSpeed", rb2d.velocity.normalized.y);

        if (Input.GetAxis("Vertical") > 0 && currentJumps > 0)
        {
            rb2d.AddForce(new Vector2(0, jump));
            currentJumps--;
        }

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            currentJumps = defaultJumps;
        }
    }
}
