using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaIncreasePickup : Collectible
{
    public Player player;
    public float amount;

    protected override void OnPickup()
    {
        player.staminaMax += amount;
        player.currentStamina = player.staminaMax;
    }
}
