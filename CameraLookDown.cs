using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLookDown : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    [Tooltip("Value between 0 and 1 to offset the camera on the Y axis while \"looking down\"")]
    [Range(0, 1)]
    public float offsetY = .1f;
    [Tooltip("Offset speed in seconds")]
    public float offsetSpeed = 1;
    public float idleTime = .5f;

    private float currentIdleTimer;
    CinemachineFramingTransposer cinemachineFraming;
    private float originalY;

    // Start is called before the first frame update
    void Start()
    {
        cinemachineFraming = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        originalY = cinemachineFraming.m_ScreenY;
        currentIdleTimer = Time.time;

        if (offsetY > 0)
            offsetY *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (Mathf.Abs(Time.time - currentIdleTimer) > idleTime)
            {
                if (cinemachineFraming.m_ScreenY < -offsetY)
                {
                    cinemachineFraming.m_ScreenY = -offsetY;
                }
                else if (cinemachineFraming.m_ScreenY > -offsetY)
                {
                    cinemachineFraming.m_ScreenY -= offsetSpeed * Time.deltaTime;
                }
            }
        }
        else
        {
            currentIdleTimer = Time.time;
            if (cinemachineFraming.m_ScreenY > originalY)
            {
                cinemachineFraming.m_ScreenY = originalY;
            }
            else if (cinemachineFraming.m_ScreenY < originalY)
            {
                cinemachineFraming.m_ScreenY += offsetSpeed * Time.deltaTime;
            }
        }
        
    }
}
