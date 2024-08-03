using UnityEngine;
using System.Collections;

public class Boss3Skill1 : BossPattern
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

        Vector3 originalPosition = bossArm.transform.position;
        Vector3 extendDirection = bossArm.transform.TransformDirection(Vector3.left);
        Vector3 extendedPosition = originalPosition + extendDirection * 15;

        float elapsedTime = 0f;
        float extendDuration = attackRange / armExtendSpeed; // 팔을 뻗는 데 걸리는 시간 계산

        while (elapsedTime < extendDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / extendDuration; // 0에서 1 사이의 값

            // 월드 좌표계에서 팔을 선형적으로 확장
            bossArm.transform.position = Vector3.Lerp(originalPosition, extendedPosition, t);

            yield return null;
        }

        // 최종 위치 보정
        bossArm.transform.position = extendedPosition;
    }

    private IEnumerator RetractArm()
    {
        Vector3 extendedPosition = bossArm.transform.position;
        Quaternion extendedRotation = bossArm.transform.rotation;

        Vector3 retractDirection = bossArm.transform.TransformDirection(Vector3.right); // 팔을 접는 방향
        Vector3 targetPosition = extendedPosition + retractDirection * attackRange; // 원래 위치로 돌아가기

        float elapsedTime = 0f;
        float retractDuration = attackRange / armExtendSpeed;

        while (elapsedTime < retractDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / retractDuration; // 0에서 1 사이의 값

            // 위치를 선형적으로 보간
            bossArm.transform.position = Vector3.Lerp(extendedPosition, targetPosition, t);

            // 회전을 구면 선형 보간(Slerp)으로 부드럽게 변경
            bossArm.transform.rotation = Quaternion.Slerp(extendedRotation, originalArmRotation, t);

            yield return null;
        }

        // 최종 위치와 회전 보정
        bossArm.transform.position = targetPosition;
        bossArm.transform.rotation = originalArmRotation;
        bossArm.transform.localPosition = originalArmPosition; // 로컬 위치도 원래대로 복원
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