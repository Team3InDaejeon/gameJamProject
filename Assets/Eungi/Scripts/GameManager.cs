using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    public static GameManager Inst
    {
        get
        {
            if (gameManager == null)
            {
                gameManager = FindObjectOfType<GameManager>();
                if (gameManager == null)
                {
                    Debug.LogError("GameManager does Not Exist!");
                }
            }
            return gameManager;
        }
    }

    //Test
    public GameObject GameOverButton;

    void Start()
    {
       //  Canvas = GameObject.FindGameObjectWithTag("CanvasUI");
    }

    public void GameOver()
    {
        // GameOver
    }

    public void EnterBossCombat() 
    {
        // EnterBossRoom
        GameObject VRCamera = GameObject.FindWithTag("CineMachineCamera");
        if (VRCamera != null)
        {
            VRCamera.GetComponent<BossRoomCamera>().SetFollowCameraHolder();
        }
    }

    public void PostBossDeath() 
    {
        GameObject VRCamera = GameObject.FindWithTag("CineMachineCamera");
        if (VRCamera != null)
        {
            VRCamera.GetComponent<BossRoomCamera>().SetFollowPlayer();
        }
    }
}
