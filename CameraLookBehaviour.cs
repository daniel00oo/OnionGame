using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLookBehaviour : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    [Tooltip("Value between 0 and 1 to offset the camera on the Y axis while \"looking down\"")]
    [Range(0, 1)]
    public float offsetY = .1f;
    [Tooltip("Offset speed in seconds")]
    public float offsetSpeed = 1;
    public float idleTime = .5f;

    //[Tooltip("The actual value of Y after the offset | is equal to Y - offsetY")]
    private float relativePosYLookDown;
    private float relativePosYLookUp;
    private float currentIdleTimer;
    CinemachineFramingTransposer cinemachineFraming;
    private float originalY;

    void Start()
    {
        cinemachineFraming = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        originalY = cinemachineFraming.m_ScreenY;
        currentIdleTimer = Time.time;
        relativePosYLookDown = cinemachineFraming.m_ScreenY - offsetY;
        relativePosYLookUp = cinemachineFraming.m_ScreenY + offsetY;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                if (Mathf.Abs(Time.time - currentIdleTimer) > idleTime)
                {
                    if (cinemachineFraming.m_ScreenY < relativePosYLookDown)
                    {
                        cinemachineFraming.m_ScreenY = relativePosYLookDown;
                    }
                    else if (cinemachineFraming.m_ScreenY > relativePosYLookDown)
                    {
                        cinemachineFraming.m_ScreenY -= offsetSpeed * Time.deltaTime;
                    }
                }
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                if (Mathf.Abs(Time.time - currentIdleTimer) > idleTime)
                {
                    if (cinemachineFraming.m_ScreenY > relativePosYLookUp)
                    {
                        cinemachineFraming.m_ScreenY = relativePosYLookUp;
                    }
                    else if (cinemachineFraming.m_ScreenY < relativePosYLookUp)
                    {
                        cinemachineFraming.m_ScreenY += offsetSpeed * Time.deltaTime;
                    }
                }
            }
            else
            {
                currentIdleTimer = Time.time;
                cinemachineFraming.m_ScreenY = originalY;
            }

        }
        else
        {
            currentIdleTimer = Time.time;
            cinemachineFraming.m_ScreenY = originalY;
        }
    }

}

