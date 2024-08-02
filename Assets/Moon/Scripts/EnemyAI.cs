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
public class EnemyAI : MonoBehaviour
{
    [Header("Stat Setting")]
    public float searchRange = 5;
    public float attackRange = 2;
    public float moveSpeed=1;
    public float cooltime = 1;
    public float jumpPower = 1;
    public EnemyType enemyType=EnemyType.Red;
    //AI Setting
    public float timeForMoving = 10;


    StateMachine<EnemyState> fsm;
    public Rigidbody2D rb;
    Collider2D collider2d;
    Coroutine curCoroutine;
    Transform target;
    Vector2 randomPoint;
    float currentTimeForMoving=0;
    float currentAttackTime=0;
    bool isGrounded=true;

    private void Awake()
    {
        fsm = new StateMachine<EnemyState>(this);
        fsm.ChangeState(EnemyState.Idle);
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }
    void Update(){
        fsm.Driver.Update.Invoke();
    }
    void FixedUpdate()
    {
        // 현재 떨어지고 있는 경우에만 isGrounded를 업데이트
        if (rb.velocity.y <= 0)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector2.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null && rayHit.distance < 0.6f)
            {
                isGrounded = true;
            }
            else
            {
                isGrounded = false;
            }
        }
    }
    void Initialize()
    {
        randomPoint=transform.position;
        fsm.ChangeState(EnemyState.Idle);
    }
    void Idle_Enter()
    {
        Debug.Log("Idle Start");
        StartCoroutine(FindTarget(searchRange, 1, EnemyState.Walk));
        //TODO : Enemy Idle Animation Start
    }
    void Idle_Update()
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
            MoveTowardsTarget(randomPoint);
        }
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
            MoveTowardsTarget(target.position);
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
    void MoveTowardsTarget(Vector2 target)
    {
        Vector2 targetPosition = new Vector2(target.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        //Give offset to the target position, so that the enemy will not jump every single time
        if (target.y > transform.position.y + 0.5f && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
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
        Gizmos.DrawRay(rb.position, Vector3.down); //ray를 그리기
    }
    
    
}
