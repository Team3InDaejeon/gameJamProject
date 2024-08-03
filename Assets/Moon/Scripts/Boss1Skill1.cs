using UnityEngine;
using System.Collections;

public class Boss1Skill1 : BossPattern
{
    [HideInInspector]
    public Transform target;
    Transform bossTransform;
    public GameObject bossArmPrefab;  // 보스의 팔 Transform
    public float attackRange = 5f;
    [HideInInspector]
    public float damageAmount = 10f;
    public float armExtendSpeed = 5f;   // 팔을 뻗는 속도

    private Vector3 targetPosition;
    private Vector3 originalArmPosition;
    private Quaternion originalArmRotation;
    private float currentTime = 0;
    private float maintainTime = 0.5f;
    private float attackDelay = 1.0f;

    private GameObject bossArm;

    public override void Init(BossAI bossAI){
        target=bossAI.GetTarget();
        bossTransform=bossAI.transform;
        damageAmount=bossAI.bossInfo.Atk1Damage;
        bossArm = Instantiate(bossArmPrefab, bossTransform.position, Quaternion.identity);
        bossArm.GetComponent<Boss1Arm>().Init(bossAI.bossInfo.Atk1Damage,bossAI.bossInfo.Type);
        bossArm.SetActive(false);
    }

    public override IEnumerator ExecutePattern()
    {
        if (target == null || bossArmPrefab == null)
        {
            Debug.LogWarning("Target or BossArm is not set for Boss1Skill1");
            yield break;
        }
        if(bossArm==null){

            bossArm = Instantiate(bossArmPrefab, bossTransform.position, Quaternion.identity);
        }
        else{
            bossArm.SetActive(true);
        }

        // 초기 팔 위치와 회전 저장
        originalArmPosition = bossArm.transform.localPosition;
        originalArmRotation = bossArm.transform.localRotation;

        // 타겟 위치 지정
        targetPosition = target.position;

        // 보스가 타겟을 향해 회전
        Vector3 directionToTarget = (targetPosition - bossTransform.position).normalized;
        bossArm.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToTarget);

        // 타겟 위치 유지 및 조준
        while (currentTime < attackDelay)
        {
            currentTime += Time.deltaTime;
            // 시각적 효과 (예: 레이저 조준선)
            Debug.DrawLine(bossTransform.position, targetPosition, Color.red);
            yield return null;
        }

        // 공격 실행 (팔 뻗기)
        yield return StartCoroutine(ExtendArm());

        yield return new WaitForSeconds(maintainTime);

        // 팔 원위치
        yield return StartCoroutine(RetractArm());
    }

    private IEnumerator ExtendArm()
    {
        Vector3 directionToTarget = (targetPosition - bossTransform.position).normalized;
        Vector3 extendedPosition = bossArm.transform.position + directionToTarget * attackRange;

        while (bossArm.transform.position != extendedPosition)
        {
            bossArm.transform.position = Vector3.MoveTowards(bossArm.transform.position, extendedPosition, armExtendSpeed * Time.deltaTime);

            // 팔이 타겟을 향하도록 회전
            bossArm.transform.right = directionToTarget;

            yield return null;
        }
    }

    private IEnumerator RetractArm()
    {
        Vector3 startPosition = bossArm.transform.position;

        while (bossArm.transform.position != bossTransform.position)
        {
            bossArm.transform.position = Vector3.MoveTowards(bossArm.transform.position, bossTransform.position, armExtendSpeed * Time.deltaTime);

            // 팔이 보스 몸체로 돌아오는 동안 점진적으로 원래 회전으로 복귀
            bossArm.transform.rotation = Quaternion.Slerp(
                bossArm.transform.rotation,
                originalArmRotation,
                Vector3.Distance(bossArm.transform.position, startPosition) / Vector3.Distance(startPosition, bossTransform.position)
            );

            yield return null;
        }

        // 최종적으로 원래 위치와 회전값으로 복구
        bossArm.transform.localPosition = originalArmPosition;
        bossArm.transform.localRotation = originalArmRotation;
        bossArm.SetActive(false);
    }

    // private void ExecuteAttack()
    // {
    //     // 공격 로직
    //     RaycastHit2D hit = Physics2D.Raycast(bossArmTransform.position, bossArmTransform.right, attackRange);

    //     if (hit.collider != null && hit.collider.CompareTag("Player"))
    //     {
    //         PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
    //         if (playerHealth != null)
    //         {
    //             playerHealth.TakeDamage(damageAmount);
    //         }
    //     }

    //     // 공격 시각 효과
    //     Debug.DrawRay(bossArmTransform.position, bossArmTransform.right * attackRange, Color.yellow, 0.5f);
    // }
}