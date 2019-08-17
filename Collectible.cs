using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public GameController gc;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            OnPickup();
            AfterPickup();
        }
    }

    // Default behaviour of a pickup item
    virtual protected void OnPickup()
    {
        Debug.Log("Collected " + gameObject.name + " Points: " + gc.points.ToString());
        gc.points += 10;
    }

    virtual protected void AfterPickup()
    {
        Destroy(gameObject);
    }
}
