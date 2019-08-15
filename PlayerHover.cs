using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerHover : MonoBehaviour
{
    Player player;

    public float glideStrength;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetCollisionInfo().below == false)
        {
            if (Input.GetKey(player.jumpKey))
            {
                player.Glide(glideStrength);
            }
        }
    }
}
