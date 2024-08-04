using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    NonPlayerType NPCType;
    [Header("�������?�־��ּ���")]
    public List<GameObject> monsterEntries = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((int)NPCType > 10 && (int)NPCType < 15)
        {
            return;
        }

        CharacterPlayer Player = other.GetComponent<CharacterPlayer>();
        
        if (Player != null)
        {
            GameObject npc = Instantiate(monsterEntries[(int)NPCType], transform.position, Quaternion.identity);

            // 8, 9, 10 NPCType == BossType
            if ((int)NPCType > 7 && (int)NPCType < 11)
            {
                GameManager.Inst.EnterBossCombat();
            }
            Destroy(this);
        }
    }

    // 1, 2, 3 // 11,12,13,14
    public void SpawnInteractiveNPC(int CurrentStage)
    {
        int NPCIndex = CurrentStage + 11;
        Instantiate(monsterEntries[NPCIndex], transform.position, Quaternion.identity);
        // InteractiveNPC if (npc.GetComponent<InteractiveNPC>()) 
    }
}
