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
    public SpriteRenderer srPlayer;

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
    public float airDrag = 1;
    public float groundDrag = 6;
    private bool canMove = true;
    private float currentCantMoveTimer;
    private Vector3 startPos;

    [Header("Stamina variables")]
    public float staminaMax;
    public float currentStamina;
    [Tooltip("Time it takes for stamina to start recharging")]
    public float idleStaminaTime;
    public float secondsToFullStamina;
    private float staminaRechargeTimer;
    private float staminaRechargePerFrame;
    public Slider staminaSlider;

    [Header("Health variables")]
    public int maxHeartCount = 3;
    public int numberOfLives = 2;
    private float lastTimeTookDamage;
    public float invincibilitySeconds = 1;
    private int currentHeartCount;
    private int currentLivesCounter;

    private bool prevBelow;

    Movement2D controller;
    // Start is called before the first frame update
    void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(jumptimeApex, 2);
        jumpVelocity = -gravity * jumptimeApex;
        controller = GetComponent<Movement2D>();
        currentJumpCount = nrOfJumps;
        lastTimeTookDamage = Time.time;
        startPos = transform.position;

        currentHeartCount = maxHeartCount;
        currentLivesCounter = numberOfLives;
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
            if (controller.collisions.above && velocity.y > 0)
                velocity.y = 0;
            if (controller.collisions.below && velocity.y < 0)
                velocity.y = 0;
            if (controller.collisions.right && velocity.x > 0)
                velocity.x = 0;
            if (controller.collisions.left && velocity.x < 0)
                velocity.x = 0;

            if (controller.collisions.below)
                currentJumpCount = nrOfJumps;

            if (controller.collisions.below)
            {
                if (Mathf.Abs(velocity.x) < groundDrag)
                {
                    velocity.x = 0;
                }
                else
                {
                    velocity.x -= groundDrag * Mathf.Sign(velocity.x);
                }
            }
            else
            {
                if (Mathf.Abs(velocity.x) < airDrag)
                {
                    velocity.x = 0;
                }
                else
                {
                    velocity.x -= airDrag * Mathf.Sign(velocity.x);
                }
            }
            if (canMove)
            {
                foreach (KeyCode jumpKey in jumpKeys)
                {
                    if (Input.GetKeyDown(jumpKey) && currentJumpCount > 0)
                    {
                        currentJumpCount--;
                        velocity.y = jumpVelocity;
                    }
                }

                Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (input.x != 0)
                {
                    if (Mathf.Abs(velocity.x) < moveSpeed)
                    {
                        velocity.x = input.x * moveSpeed;
                    }
                    else
                    {
                        if (Mathf.Sign(input.x) != Mathf.Sign(velocity.x))
                            velocity.x += (input.x * moveSpeed) / 10;
                    }
                }
            }
            
            //Animation handling
            ///*
            if (velocity.x > 0)
            {
                anim.SetFloat("Speed", 1);
                srPlayer.flipX = false;
            }


            else if (velocity.x < 0)
            {
                anim.SetFloat("Speed", 1);
                srPlayer.flipX = true;
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
    public void DrainStamina(float quantity)
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

    // Health handling

    public void TakeDamage(int amount, Vector3 sourcePosition, float force)
    {
        if (Time.time - lastTimeTookDamage > invincibilitySeconds)
        {
            lastTimeTookDamage = Time.time;
            currentHeartCount -= amount;

            if (currentHeartCount < 0)
            {
                Death();
            }
            else
            {
                OnDamageVelocity(sourcePosition, force);
                StartCoroutine("TakeDamageAnimation");
            }
        }
    }

    public void Death()
    {
        currentLivesCounter--;
        if (currentLivesCounter < 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = startPos;
            currentHeartCount = maxHeartCount;

            StartCoroutine("TakeDamageAnimation");
        }
    }

    void OnDamageVelocity(Vector3 source, float force)
    {
        velocity.y = force * Mathf.Sign(transform.position.y - source.y);
        velocity.x = force * Mathf.Sign(transform.position.x - source.x);
        currentCantMoveTimer = Time.time;
        StartCoroutine("AfterHitMovementBlock");
    }

    IEnumerator AfterHitMovementBlock()
    {
        canMove = false;
        while (Time.time - currentCantMoveTimer < 0.5f)
        {
            yield return null;
        }
        canMove = true;
    }

    public void AddForce(Vector2 direction, float force)
    {
        if (direction.y != 0)
        {
            velocity.y += force * (direction.y - transform.position.y);
        }
        if (direction.x != 0)
        {
            velocity.x += force * (direction.x - transform.position.x);
        }
    }

    IEnumerator TakeDamageAnimation()
    {
        while (Time.time - lastTimeTookDamage < invincibilitySeconds)
        {
            srPlayer.color = new Color(srPlayer.color.r, srPlayer.color.g, srPlayer.color.b, 0.5f);
            for(int i = 0; i < 10; i++)
                yield return null;
            srPlayer.color = new Color(srPlayer.color.r, srPlayer.color.g, srPlayer.color.b, 1f);
            for (int i = 0; i < 10; i++)
                yield return null;
        }

        srPlayer.color = new Color(srPlayer.color.r, srPlayer.color.g, srPlayer.color.b, 1f);
    }
}
