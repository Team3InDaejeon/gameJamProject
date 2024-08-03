using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : BossAI
{
    public GameObject StunPrefab;

    protected override IEnumerator OnPatternQueueEmpty()
    {
        yield return StartCoroutine(ApplyStun());
        yield return base.OnPatternQueueEmpty();
    }

    private IEnumerator ApplyStun()
    {
        StunPrefab.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        StunPrefab.SetActive(false);
    }
}