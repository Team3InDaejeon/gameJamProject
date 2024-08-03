using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomTrigger : MonoBehaviour
{
   void OnTriggerEnter2D(Collider2D other)
    {
        CharacterPlayer Player = other.GetComponent<CharacterPlayer>();
        if (Player != null)
        {
            GameManager.Inst.EnterBossCombat();
        }
    }
}
