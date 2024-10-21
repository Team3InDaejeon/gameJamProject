using UnityEngine;
using System.Collections;

public class Boss3 : BossAI
{
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;
    private Vector2 wanderTarget;

    private bool isWandering = false;

    protected override void Start()
    {
        base.Start();
        SetNewWanderTarget();
        StartCoroutine(WanderRoutine());
    }

    protected override void Update()
    {
        base.Update(); // BossAI의 Update 로직 유지

        if (!isExecutingPattern && isWandering)
        {
            Move(1); // WanderMovement 대신 Move 사용
        }
    }

    private IEnumerator WanderRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(wanderInterval);
            if (!isExecutingPattern)
            {
                SetNewWanderTarget();
                isWandering = true;
            }
        }
    }

    private void SetNewWanderTarget()
    {
        float randomAngle = Random.Range(0f, 360f);
        wanderTarget = (Vector2)transform.position + (Vector2)(Quaternion.Euler(0, 0, randomAngle) * Vector2.right * wanderRadius);
    }

    protected override IEnumerator ExecuteNextPattern()
    {
        isWandering = false; // 패턴 실행 시 배회 중지
        yield return StartCoroutine(base.ExecuteNextPattern());
        isWandering = true; // 패턴 실행 후 배회 재개
    }

    protected override void Move(float multiplier = 1)
    {
        if (isWandering)
        {
            Vector2 direction = ((Vector2)wanderTarget - (Vector2)transform.position).normalized;
            Vector2 movement = direction * moveSpeed * multiplier * Time.deltaTime;

            if (rb != null)
            {
                rb.MovePosition(rb.position + movement);
            }
            else
            {
                transform.position += (Vector3)movement;
            }

            if (Vector2.Distance(transform.position, wanderTarget) < 0.1f)
            {
                SetNewWanderTarget();
            }
        }
        else
        {
            base.Move(multiplier * 0.8f);
        }
    }

    // 디버그를 위한 메서드
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, wanderTarget);
    }
}