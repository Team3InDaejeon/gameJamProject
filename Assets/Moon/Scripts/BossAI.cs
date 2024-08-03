using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BossAI : EnemyAI
{

    [SerializeField]
    private List<BossPattern> availablePatterns;
    private Queue<BossPattern> patternQueue = new Queue<BossPattern>();
    public bool isExecutingPattern = false;
    [SerializeField]
    private List<int> patternOrder;


    [Header("Boss Specific Settings")]
    public ScriptableBoss bossInfo;

    protected override void Start()
    {
        base.Start();
        // 보스 특정 초기화
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        Stat.SetMaxHealth(bossInfo.Health); 
        moveSpeed = bossInfo.moveSpeed; 
        foreach(var pattern in availablePatterns)
        {
            pattern.Init(this);
        }
        InitializePatternQueue();
    }
    private void InitializePatternQueue()
    {
        foreach (var pattern in patternOrder)
        {
            
            patternQueue.Enqueue(availablePatterns[pattern]);
        }
    }

    protected override void Update()
    {
        base.Update();
        Debug.Log(fsm.State);
        if (fsm.State == EnemyState.Walk)
        {
            Move(1);
        }
        if (!isExecutingPattern && patternQueue.Count > 0)
        {
            StartCoroutine(ExecuteNextPattern());
        }
        Debug.Log("Now Base Update()");
        if(Stat.GetCurrentHealth() <= 0)
        {
            SetDead();
        }
    }

    protected virtual IEnumerator ExecuteNextPattern()
    {
        isExecutingPattern = true;
        BossPattern nextPattern = patternQueue.Dequeue();
        Debug.Log("Can Execute: " + nextPattern.CanExecute());
        if (nextPattern.CanExecute())
        {
            yield return StartCoroutine(nextPattern.ExecutePattern());
        }

        yield return new WaitForSeconds(nextPattern.interval);
        if (patternQueue.Count == 0)
        {
            yield return StartCoroutine(OnPatternQueueEmpty());
        }

        isExecutingPattern = false;
    }

    protected virtual IEnumerator OnPatternQueueEmpty()
    {
        InitializePatternQueue();
        yield return null;
    }

    // Change to new Order of Patterns
    public void ChangePatternOrder(BossPattern[] newOrder)
    {
        patternQueue.Clear();
        foreach (var pattern in newOrder)
        {
            patternQueue.Enqueue(pattern);
        }
    }
    protected override void KnockBack(){}
    protected override void Move(float multiplier = 1)
    {
        base.Move(multiplier * 0.8f); 
    }
    protected override void SetDead()
    {
        base.SetDead();
        // 보스가 죽었을 때의 처리
        GameManager.Inst.PostBossDeath();
    }
}