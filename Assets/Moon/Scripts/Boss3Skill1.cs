using UnityEngine;
using System.Collections;

public class Boss3Skill1 : BossPattern
{
    [HideInInspector]
    public Transform target;
    Transform bossTransform;
    public GameObject slashPrefab;  // 큰 검기 프리팹
    public float attackRange = 30f;  // 검기가 이동할 거리
    [HideInInspector]
    public int damageAmount = 10;
    public float slashSpeed = 15f;   // 검기 이동 속도
    public float slashHeight = 20f;  // 검기의 높이 (맵 Y축을 덮을 만큼 큰 값)
    Animator animator;
    public override void Init(BossAI bossAI)
    {
        target = bossAI.GetTarget();
        bossTransform = bossAI.transform;
        damageAmount = bossAI.bossInfo.Atk1Damage;
        animator=bossAI.animator;
    }

    public override IEnumerator ExecutePattern()
    {
        if (target == null || slashPrefab == null)
        {
            Debug.LogWarning("Target or SlashPrefab is not set for Boss3Skill1");
            yield break;
        }


        // 검기 발사
        yield return StartCoroutine(LaunchSlash());
    }

    private IEnumerator LaunchSlash()
    {
        animator.SetTrigger("AttackTrigger");
        Vector3 directionToTarget = (target.position - bossTransform.position).normalized;

        // 검기 생성 및 초기 설정
        GameObject slash = Instantiate(slashPrefab, bossTransform.position, Quaternion.identity);
        slash.transform.right = directionToTarget;  // 검기의 방향을 타겟 쪽으로 설정

        // 검기에 데미지 정보 전달 (필요한 경우)
        Slash slashComponent = slash.GetComponent<Slash>();
        if (slashComponent != null)
        {
            slashComponent.SetDamage(damageAmount);
        }

        // 검기 크기 초기화
        slash.transform.localScale = Vector3.zero;

        // 0.5초 동안 검기 크기 증가
        float growDuration = 0.3f;
        Vector3 targetScale = new Vector3(1f, 1f, 1f);
        float elapsedTime = 0f;

        while (elapsedTime < growDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / growDuration;
            slash.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }

        // 최종 크기 설정
        slash.transform.localScale = targetScale;

        // 검기 이동
        float distanceTraveled = 0;
        while (distanceTraveled < attackRange)
        {
            slash.transform.Translate(Vector3.right * slashSpeed * Time.deltaTime);
            distanceTraveled += slashSpeed * Time.deltaTime;

            // 이동하면서 점점 투명해지게 설정
            float alpha = 1 - (distanceTraveled / attackRange);
            SetSlashAlpha(slash, alpha);

            yield return null;
        }

        // 검기 제거
        Destroy(slash);
    }

    // 검기의 알파값을 설정하는 헬퍼 메서드
    private void SetSlashAlpha(GameObject slash, float alpha)
    {
        SpriteRenderer spriteRenderer = slash.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
    }
}