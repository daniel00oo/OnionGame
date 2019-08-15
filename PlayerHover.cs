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

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        timeSinceHoverStart = player.staminaMax;
        staminaPerFrame = staminaPerSecond * Time.deltaTime;
        if (fallingSpeed > 0)
        {
            fallingSpeed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetCollisionInfo().below == false)
        {
            if (Input.GetKey(player.jumpKey))
            {
                if (player.velocity.y < 0 && player.HasStamina(staminaPerFrame))
                {
                    player.drainStamina(staminaPerFrame);
                    player.velocity.y = fallingSpeed;
                    player.anim.SetBool("Gliding", true);
                }
                
            }
            else
            {
                player.anim.SetBool("Gliding", false);
                timeSinceHoverStart = Time.time;
            }
        }
    }
}
