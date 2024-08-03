using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    private static MonsterManager monsterManager;

    [SerializeField]
    List<int> StageMonsterNum;
    int SpawnedMonsterNum = 0;
    int CurrentStage = 0;
    public static MonsterManager Inst
    {
        get
        {
            if (monsterManager == null)
            {
                monsterManager = FindObjectOfType<MonsterManager>();
                if (monsterManager == null)
                {
                    Debug.LogError("monsterManager does Not Exist! ");
                }
            }
            return monsterManager;
        }
    }

    void Awake()
    {
        if (monsterManager)
        {
            Destroy(gameObject);
        }
        else
        {
            monsterManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    List<GameObject> MonsterArray;
    
    public int GetMonsterNum() 
    {
        return MonsterArray.Count;
    }

    void Start()
    {
        MonsterArray = new List<GameObject>();
        Reset();
    }

    public void AddMonster(GameObject Monster)
    {
        MonsterArray.Add(Monster);
        ++SpawnedMonsterNum;
    }

    public void RemoveMonster(GameObject Monster)
    {
        MonsterArray.Remove(Monster);

        if (SpawnedMonsterNum == 0) 
        {
            // 보스룸 도달할 수 있도록 하기
        }
    }

    public void Reset()
    {
        MonsterArray.Clear();
        SpawnedMonsterNum = 0;
    }
}
