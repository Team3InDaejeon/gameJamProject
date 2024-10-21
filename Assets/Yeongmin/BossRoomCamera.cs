using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BossRoomCamera : MonoBehaviour
{
    CinemachineVirtualCamera VRCamera;
    [SerializeField]
    GameObject CameraHolder;

    void Start()
    {
        VRCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetFollowCameraHolder() 
    {
        if (VRCamera != null && CameraHolder != null)
        {
            VRCamera.Follow = CameraHolder.transform;
        }
    }

    public void SetFollowPlayer()
    {
        if (VRCamera != null )
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                VRCamera.Follow = player.transform;
            }
        }
    }

}
