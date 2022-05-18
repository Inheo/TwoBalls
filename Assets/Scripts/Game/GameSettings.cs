using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Create Game Settings", fileName = "GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    [SerializeField] private LevelSettings[] _levels;

    public int Count => _levels.Length;

    public LevelSettings this[int index]
    {
        get
        {
            if(index >= _levels.Length)
                index = index % _levels.Length;
                
            return _levels[index];
        }
    }
}