using UnityEngine;
using System.Collections;

public class Boss2 : BossAI
{
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;
    private Vector2 wanderTarget;

    [SerializeField]
    private float playerDetectionRange = 10f;

    private bool isExecuting = false;

    protected override void Start()
    {
        base.Start();
        SetNewWanderTarget();
        StartCoroutine(WanderRoutine());
    }

    protected override void Update()
    {
        if (!isExecuting)
        {
            base.Update(); // 패턴 실행을 위한 기본 Update 호출
        }
        if (IsPlayerInAttackRange())
        {
            
            
        }
        else if (IsPlayerInDetectionRange())
        {
            if (!isExecuting)
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            if (!isExecuting)
            {
                fsm.ChangeState(EnemyState.Walk);
                WanderMovement();
            }
        }
    }

    private bool IsPlayerInDetectionRange()
    {
        return GetTarget() != null && Vector2.Distance(transform.position, GetTarget().position) <= playerDetectionRange;
    }

    private bool IsPlayerInAttackRange()
    {
        return GetTarget() != null && Vector2.Distance(transform.position, GetTarget().position) <= attackRange;
    }

    private void StopMovement()
    {
        // 여기서 애니메이션을 정지 상태로 변경할 수 있습니다.
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (GetTarget() != null)
        {
            Vector2 direction = ((Vector2)GetTarget().position - (Vector2)transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }

    private void WanderMovement()
    {
        Vector2 movement = Vector2.MoveTowards(transform.position, wanderTarget, moveSpeed * Time.deltaTime) - (Vector2)transform.position;
        transform.Translate(movement);
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(wanderInterval);
            if (!IsPlayerInDetectionRange())
            {
                SetNewWanderTarget();
            }
        }
    }

    private void SetNewWanderTarget()
    {
        float randomAngle = Random.Range(0f, 360f);
        wanderTarget = (Vector2)transform.position + (Vector2)(Quaternion.Euler(0, 0, randomAngle) * Vector2.right * wanderRadius);
        Debug.Log($"New Wander Target: {wanderTarget}");
    }

    protected override IEnumerator ExecuteNextPattern()
    {
        isExecuting = true;
        yield return StartCoroutine(base.ExecuteNextPattern());
        isExecuting = false;
    }

    // 필요한 경우 Move 메서드 오버라이드
    protected override void Move(float multiplier = 1)
    {
        if (!isExecuting)
        {
            base.Move(multiplier);
        }
        else
        {
            StopMovement();
        }
    }
}