using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 firstLoadPosition = new Vector3(0,0,0);
    public Vector3 menuPosition = new Vector3(0,0,0);

    [Header("事件监听")]
    public SceneLoadEvent SceneLoadEvent;
    public VoidEventSO newGameEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;

    [Header("场景转换广播")]
    public VoidEventSO afterSceneLoadedEvent;
    public FadeEventSO fadeEvent;

    [Header("场景")]
    public GameSceneSO startMenuScene;
    public GameSceneSO gameOverScene;
    public GameSceneSO mainGameScene;
    private GameSceneSO currentLoadScene;
    private GameSceneSO sceneToLoad;
    private Vector3 positionToGo;

    [Header("场景转换属性")]
    public float fadeDuration;
    public bool isLoading;
    private bool fadeScreen;

    private void Awake()
    {

    }

    private void Start()
    {
        SceneLoadEvent.RaiseLoadRequestEvent(startMenuScene, menuPosition, true);        
    }

    private void Update()
    {
    }

    private void OnEnable()
    {
        SceneLoadEvent.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        gameOverEvent.OnEventRaised += GameOver;
        backToMenuEvent.OnEventRaised += BackToMenu;
    }

    private void OnDisable()
    {
        SceneLoadEvent.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        gameOverEvent.OnEventRaised -= GameOver;
        backToMenuEvent.OnEventRaised -= BackToMenu;
    }

    private void NewGame()
    {
        Time.timeScale = 1;
        sceneToLoad = mainGameScene;
        OnLoadRequestEvent(sceneToLoad, firstLoadPosition, true);
    }

    private void GameOver()
    {
        Time.timeScale = 1;
        sceneToLoad = gameOverScene;
        OnLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    private void BackToMenu()
    {
        Time.timeScale = 1;
        sceneToLoad = startMenuScene;
        OnLoadRequestEvent(sceneToLoad, menuPosition, true);
    }

    #region 场景加载事件请求
    private void OnLoadRequestEvent(GameSceneSO sceneToLoad, Vector3 posToGo, bool fadeScreen)
    {
        if(isLoading)
        {
            return;
        }

        isLoading = true;
        this.sceneToLoad = sceneToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen;

        if (currentLoadScene != null)
        {
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            LoadNewScene();
        }
    }

    private IEnumerator UnLoadPreviousScene()
    {
        if(fadeScreen)
        {
            fadeEvent.fadeIn(fadeDuration);
        }

        yield return new WaitForSeconds(fadeDuration);

        currentLoadScene.sceneReference.UnLoadScene();
        playerTransform.gameObject.SetActive(false);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        var loadingOption =  sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = sceneToLoad;
        playerTransform.position = positionToGo;
        playerTransform.gameObject.SetActive(true);
        Scene sceneToActive = handle.Result.Scene;
        if (sceneToActive.IsValid())
        {
            SceneManager.SetActiveScene(sceneToActive);
        }

        if (fadeScreen)
        {
            fadeEvent.fadeOut(fadeDuration);
        }
        isLoading = false;
        //通知所有需要执行场景转换后有操作的物体
        afterSceneLoadedEvent?.OnEventRaised();
    }
    #endregion
}
