using UnityEngine;
using System.Collections;

public abstract class BossPattern : MonoBehaviour
{
    public float interval=1f;

    public abstract IEnumerator ExecutePattern();
    public abstract void Init(BossAI bossAI);

    public virtual bool CanExecute()
    {
        return true; // 기본적으로 항상 실행 가능
    }
}