using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Movement2D))]
public class Player : MonoBehaviour
{
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

        if (Input.GetKey(jumpKey) && currentJumpCount > 0)
        {
            currentJumpCount--;
            velocity.y = jumpVelocity;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        velocity.x = input.x * moveSpeed;

        velocity.y += Time.deltaTime * gravity;

        controller.Move(velocity * Time.deltaTime);

        if (controller.collisions.below)
            prevBelow = true;
    }

    public void SetGravity(float gravity)
    {
        this.gravity = gravity;
    }
    public float GetGravity()
    {
        return gravity;
    }

    public Movement2D.CollisionInfo GetCollisionInfo()
    {
        return controller.collisions;
    }

    public void Glide(float glideStrength)
    {
        if (velocity.y < 0)
            velocity.y = glideStrength;
    }
}
