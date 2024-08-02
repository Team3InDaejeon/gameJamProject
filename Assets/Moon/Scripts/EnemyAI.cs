using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;
using Unity.VisualScripting;
public enum EnemyState
{
    Idle,
    Walk,
    Attack,
    Death
}
public enum EnemyType
{
    Red,
    Normal,
    Blue
}
public class EnemyAI : CharacterBase
{
    [Header("Stat Setting")]
    [SerializeField]
    protected float searchRange = 5;
    [SerializeField]
    protected float attackRange = 2;
    [SerializeField]
    protected float cooltime = 1;
    [SerializeField]
    protected float jumpPower = 1;
    [SerializeField]
    protected float moveSpeed=3;
    public EnemyType enemyType=EnemyType.Red;
    private BoxCollider2D boxCollider;

    //AI Setting
    public float timeForMoving = 10;

    [Header("Test Setting")]
    public float rayLength=1.2f;

    StateMachine<EnemyState> fsm;
    public Rigidbody2D rb;
    Transform target;
    Vector2 randomPoint;
    Animator animator;
    float currentTimeForMoving=0;
    float currentAttackTime=0;
    bool isGrounded=true;
    float speedMultiplier=1;

    //MonoBehaviour Functions
    private void Awake()
    {
        fsm = new StateMachine<EnemyState>(this);
        fsm.ChangeState(EnemyState.Idle);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator=GetComponent<Animator>();
        boxCollider=GetComponent<BoxCollider2D>();
        Debug.Log("is Stat null? : "+Stat==null);
        if(Stat!=null){
            moveSpeed=Stat.GetMoveSpeed();
        }
    }
    void Update(){
        fsm.Driver.Update.Invoke();
    }
    void FixedUpdate()
    {
        // 바운딩 박스의 바닥 부분의 중심 좌표 계산
        Vector2 origin = boxCollider.bounds.center;
        origin.y = boxCollider.bounds.min.y;

        // Raycast 발사
        isGrounded = Physics2D.Raycast(origin, Vector2.down, rayLength, LayerMask.GetMask("Platform"));

        Debug.Log(isGrounded);

        // 디버그 Ray 그리기
        Debug.DrawRay(origin, Vector2.down * rayLength, Color.red);
    }
    void Initialize()
    {
        randomPoint=transform.position;
        fsm.ChangeState(EnemyState.Idle);
    }
    //State Functions
    void Idle_Enter()
    {
        Debug.Log("Idle Start");
        StartCoroutine(FindTarget(searchRange, 1, EnemyState.Walk));
        //TODO : Enemy Idle Animation Start
    }
    void Idle_Update()
    {
        Idle();
    }
    
    void Walk_Enter()
    {
        Debug.Log("Walk Start");
        StartCoroutine(FindTarget(attackRange, .1f, EnemyState.Attack));
        //TODO : Enemy Walk Animation Start
    }
    void Walk_Update(){
        if(Vector2.Distance(transform.position, target.position) > searchRange + 1f){
            fsm.ChangeState(EnemyState.Idle);
            target=null;
        }else{
            Move(speedMultiplier);
        }
    }
    void Attack_Enter()
    {
        Debug.Log("Attack Start");
        //TODO : Enemy Attack Animation Start
    }
    void Attack_Update(){
        if(currentAttackTime>=cooltime){
            //TODO : Enemy Attack
            currentAttackTime=0;
        }
        else if(target != null && Vector2.Distance(transform.position, target.position) > attackRange+1f)
        {
            fsm.ChangeState(EnemyState.Walk);
        }
        else{
            currentAttackTime+=Time.deltaTime;
        }
    }

    void Death_Enter()
    {
        Debug.Log("Death Start");
        //TODO : Enemy Death Animation Start
        Destroy(gameObject,2f);
    }

    //Utility Functions
    protected override void Idle()
    {
        if (target != null)
        {
            fsm.ChangeState(EnemyState.Walk);
        }
        else if (currentTimeForMoving >= timeForMoving)
        {
            currentTimeForMoving = 0;
            randomPoint = new Vector2(Random.Range(-5, 5), transform.position.y);
        }
        else
        {
            currentTimeForMoving += Time.deltaTime;
            Move(speedMultiplier);
        }
    }
    protected override void Move(float multiplier = 1)
    {
        // If a target exists, move towards it; otherwise, move towards a random point
        if (target != null)
        {
            MoveTowardsTarget(target.position, multiplier);
        }
        else
        {
            MoveTowardsTarget(randomPoint, multiplier);
        }
    }
    void MoveTowardsTarget(Vector2 target, float multiplier = 1)
    {
        Vector2 targetPosition = new Vector2(target.x, transform.position.y);
        float adjustedSpeed = moveSpeed * multiplier; // Adjust speed with multiplier
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, adjustedSpeed * Time.deltaTime);

        // Give offset to the target position, so that the enemy will not jump every single time
        if (target.y > transform.position.y + 0.5f && isGrounded)
        {
            Jump();
        }
    }

    public IEnumerator FindTarget(float searchRange, float searchTime, EnemyState newState)
    {
        while (true)
        {
            Collider2D[] collidersInSightRange = Physics2D.OverlapCircleAll(transform.position, searchRange - .1f, LayerMask.GetMask("Player"));
            if (collidersInSightRange.Length > 0)
            {
                target = collidersInSightRange[0].transform;
                fsm.ChangeState(newState);
                yield break;
            }

            yield return new WaitForSeconds(searchTime);
        }
    }
    

    protected override void Jump()
    {
        isGrounded = false;
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(randomPoint, 1);
    }
    
    
}
