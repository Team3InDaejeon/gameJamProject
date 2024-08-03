using UnityEngine;
using System.Collections;

public class Boss1Skill2 : BossPattern
{
    Transform bossTransform;
    int damageAmount;
    public override void Init(BossAI bossAI)
    {
        bossTransform = bossAI.transform;
        damageAmount = bossAI.bossInfo.Atk1Damage;
    }
    public override IEnumerator ExecutePattern(){
        yield return new WaitForSeconds(1.0f);
    }
}