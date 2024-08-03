using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    public Slider slider;   
    public string SceneName;

    private float time;

    void Start()
    {
        StartCoroutine(LoadAsynSceneCoroutine());
    }

    public void LoadScene(string sceneName)
    {
        SceneName = sceneName;
    }   

    IEnumerator LoadAsynSceneCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            time =+ Time.time;
            slider.value = time/5f;
            if (time > 5)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    

}
