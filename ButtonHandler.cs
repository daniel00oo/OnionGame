using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public GameController g;
    public void OnClickResume()
    {
        g.paused = false;
    }

    public void OnClickExitGame()
    {
        Application.Quit();
    }
}
