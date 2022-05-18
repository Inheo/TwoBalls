using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private string _currentScene;

    public event Action OnSceneLoaded;

    private  IStartCoroutine _coroutineStarter;

    public SceneLoader(IStartCoroutine coroutineStarter)
    {
        _coroutineStarter = coroutineStarter;
    }

    public bool TryLoadLevel(string newScene)
    {
        if (!string.IsNullOrEmpty(_currentScene))
        {
            SceneManager.UnloadSceneAsync(_currentScene, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
        }

        _currentScene = newScene;
        _coroutineStarter.StartCoroutine(LoadScene(_currentScene));
        return true;
    }

    private IEnumerator LoadScene(string sceneName)
    {
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= SceneLoaded;
        OnSceneLoaded?.Invoke();
    }
}