using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
public class IntroScene : MonoBehaviour
{
    public Image image;
    public TMP_Text teamText;
    public TMP_Text start;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        Sequence MySequence = DOTween.Sequence();
        MySequence.Append(MySequence2())
        .OnComplete(() => { start.DOFade(1, 1.5f); });
        button.onClick.AddListener(() => { 
                GameSceneManager.Inst.ChangeScene("Stage_01"); 
                Debug.Log("Clicked");
        });
    }
    Sequence MySequence2()
    {
        return DOTween.Sequence()
        .Append(teamText.DOFade(1, 1.5f))
        .Append(teamText.DOFade(0, 1.5f))
        .Append(image.DOFade(1, 1.5f));
        
    }
    

}
    
