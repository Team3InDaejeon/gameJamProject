// using UnityEngine;
// using System.Collections.Generic;
// using System.Collections;
// public enum BossState { Idle, Walk, Attack, Death }

// public class BossAIFix : CharacterBase, ICombat
// {
//     [SerializeField]
//     private List<BossPattern> availablePatterns;
//     private Queue<BossPattern> patternQueue = new Queue<BossPattern>();
//     private bool isExecutingPattern = false;
//     [SerializeField]
//     private List<int> patternOrder;

//     [Header("Boss Specific Settings")]
//     public ScriptableBoss bossInfo;

//     private Transform target;
//     private Rigidbody2D rb;
//     private Animator animator;
//     private BoxCollider2D boxCollider;
//     private SpriteRenderer spriteRenderer;
//     private float moveSpeed;

//     private BossState currentState = BossState.Idle;

//     public EnemyType enemyType = EnemyType.Normal;

//     protected override void Start()
//     {
//         base.Start();
//         rb = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         boxCollider = GetComponent<BoxCollider2D>();
//         if (boxCollider == null)
//         {
//             Debug.LogError("BoxCollider2D is missing on the Boss object!");
//         }
//         spriteRenderer = GetComponent<SpriteRenderer>();
//         if (Stat != null)
//         {
//             moveSpeed = Stat.GetMoveSpeed();
//             ScriptableEnemy enemyInfo = Stat.GetCharacterInfo();
//             Stat.SetMaxHealth(enemyInfo.Health);
//             Stat.SetHealth(enemyInfo.Health);
//             enemyType = enemyInfo.Type;
//             Debug.Log($"Enemy Type : {enemyType}");
//         }
//         else
//         {
//             Debug.LogError("Stat is null on the Boss object!");
//         }
//         Initialize();
//     }

//     private void Initialize()
//     {
//         SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
//         InitializePatternQueue();
//         foreach (var pattern in availablePatterns)
//         {
//             pattern.Init(this);
//         }
//     }

//     private void InitializePatternQueue()
//     {
//         patternQueue.Clear();
//         foreach (var patternIndex in patternOrder)
//         {
//             patternQueue.Enqueue(availablePatterns[patternIndex]);
//         }
//     }

//     protected void Update()
//     {
//         UpdateState();
//         if (currentState == BossState.Walk)
//         {
//             Move();
//         }
//         if (!isExecutingPattern && patternQueue.Count > 0)
//         {
//             StartCoroutine(ExecuteNextPattern());
//         }
//     }

//     private void UpdateState()
//     {
//         if (target == null) return;

//         float distanceToTarget = Vector2.Distance(transform.position, target.position);
//         if (distanceToTarget > attackRange)
//         {
//             currentState = BossState.Walk;
//         }
//         else
//         {
//             currentState = BossState.Attack;
//         }
//     }

//     protected virtual IEnumerator ExecuteNextPattern()
//     {
//         isExecutingPattern = true;
//         BossPattern nextPattern = patternQueue.Dequeue();
//         Debug.Log("Executing pattern: " + nextPattern.GetType().Name);
//         if (nextPattern.CanExecute())
//         {
//             yield return StartCoroutine(nextPattern.ExecutePattern());
//         }

//         yield return new WaitForSeconds(nextPattern.interval);
//         if (patternQueue.Count == 0)
//         {
//             yield return StartCoroutine(OnPatternQueueEmpty());
//         }

//         isExecutingPattern = false;
//     }

//     protected virtual IEnumerator OnPatternQueueEmpty()
//     {
//         InitializePatternQueue();
//         yield return null;
//     }

//     public void ChangePatternOrder(BossPattern[] newOrder)
//     {
//         patternQueue.Clear();
//         foreach (var pattern in newOrder)
//         {
//             patternQueue.Enqueue(pattern);
//         }
//     }

//     protected virtual void Move()
//     {
//         if (target != null)
//         {
//             Vector2 direction = (target.position - transform.position).normalized;
//             rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
//             if (animator != null)
//             {
//                 animator.SetFloat("walkSpeed", direction.magnitude);
//             }
//         }
//     }

//     public void SetTarget(Transform newTarget)
//     {
//         target = newTarget;
//     }

//     public IEnumerator FindTarget(float searchRange, float searchTime, BossState newState)
//     {
//         while (true)
//         {
//             Collider2D[] collidersInSightRange = Physics2D.OverlapCircleAll(transform.position, searchRange - .1f, LayerMask.GetMask("Player"));
//             if (collidersInSightRange.Length > 0)
//             {
//                 target = collidersInSightRange[0].transform;
//                 currentState = newState;
//                 yield break;
//             }

//             yield return new WaitForSeconds(searchTime);
//         }
//     }

//     public virtual void TakeDamage(int damage, EnemyType enemyType = EnemyType.Normal)
//     {
//         Stat.DecreaseHealth(damage);
//         Debug.Log("Boss Health : " + Stat.GetHealth());
//         KnockBack();

//         if (Stat.GetHealth() <= 0)
//         {
//             currentState = BossState.Death;
//             // 죽음 처리 로직 추가
//         }
//         else
//         {
//             StartCoroutine(TakeDamageEffect());
//         }
//     }

//     protected virtual void KnockBack()
//     {
//         // 넉백 로직 구현
//     }

//     private IEnumerator TakeDamageEffect()
//     {
//         // 피해 효과 로직 구현
//         yield return null;
//     }

//     protected override void SetDead()
//     {
//         // 사망 처리 로직 구현
//     }

//     // ICombat 인터페이스 구현 메서드들 추가 (필요한 경우)
// }