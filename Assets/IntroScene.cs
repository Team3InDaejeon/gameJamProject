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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public IEnumerator FadeIn()
    {
        image.DOFade(0, 1f);
        yield return new WaitForSeconds(1f);
    }
}
    
