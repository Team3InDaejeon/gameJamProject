using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    private static KeyManager keyManager;
  
    public static KeyManager Inst
    {
        get
        {
            if (keyManager == null)
            {
                keyManager = FindObjectOfType<KeyManager>();
                if (keyManager == null)
                {
                    Debug.LogError("KeyManager does Not Exist! ");
                }
            }
            return keyManager;
        }
    }

    void Awake()
    {
        // 이미 인스턴스가 존재하면 새로운 오브젝트를 파괴
        if (keyManager)
        {
            Destroy(gameObject);
        }
        else
        {
            // 싱글톤 인스턴스 설정
            keyManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // ==키 값============================
    // 이동
    public KeyCode MoveLeft = KeyCode.LeftArrow;
    public KeyCode MoveRight = KeyCode.RightArrow;
    public KeyCode MoveUp = KeyCode.UpArrow;
    public KeyCode MoveDown = KeyCode.DownArrow;

    // 점프
    public KeyCode Jump = KeyCode.Space;

    
  // 공격
    public KeyCode MeleeAttack = KeyCode.A;     //근접공격
    public KeyCode QSkill = KeyCode.Q;              //QSkill
    public KeyCode WSkill = KeyCode.W;             //WSkill
    public KeyCode ESkill = KeyCode.E;              //ESkill
    public KeyCode RSkill = KeyCode.R;              //RSkill

    // 옵션
    public KeyCode Menu = KeyCode.Escape;          // 옵션창


  // ==지원 함수======================
  public float GetAxisRawHorizontal()
  {
        return Input.GetAxis("Horizontal");
     // if (Input.GetKey(MoveLeft)) return -1;
     // else if (Input.GetKey(MoveRight)) return 1;
     // else return 0;
  }

  public int GetAxisRawVertical()
  {
      if (Input.GetKey(MoveDown)) return -1;
      else if (Input.GetKey(MoveUp)) return 1;
      else return 0;
  }
}