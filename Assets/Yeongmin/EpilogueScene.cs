using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneEpilogue : MonoBehaviour
{
    [SerializeField]
    List<Sprite> EpilogueImages;
    [SerializeField]
    Image Image_Epilogue;
    int PageIndex = 0;

    public void NextPage()
    {
        ++PageIndex;

        if (Image_Epilogue != null && EpilogueImages[PageIndex] != null)
        {
            Image_Epilogue.sprite = EpilogueImages[PageIndex];
        }

        if (PageIndex > EpilogueImages.Count) 
        {
            GameSceneManager.Inst.ChangeScene("Intro");
        }
    }
}
