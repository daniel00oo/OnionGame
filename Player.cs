using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Movement2D))]
public class Player : MonoBehaviour
{
    public Animator anim;
    public float jumpHeight = 4f;
    public float jumptimeApex = .4f;
    public float moveSpeed = 6;
    float gravity;
    Vector3 velocity;

    public KeyCode jumpKey;
    public int nrOfJumps;
    private int currentJumpCount;
    float jumpVelocity = 8;

    private bool prevBelow;

    Movement2D controller;
    // Start is called before the first frame update
    void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(jumptimeApex, 2);
        jumpVelocity = -gravity * jumptimeApex;
        controller = GetComponent<Movement2D>();
        jumpKey = KeyCode.Space;
        currentJumpCount = nrOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.collisions.left)
        {
            Debug.Log(true);
        }
        //Debug.Log(controller.collisions.below);
        if (prevBelow == true && controller.collisions.below == false && currentJumpCount == nrOfJumps)
        {
            prevBelow = false;
            currentJumpCount--;
        }
        if (controller.collisions.above || controller.collisions.below)
            velocity.y = 0;
        if (controller.collisions.below)
            currentJumpCount = nrOfJumps;

        if (Input.GetKeyDown(jumpKey) && currentJumpCount > 0)
        {
            currentJumpCount--;
            velocity.y = jumpVelocity;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;

        //Animation handling
        if (velocity.x > 0)
        {
            anim.SetFloat("Speed", 1);
            if (transform.localScale.x < 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

        }


        else if (velocity.x < 0)
        {
            anim.SetFloat("Speed", 1);
            if (transform.localScale.x > 0)
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else
            anim.SetFloat("Speed", 0);
        anim.SetFloat("VerticalSpeed", velocity.y);

        velocity.y += Time.deltaTime * gravity;

        controller.Move(velocity * Time.deltaTime);

        

        if (controller.collisions.below)
            prevBelow = true;
    }
}
