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
    public float PlayerHP = 100.0f;
    public GameObject GameOverButton;

    private GameObject Canvas;

    public int EventTick = 0;

    void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("CanvasUI");
    }

    public void GameOver()
    {
        //GameSceneManager.instance.ChangeScene(GameOverSceneName);

        GameObject GOB = Instantiate(GameOverButton) as GameObject;
        GOB.transform.SetParent(Canvas.transform, false);
    }
}
