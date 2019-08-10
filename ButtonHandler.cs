using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameController g;
    public void onClick()
    {
        g.paused = false;
    }
}
