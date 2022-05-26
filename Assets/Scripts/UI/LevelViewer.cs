using UnityEngine;
using TMPro;
using Scripts.Data;

public class LevelViewer : MonoBehaviour
{
    [SerializeField] private Game _game;

    private TextMeshProUGUI _levelText;

    private void Awake()
    {
        _levelText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _game.OnStartLevel += UpdateUI;
    }
    
    private void UpdateUI()
    {
        _levelText.text = "Level: " + PlayerProgress.GetData().Level;
    }
}