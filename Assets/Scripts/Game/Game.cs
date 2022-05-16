using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private Ball[] _balls;

    public bool EndGame { get; private set; }

    public static Game Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EndGame = false;

        _winPanel.SetActive(false);
        _failPanel.SetActive(false);

        for (int i = 0; i < _balls.Length; i++)
        {
            _balls[i].OnFinished += CheckWin;
            _balls[i].OnFail += Fail;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _balls.Length; i++)
        {
            _balls[i].OnFinished -= CheckWin;
            _balls[i].OnFail -= Fail;
        }
    }

    private void CheckWin()
    {
        bool isWin = _balls.Where(ball => ball.IsFinished == true).Count() == _balls.Length;

        if (isWin == true)
        {
            _winPanel.SetActive(true);
            EndGame = true;
        }
    }

    private void Fail()
    {
        _failPanel.SetActive(true);
        EndGame = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
