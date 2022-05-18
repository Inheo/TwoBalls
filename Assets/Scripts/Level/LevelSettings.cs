using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Create Level Settings", fileName = "Level", order = 0)]
public class LevelSettings : ScriptableObject
{
    [SerializeField] private string _sceneName;

    public string SceneName => _sceneName;
}