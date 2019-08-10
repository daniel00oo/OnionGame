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
            Collected();
        }
    }

    private void Collected()
    {
        Debug.Log("Collected " + gameObject.name + " Points: " + gc.points.ToString());
        gc.points += 10;
        Destroy(gameObject);
    }
}
