using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (Movement2D))]
public class Player : MonoBehaviour
{
    [Header("External objects")]
    public GameController gm;
    public Animator anim;

    [Header("Jump variables")]
    public float jumpHeight = 4f;
    public float jumptimeApex = .4f;
    
    public KeyCode[] jumpKeys;
    public int nrOfJumps;
    private int currentJumpCount;
    float jumpVelocity = 8;

    float gravity;
    [HideInInspector]
    public Vector3 velocity;
    [Header("Movement variables")]
    public float moveSpeed = 6;
    [Header("Stamina variables")]
    public float staminaMax;
    public float currentStamina;
    [Tooltip("Time it takes for stamina to start recharging")]
    public float idleStaminaTime;
    public float secondsToFullStamina;
    private float staminaRechargeTimer;
    private float staminaRechargePerFrame;
    public Slider staminaSlider;

    private bool prevBelow;

    Movement2D controller;
    // Start is called before the first frame update
    void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(jumptimeApex, 2);
        jumpVelocity = -gravity * jumptimeApex;
        controller = GetComponent<Movement2D>();
        currentJumpCount = nrOfJumps;
        
        currentStamina = staminaMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.paused)
        {
            staminaRechargePerFrame = (staminaMax / secondsToFullStamina) * Time.deltaTime;

            if (Time.time - staminaRechargeTimer > idleStaminaTime && currentStamina < staminaMax)
            {
                RechargeStamina(staminaRechargePerFrame);
                if (currentStamina > staminaMax)
                {
                    currentStamina = staminaMax;
                }

            }
            staminaSlider.value = currentStamina / staminaMax;

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

            foreach (KeyCode jumpKey in jumpKeys)
            {
                if (Input.GetKeyDown(jumpKey) && currentJumpCount > 0)
                {
                    currentJumpCount--;
                    velocity.y = jumpVelocity;
                }
            }

            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            velocity.x = input.x * moveSpeed;

            //Animation handling
            ///*
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
            //*/

            velocity.y += Time.deltaTime * gravity;

            controller.Move(velocity * Time.deltaTime);



            if (controller.collisions.below)
                prevBelow = true;
        }
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
    public void drainStamina(float quantity)
    {
        
        currentStamina -= quantity;

        staminaRechargeTimer = Time.time;

        Mathf.Clamp(currentStamina, 0, staminaMax);
    }
    public bool HasStamina(float amountToDrain)
    {
        return currentStamina - amountToDrain > 0;
    }
    public void RechargeStamina(float amount)
    {
        currentStamina += amount;
    }
}
