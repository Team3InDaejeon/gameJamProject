using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        // �̹� �ν��Ͻ��� �����ϸ� ���ο� ������Ʈ�� �ı�
        if (keyManager)
        {
            Destroy(gameObject);
        }
        else
        {
            // �̱��� �ν��Ͻ� ����
            keyManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    // ==Ű ��============================
    // �̵�
    public KeyCode MoveLeft = KeyCode.LeftArrow;
    public KeyCode MoveRight = KeyCode.RightArrow;
    public KeyCode MoveUp = KeyCode.UpArrow;
    public KeyCode MoveDown = KeyCode.DownArrow;

    // ����
    public KeyCode Jump = KeyCode.Space;

    
  // ����
    public KeyCode MeleeAttack = KeyCode.A;     //��������
    public KeyCode QSkill = KeyCode.Q;              //QSkill
    public KeyCode WSkill = KeyCode.W;             //WSkill
    public KeyCode ESkill = KeyCode.E;              //ESkill
    public KeyCode RSkill = KeyCode.R;              //RSkill

    // �ɼ�
    public KeyCode Menu = KeyCode.Escape;          // �ɼ�â


  // ==���� �Լ�======================
  public int GetAxisRawHorizontal()
  {
        if (Input.GetKey(MoveLeft))
        {
            return -1;
        }

        if (Input.GetKey(MoveRight))
        {
            return 1;
        }

        return 0;
    }

  public int GetAxisRawVertical()
  {
      if (Input.GetKey(MoveDown)) return -1;
      else if (Input.GetKey(MoveUp)) return 1;
      else return 0;
  }
}