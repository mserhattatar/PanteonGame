using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public static Action FinisLineAction;

    [SerializeField] private GameObject loadingCanvas;


    private void Start()
    {
        StartNextLevel();
        Screen.SetResolution(2960, 1440, true);
    }


#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX || UNITY_WEBGL
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
#endif


    protected internal void StartNextLevel()
    {
        StartCoroutine(LoadYourAsyncScene());
    }

    private IEnumerator LoadYourAsyncScene()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        activeScene ++;
        if (activeScene == SceneManager.sceneCountInBuildSettings)
            activeScene = 1;
       
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(activeScene);

        // Wait until the asynchronous scene fully loads
        yield return new WaitUntil(() => asyncLoad.isDone);

        //wait for shaders
        yield return new WaitForSeconds(1f);
        loadingCanvas.SetActive(false);
    }
}