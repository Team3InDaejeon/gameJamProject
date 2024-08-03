using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Arm : MonoBehaviour
{
    EnemyType enemyType= EnemyType.Red;
    int damageCount=10;
    int playerLayer;
    public void Init(int damageCount, EnemyType enemyType)
    {
        this.enemyType = enemyType;
        this.damageCount = damageCount;
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            CharacterPlayer player = other.GetComponent<CharacterPlayer>();
            if (player != null)
            {
                player.TakeDamage(damageCount, enemyType);
                Debug.Log("Player Hit");
            }
        }
    }
}
