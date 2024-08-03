using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    private static GameSceneManager gameSceneManager;
    public static GameSceneManager Inst
    {
        get
        {
            if (gameSceneManager == null)
            {
                gameSceneManager = FindObjectOfType<GameSceneManager>();
                if (gameSceneManager == null)
                {
                    Debug.LogError("GameSceneManager does Not Exist!");
                }
            }
            return gameSceneManager;
        }
    }
    void Awake(){
        if(gameSceneManager==null){
            gameSceneManager=this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    string sceneToLoad="Stage_01";
    public void ChangeScene(string sceneName)
    {
        SetSceneToLoad(sceneName);
        StartCoroutine(LoadSceneAsync());
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetSceneToLoad(string sceneName)
    {
        sceneToLoad = sceneName;
    }
    private IEnumerator LoadSceneAsync()
    {
        // 먼저 로딩 씬을 로드합니다.
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("LoadingScene");
        while (!loadingOperation.isDone)
        {
            yield return null;
        }

        // LoadingSceneManager가 로드되길 기다립니다.
        yield return new WaitForSeconds(0.1f);

        // LoadingSceneManager에게 다음 씬 정보를 전달합니다.
        LoadingSceneManager loadingManager = FindObjectOfType<LoadingSceneManager>();
        if (loadingManager != null)
        {
            loadingManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("LoadingSceneManager not found in the Loading Scene!");
        }
    }
}
