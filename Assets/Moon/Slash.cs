
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private int damage = 10;
    int playerLayer;
    public void Init(int damageCount, EnemyType enemyType)
    {
        this.damage = damageCount;
        playerLayer = LayerMask.NameToLayer("Player");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            CharacterPlayer player = other.GetComponent<CharacterPlayer>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Debug.Log("Player Hit");
            }
        }

    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }
}