using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHover : MonoBehaviour
{
    Player player;

    public float fallingSpeed;
    public float staminaPerSecond;

    private float staminaPerFrame;
    private float timeSinceHoverStart;
    private bool keyPressed;
    

    // Start is called before the first frame update
    void Start()
    {       
        player = GetComponent<Player>();
        timeSinceHoverStart = player.staminaMax;
        keyPressed = false;
        
        if (fallingSpeed > 0)
        {
            fallingSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.gm.paused)
        {
            staminaPerFrame = staminaPerSecond * Time.deltaTime;
            if (player.GetCollisionInfo().below == false)
            {
                keyPressed = false;
                foreach (KeyCode jumpKey in player.jumpKeys)
                {

                    if (Input.GetKey(jumpKey))
                    {
                        keyPressed = true;
                    }
                }
                if (keyPressed)
                {
                    if (player.velocity.y < 0 && player.HasStamina(staminaPerFrame))
                    {
                        player.DrainStamina(staminaPerFrame);
                        player.velocity.y = fallingSpeed;
                        player.anim.SetBool("Gliding", true);
                    }
                    else
                    {
                        player.anim.SetBool("Gliding", false);
                    }

                }
                else
                {
                    timeSinceHoverStart = Time.time;
                    player.anim.SetBool("Gliding", false);
                }
            }
            else
            {
                player.anim.SetBool("Gliding", false);
            }
        }
    }
}
