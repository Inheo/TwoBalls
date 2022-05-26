using UnityEngine;
using TMPro;
using Scripts.Data;

public class LevelViewer : MonoBehaviour
{
    private const string LEVEL_PARAMETER = "Level: ";
    [SerializeField] private Game _game;

    private TextMeshProUGUI _levelText;

    private void Awake()
    {
        _levelText = GetComponent<TextMeshProUGUI>();
        _game.OnStartLevel += UpdateUI;
    }
    
    private void UpdateUI()
    {
        _levelText.text = LEVEL_PARAMETER + (PlayerProgress.GetData().Level + 1);
    }
}