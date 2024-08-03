using UnityEngine;
using System.Collections;

public class Boss2Skill1 : BossPattern
{
    public override void Init(BossAI bossAI)
    {
        // target = bossAI.GetTarget();
        // bossTransform = bossAI.transform;
        // damageAmount = bossAI.bossInfo.Atk1Damage;
    }
    public override IEnumerator ExecutePattern()
    {
        yield return new WaitForSeconds(1.0f);
    }
}