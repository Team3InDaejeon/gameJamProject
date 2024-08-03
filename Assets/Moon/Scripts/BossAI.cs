using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BossAI : EnemyAI
{

    [SerializeField]
    private List<BossPattern> availablePatterns;
    private Queue<BossPattern> patternQueue = new Queue<BossPattern>();
    private bool isExecutingPattern = false;
    [SerializeField]
    private List<int> patternOrder;


    [Header("Boss Specific Settings")]
    public ScriptableBoss bossInfo;

    protected override void Start()
    {
        base.Start();
        // 보스 특정 초기화
        SetTarget(GameObject.FindGameObjectWithTag("Player").transform);
        Stat.SetMaxHealth(bossInfo.Health); // 예: 보스의 체력을 더 높게 설정
        moveSpeed = 2f; // 예: 보스의 이동 속도를 더 느리게 설정
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
        if (!isExecutingPattern && patternQueue.Count > 0)
        {
            StartCoroutine(ExecuteNextPattern());
        }
    }

    private IEnumerator ExecuteNextPattern()
    {
        isExecutingPattern = true;
        BossPattern nextPattern = patternQueue.Dequeue();

        if (nextPattern.CanExecute())
        {
            yield return StartCoroutine(nextPattern.ExecutePattern());
        }

        yield return new WaitForSeconds(nextPattern.interval);
        if(patternQueue.Count == 0)
        {
            yield return new WaitForSeconds(5.0f);
            InitializePatternQueue();
        }

        isExecutingPattern = false;
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

}