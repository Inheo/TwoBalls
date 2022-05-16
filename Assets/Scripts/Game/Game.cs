using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _failPanel;
    [SerializeField] private Ball[] _balls;

    private void Start()
    {
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
        }
    }

    private void Fail()
    {
        _failPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
