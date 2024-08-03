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
    private float aimingTime = 4.0f;

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
        

       
        // 타겟 위치 유지 및 조준
        while (currentTime < aimingTime)
        {
            currentTime += Time.deltaTime;
            targetPosition = target.position;
            // 보스가 타겟을 향해 회전
            Vector3 directionToTarget = (targetPosition - bossTransform.position).normalized;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            Debug.Log(angle);
            bossArm.transform.rotation = Quaternion.Euler(0, 0, angle-180);
            // 시각적 효과 (예: 레이저 조준선)
            Debug.DrawLine(bossTransform.position, targetPosition, Color.red);
            yield return null;
        }
        currentTime=0;
        yield return new WaitForSeconds(attackDelay);
        // 공격 실행 (팔 뻗기)
        yield return StartCoroutine(ExtendArm());

        yield return new WaitForSeconds(maintainTime);

        // 팔 원위치
        yield return StartCoroutine(RetractArm());
    }

    private IEnumerator ExtendArm()
    {
        Vector3 directionToTarget = (targetPosition - bossTransform.position).normalized;

        // 먼저 팔의 회전을 설정합니다
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
        bossArm.transform.rotation = Quaternion.Euler(0, 0, angle - 180);

        Vector3 originalLocalPosition = bossArm.transform.localPosition;
        Vector3 extendedLocalPosition = originalLocalPosition + Vector3.left * 10;

        float elapsedTime = 0f;
        float extendDuration = attackRange / armExtendSpeed; // 팔을 뻗는 데 걸리는 시간 계산

        while (elapsedTime < extendDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / extendDuration; // 0에서 1 사이의 값

            // 로컬 X축을 따라 팔을 선형적으로 확장
            bossArm.transform.localPosition = Vector3.Lerp(originalLocalPosition, extendedLocalPosition, t);

            yield return null;
        }

        // 최종 위치 보정
        bossArm.transform.localPosition = extendedLocalPosition;
    }

    private IEnumerator RetractArm()
    {
        Vector3 extendedLocalPosition = bossArm.transform.localPosition;
        Quaternion extendedRotation = bossArm.transform.localRotation;

        float elapsedTime = 0f;
        float retractDuration = Vector3.Distance(extendedLocalPosition, originalArmPosition) / armExtendSpeed;

        while (elapsedTime < retractDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / retractDuration; // 0에서 1 사이의 값

            // 위치를 선형적으로 보간
            bossArm.transform.localPosition = Vector3.Lerp(extendedLocalPosition, originalArmPosition, t);

            // 회전을 구면 선형 보간(Slerp)으로 부드럽게 변경
            bossArm.transform.localRotation = Quaternion.Slerp(extendedRotation, originalArmRotation, t);

            yield return null;
        }
        
        // 최종 위치와 회전 보정
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