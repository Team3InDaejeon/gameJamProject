using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    int DotDamageAmount;
    float SkillDuration;

    public float DamageInterval = 1.0f; 
    private float ElapsedTime;    
    private float DamageTimer;

    List<ICombat> MonstersArray = new List<ICombat>();

    public void Initialize(int NewDotDamageAmount, float NewSkillDuration)
    {
        DotDamageAmount = NewDotDamageAmount;
        SkillDuration = NewSkillDuration;
        ElapsedTime = 0f;
        DamageTimer = 0f;
    }

    void Update()
    {
        ElapsedTime += Time.deltaTime;

        DamageTimer += Time.deltaTime;

        if (DamageTimer >= DamageInterval)
        {
            ApplyDamage();
            DamageTimer = 0.0f; 
        }

        if (ElapsedTime >= SkillDuration)
        {
            Destroy(this);
        }
    }

    private void ApplyDamage()
    {
        // foreach (GameObject obj in affectedObjects)
        // {
        //     // 적에게 피해 적용 로직
        //     EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();
        //     if (enemyHealth != null)
        //     {
        //         enemyHealth.TakeDamage(dotDamage);
        //         Debug.Log($"Apply {dotDamage} damage to {obj.name}");
        //     }
        // }

        foreach (ICombat monster in MonstersArray)
        {
            if (monster != null)
            {
                monster.TakeDamage(DotDamageAmount);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        ICombat combatTarget = other.GetComponent<ICombat>();
        if (combatTarget != null)
        {
            MonstersArray.Add(combatTarget);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        ICombat combatTarget = other.GetComponent<ICombat>();
        if (combatTarget != null)
        {
            MonstersArray.Remove(combatTarget);
        }
    }
}
