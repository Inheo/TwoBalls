using Scripts.Data;
using UnityEngine;

public class Game : MonoBehaviour, IStartCoroutine
{
    [SerializeField] private FadePanel _winPanel;
    [SerializeField] private FadePanel _failPanel;
    [SerializeField] private GameSettings _gameSettings;

    private SceneLoader _sceneLoader;

    public event System.Action<int> OnStartLevel;
    public event System.Action<int> OnFinishLevel;
    public event System.Action<int> OnFailLevel;

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
        OnStartLevel?.Invoke(PlayerProgress.GetData().Level);
        _winPanel.Hide(true);
        _failPanel.Hide(true);

        _sceneLoader.OnSceneLoaded += SceneLoaded;

        _sceneLoader.TryLoadLevel(_gameSettings[PlayerProgress.GetData().Level].SceneName);
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
        _winPanel.Show();
        OnFinishLevel?.Invoke(PlayerProgress.GetData().Level);
    }

    private void ShowFailPanel()
    {
        _failPanel.Show();
        OnFailLevel?.Invoke(PlayerProgress.GetData().Level);
    }

    public void RestartGame()
    {
        Unsubscribe();
        StartLevel();
    }
}