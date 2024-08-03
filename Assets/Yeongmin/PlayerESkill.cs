using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerESkill : CharacterSkill
{
    [SerializeField]
    float SkillDuration = 5.0f;
    float SkillStartTime = 0.0f;
    [SerializeField]
    int DotDamageAmount = 10;

    [SerializeField]
    private GameObject FireTrapPrefab;

    [SerializeField]
    float DurationDistance;

    private GameObject ActiveFireTrap;

    public override void StartSkill() 
    {
        if (Player != null && CooldownManager.CheckCooldown())
        {
            CooldownManager.StartCooldown();
            SpawnFireTrap();
        }
    }

    public override void UpdateSkill()
    {
        if (Player == null)
        {
            return;
        }

        base.UpdateSkill();
    }
    public override void EndSkill() 
    {
        if (ActiveFireTrap != null)
        {
            Destroy(ActiveFireTrap);
        }
    }

    private void SpawnFireTrap() 
    {
        if (Player == null) 
        {
            return;
        }

        float PlayerWidth = Player.GetComponent<BoxCollider2D>().bounds.size.x;
        float FireTrapWidth = FireTrapPrefab.GetComponent<BoxCollider2D>().bounds.size.x;
        Vector2 SpawnPosition = (Vector2)transform.position +(Vector2)transform.right *(PlayerWidth + FireTrapWidth/2);
        SpawnPosition += new Vector2(DurationDistance,0);
        ActiveFireTrap = Instantiate(FireTrapPrefab, SpawnPosition, Quaternion.identity);
        FireTrap fireTrap = ActiveFireTrap.GetComponent<FireTrap>();
       
        if (ActiveFireTrap != null)
        {
            fireTrap.Initialize(DotDamageAmount, SkillDuration);
        }

        // 일정 시간 후 불장판 제거
        Destroy(ActiveFireTrap, SkillDuration);
    }
}
