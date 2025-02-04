﻿using System.Collections;
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

    public GameObject GameOverButton;
    public int CurrentStage = 0;
    public bool IsBossDead = false;
    public GameObject Square;

    GameObject rightBlock;
    GameObject leftBlock;
    void Start()
    {
       //  Canvas = GameObject.FindGameObjectWithTag("CanvasUI");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
    }

    public void EnterBossCombat(Transform ColiderCenter) 
    {
        // EnterBossRoom
        GameObject VRCamera = GameObject.FindWithTag("CineMachineCamera");
        if (VRCamera != null)
        {
            VRCamera.GetComponent<BossRoomCamera>().SetFollowCameraHolder();
        }
        Vector2 Offset = new Vector2(15, 0);
        rightBlock=Instantiate(Square, (Vector2)ColiderCenter.position + Offset, Quaternion.identity);
        leftBlock = Instantiate(Square, (Vector2)ColiderCenter.position - Offset, Quaternion.identity);

    }

    public void PostBossDeath() 
    {
        if(rightBlock!=null)
        {
            Destroy(rightBlock);
        }
        if(leftBlock!=null)
        {
            Destroy(leftBlock);
        }
        if(IsBossDead){
            return;
        }
        IsBossDead = true;
        // 카메라 고정
        GameObject VRCamera = GameObject.FindWithTag("CineMachineCamera");
        if (VRCamera != null)
        {
            VRCamera.GetComponent<BossRoomCamera>().SetFollowPlayer();
        }

        // NPC 스폰
        NPCSpawner Spawner = GameObject.FindWithTag("NPCSpawner").GetComponent<NPCSpawner>();
        if (Spawner) 
        {
            Spawner.SpawnInteractiveNPC(CurrentStage);
        }
    }

    public void NextStage() 
    {
        switch (CurrentStage) 
        {
            case 0:
                ++CurrentStage;
                GameSceneManager.Inst.ChangeScene("Stage_02");
                break;
            case 1:
                ++CurrentStage;
                GameSceneManager.Inst.ChangeScene("Stage_03");
                break;
            case 2:
                ++CurrentStage;
                GameSceneManager.Inst.ChangeScene("Stage_04");
                break;
            case 3:
                ++CurrentStage;
                GameSceneManager.Inst.ChangeScene("Stage_05");
                break;
        }
    }
}
