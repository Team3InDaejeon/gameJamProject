using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Test
    public float PlayerHP = 100.0f;
    public GameObject GameOverButton;

    private GameObject Canvas;
    
    public int EventTick = 0;

    //public string GameOverSceneName;
    
    void Start()
    {
        Canvas = GameObject.FindGameObjectWithTag("CanvasUI");
    }

    void Update()
    {
        if (PlayerHP <= 0.0f && EventTick < 1 && Canvas != null)
        {
            GameOver();
            EventTick++;
        }
    }

    private void GameOver()
    {
        //GameSceneManager.instance.ChangeScene(GameOverSceneName);

        GameObject GOB = Instantiate(GameOverButton) as GameObject;
        GOB.transform.SetParent(Canvas.transform, false);
    }
}
