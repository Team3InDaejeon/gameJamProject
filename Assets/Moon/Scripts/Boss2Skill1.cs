using UnityEngine;
using System.Collections;

public class Boss2Skill1 : BossPattern
{
    public float damageRadius = 15f;
    public int damageAmount = 50;
    public float pauseDuration = 1f;
    public float damageDuration = 5f;

    public GameObject damageEffect;

    private BossAI bossAI;

    public override void Init(BossAI boss)
    {
        bossAI = boss;
        damageAmount = bossAI.bossInfo.Atk1Damage;
    }

    public override bool CanExecute()
    {
        // 플레이어가 보스의 범위 내에 있는지 확인
        float distanceToPlayer = Vector3.Distance(bossAI.transform.position, bossAI.GetTarget().position);
        return distanceToPlayer <= damageRadius;
    }

    public override IEnumerator ExecutePattern()
    {
        // 1초간 멈춤
        Debug.Log("Start Skill 1");
        float originalMoveSpeed = bossAI.GetStatComponent().GetMoveSpeed();
        bossAI.GetStatComponent().SetMoveSpeed(0);
        yield return new WaitForSeconds(pauseDuration);

        // 데미지 영역 표시 (예: 파티클 시스템 활성화)
        damageEffect.SetActive(true);
        damageEffect.GetComponent<ParticleSystem>().Stop();
        damageEffect.GetComponent<ParticleSystem>().Play();

        // 1초간 데미지 주기
        float elapsedTime = 0f;
        while (elapsedTime < damageDuration)
        {
            Collider[] hitColliders = Physics.OverlapSphere(bossAI.transform.position, damageRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    Debug.Log("Hit Player");
                    ICombat damageable = hitCollider.GetComponent<ICombat>();
                    if (damageable != null)
                    {
                        damageable.TakeDamage(damageAmount/5, bossAI.bossInfo.Type);
                    }
                    CharacterStat playerStat = hitCollider.GetComponent<CharacterStat>();
                    Debug.Log(playerStat.GetCurrentHealth());
                }
            }
            
            yield return new WaitForSeconds(damageDuration / 5);
            elapsedTime += damageDuration / 5;
        }

        // 보스 움직임 다시 활성화
        bossAI.GetStatComponent().SetMoveSpeed(originalMoveSpeed);
        damageEffect.SetActive(false);

    }
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}