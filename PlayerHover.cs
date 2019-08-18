using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHover : MonoBehaviour
{
    Player player;

    public float fallingSpeed;
    public float staminaPerSecond;
    public float glideTilt = 0.1f;

    private float staminaPerFrame;
    private float timeSinceHoverStart;
    private bool keyPressed;
    public SpriteRenderer sr;

    public bool externalCanGlide = false;
    

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
                    if ((player.velocity.y < 0 && player.HasStamina(staminaPerFrame)) || externalCanGlide)
                    {
                        player.DrainStamina(staminaPerFrame);
                        player.velocity.y = fallingSpeed;
                        player.anim.SetBool("Gliding", true);
                        if (player.velocity.x != 0)
                        {
                            float newTilt = Mathf.Lerp(sr.transform.rotation.z, -player.velocity.x, glideTilt * Time.deltaTime);
                            sr.transform.rotation = new Quaternion(0, 0, newTilt, 1);
                        }
                        else
                        {
                            float newTilt = Mathf.Lerp(sr.transform.rotation.z, 0, glideTilt * 100 * Time.deltaTime);
                            sr.transform.rotation = new Quaternion(0, 0, newTilt, 1);
                        }
                    }
                    else
                    {
                        player.anim.SetBool("Gliding", false);
                        float newTilt = Mathf.Lerp(sr.transform.rotation.z, 0, glideTilt * 100 * Time.deltaTime);
                        sr.transform.rotation = new Quaternion(0, 0, newTilt, 1);
                    }
                }
                else
                {
                    player.anim.SetBool("Gliding", false);
                    float newTilt = Mathf.Lerp(sr.transform.rotation.z, 0, glideTilt * 100 * Time.deltaTime);
                    sr.transform.rotation = new Quaternion(0, 0, newTilt, 1);
                }
            }
            else
            {
                player.anim.SetBool("Gliding", false);
                sr.transform.rotation = new Quaternion(0, 0, 0, 1);
            }
        }
    }
}
