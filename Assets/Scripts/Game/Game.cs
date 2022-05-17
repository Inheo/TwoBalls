using UnityEngine;

public class Game : MonoBehaviour, IStartCoroutine
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _failPanel;

    private const string GAME_PARAMETER = "Game";
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = new SceneLoader(this);
    }

    private void Start()
    {
        StartLevel();
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void StartLevel()
    {
        _winPanel.SetActive(false);
        _failPanel.SetActive(false);

        _sceneLoader.OnSceneLoaded += SceneLoaded;

        _sceneLoader.TryLoadLevel(GAME_PARAMETER);
    }

    private void SceneLoaded()
    {
        _sceneLoader.OnSceneLoaded -= SceneLoaded;
        Subscribe();
    }

    private void Subscribe()
    {
        Level.Instance.OnLevelComplete += ShowWinPanel;
        Level.Instance.OnLevelFail += ShowFailPanel;
    }

    private void Unsubscribe()
    {
        Level.Instance.OnLevelComplete -= ShowWinPanel;
        Level.Instance.OnLevelFail -= ShowFailPanel;
    }

    private void ShowWinPanel()
    {
        _winPanel.SetActive(true);
    }

    private void ShowFailPanel()
    {
        _failPanel.SetActive(true);
    }

    public void RestartGame()
    {
        Unsubscribe();
        StartLevel();
    }
}