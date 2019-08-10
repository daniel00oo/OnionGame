using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Canvas canvas;

    public int points = 0;
    public bool paused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
        }

        if (paused == true)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }

    void Pause()
    {
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void Resume()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
