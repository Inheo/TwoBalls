using UnityEngine;
using TMPro;

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
    
    private void UpdateUI(int level)
    {
        _levelText.text = LEVEL_PARAMETER + (level + 1);
    }
}